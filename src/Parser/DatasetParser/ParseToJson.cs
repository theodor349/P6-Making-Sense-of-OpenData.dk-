using System;
using System.Text.Json;
using Shared.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft;
using System.Linq;
using Newtonsoft.Json.Linq;
using Shared.Models.ObjectAttributes;
using CoordinateSharp;

namespace DatasetParser
{
    public interface IParseToJson
    {
        JObject ParseDatasetObjectToJson(DatasetObject datasetObject, int iteration);
    }


    public class ParseToJson : IParseToJson
    {
        private readonly IConfiguration _configuration;

        private Shared.Models.GenericCoordinate firstCoordinate;

        private CoordinateReferenceSystem? crs;
        private DatasetType datasetType;

        public ParseToJson(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public JObject ParseDatasetObjectToJson(DatasetObject dataset, int iteration)
        {
            datasetType = dataset.DatasetType;
            if (dataset.HasProperty("CoordinateReferenceSystem"))
                crs = JsonSerializer.Deserialize<CoordinateReferenceSystem>(dataset.GetProperty("CoordinateReferenceSystem"));
            
            var GeoJson = new JObject(
                new JProperty("type", "FeatureCollection"),
                new JProperty("features", new JArray(
                    FindGeoObjectsInDataset(dataset)
                    ))
                );
            return GeoJson;
        }

        private List<JObject> FindGeoObjectsInDataset(DatasetObject dataset)
        {
            List<JObject> GeoObjects = new List<JObject>();

            foreach(IntermediateObject obj in dataset.Objects)
            {
                foreach (ObjectAttribute objAttr in obj.Attributes)
                {
                    List<JObject> newPolygons = CheckObjAttrForGeoObjects(objAttr);
                    if(newPolygons != null)
                    {
                        GeoObjects.AddRange(newPolygons);
                    }
                }
            }
                
            return GeoObjects;  
        }
        
        private List<JObject> CheckObjAttrForGeoObjects(ObjectAttribute objAttr)
        {
            
            List<JObject> geoObjects = new List<JObject>();

            JObject geoObj = CheckObjAttrForTags(objAttr);
            if (geoObj != null)
            {
                geoObjects.Add(geoObj);
            }

            else if (objAttr.Labels.Contains(new LabelModel(ObjectLabel.List)))
            {
                List<JObject> newGeoObjects = CheckObjAttrForGeoObjects((List<ObjectAttribute>)objAttr.Value);
                if (newGeoObjects != null)
                {
                    geoObjects.AddRange(newGeoObjects);
                }
            }


            return geoObjects;
        }

        private List<JObject> CheckObjAttrForGeoObjects(List<ObjectAttribute> objAttr)
        {
            List<JObject> geoObjects = new List<JObject>();
            foreach (ObjectAttribute obj in objAttr)
            {

                JObject? geoObj = CheckObjAttrForTags(obj);
                if (geoObj != null)
                {
                    geoObjects.Add(geoObj);
                }

                else if (obj.Labels.Contains(new LabelModel(ObjectLabel.List)))
                {
                    List<JObject> newGeoObjects = CheckObjAttrForGeoObjects((List<ObjectAttribute>)obj.Value);
                    if (newGeoObjects != null)
                    {
                        geoObjects.AddRange(newGeoObjects);
                    }
                }
            }
            return geoObjects;
        }

        private JObject? CheckObjAttrForTags(ObjectAttribute objAttr)
        {
            switch (datasetType)
            {
                case DatasetType.Parking:
                    return ParkingAttrTagOrder(objAttr);
                case DatasetType.Routes:
                    return RoutesAttrTagOrder(objAttr);
            }
            throw new NullReferenceException();
        }

        private JObject? ParkingAttrTagOrder(ObjectAttribute objAttr)
        {
            if (objAttr.Labels.Contains(new LabelModel(ObjectLabel.Polygon)))
            {
                return GetObjectWithCoordinates(objAttr, ObjectLabel.Polygon.ToString());
            }
            if (objAttr.Labels.Contains(new LabelModel(ObjectLabel.LineString)))
            {
                return GetObjectWithCoordinates(objAttr, ObjectLabel.LineString.ToString());
            }
            if (objAttr.Labels.Contains(new LabelModel(ObjectLabel.MultiPoint)))
            {
                return GetObjectWithCoordinates(objAttr, ObjectLabel.MultiPoint.ToString());
            }
            if (objAttr.HasLabel(ObjectLabel.Point))
            {
                return GetObjectWithCoordinates(objAttr, ObjectLabel.Point.ToString());
            }
            else return null;
        }

        private JObject? RoutesAttrTagOrder(ObjectAttribute objAttr)
        {

            if (objAttr.HasLabel(ObjectLabel.Polygon))
            {
                objAttr.Labels.Remove(new LabelModel(ObjectLabel.Polygon));
                objAttr.AddLabel(ObjectLabel.LineString, 1);
            }

            if (objAttr.Labels.Contains(new LabelModel(ObjectLabel.LineString)))
            {
                return GetObjectWithCoordinates(objAttr, ObjectLabel.LineString.ToString());
            }
            if (objAttr.Labels.Contains(new LabelModel(ObjectLabel.MultiPoint)))
            {
                return GetObjectWithCoordinates(objAttr, ObjectLabel.MultiPoint.ToString());
            }
            if (objAttr.HasLabel(ObjectLabel.Point))
            {
                return GetObjectWithCoordinates(objAttr, ObjectLabel.Point.ToString());
            }
            else return null;
        }

        private JObject GetObjectWithCoordinates(ObjectAttribute objAttr, string typeName)
        {

            return new JObject(
                    new JProperty("type", "Feature"),
                    new JProperty("geometry",
                        new JObject(
                            new JProperty("type", typeName),
                            GetCoordinateProperty(objAttr)
                        )),
                    new JProperty("properties", new JObject())
                    );          
        }

        private JProperty GetCoordinateProperty(ObjectAttribute objAttr)
        {
            if (objAttr.Labels.Contains(new LabelModel(ObjectLabel.Polygon)))
            {
                return new JProperty("coordinates", GetCoordinates(objAttr));
            }
            else if (objAttr.HasLabel(ObjectLabel.Point))
            {
                GenericCoordinate coord = GetCoordinate(objAttr);
                JArray jArray = new JArray(coord.latitude, coord.longitude);
                return new JProperty("coordinates", jArray);
            }
            else
            {
                JArray jArray = new JArray();
                List<GenericCoordinate> coords = new List<GenericCoordinate>();

                foreach (var coord in (List<ObjectAttribute>)objAttr.Value)
                {
                    coords.Add(new GenericCoordinate(coord, crs));
                }
                coords = GenericCoordinate.SortAccordingToRightHandRule(coords);
                foreach (var gCoord in coords)
                {
                    jArray.Add(new JArray(gCoord.latitude, gCoord.longitude));
                }

                return new JProperty("coordinates", jArray);
            }
        }

        private JArray GetCoordinates(ObjectAttribute objAttr)
        {

            JArray jArray = new JArray();
            JArray coordinates = new JArray();
            List<GenericCoordinate> coords = new List<GenericCoordinate>();

            foreach (var coord in (List<ObjectAttribute>) objAttr.Value)
            {
                coords.Add(GetCoordinate(coord));
            }
            coords = GenericCoordinate.SortAccordingToRightHandRule(coords);

            foreach(GenericCoordinate coord in coords)
            {
                coordinates.Add(new JArray(coord.latitude, coord.longitude));
            }

            jArray.Add(coordinates);
            return jArray;
        }

        private GenericCoordinate GetCoordinate(ObjectAttribute coord)
        {
            GenericCoordinate genericCoord;
            genericCoord = new GenericCoordinate(coord, crs);
            return genericCoord;
        }
    }
}

