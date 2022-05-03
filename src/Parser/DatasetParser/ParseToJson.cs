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

        private CoordinateReferenceSystem? _crs;
        private DatasetType datasetType;
        private bool _useRightHandRule;

        public ParseToJson(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public ParseToJson() 
        {
            _configuration = null;
        }

        public JObject ParseDatasetObjectToJson(DatasetObject dataset, int iteration)
        {
            datasetType = dataset.DatasetType;
            if (dataset.HasProperty("CoordinateReferenceSystem"))
                _crs = JsonSerializer.Deserialize<CoordinateReferenceSystem>(dataset.GetProperty("CoordinateReferenceSystem"));
            
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
            else if (objAttr.Labels.Contains(new LabelModel(PredefinedLabels.List)))
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

                else if (obj.Labels.Contains(new LabelModel(PredefinedLabels.List)))
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
            if (objAttr.Labels.Contains(new LabelModel(PredefinedLabels.Polygon)))
            {
                _useRightHandRule = true;
                return GetObjectWithCoordinates(objAttr, PredefinedLabels.Polygon);
            }
            if (objAttr.Labels.Contains(new LabelModel(PredefinedLabels.LineString)))
            {
                return GetObjectWithCoordinates(objAttr, PredefinedLabels.LineString);
            }
            if (objAttr.Labels.Contains(new LabelModel(PredefinedLabels.MultiPoint)))
            {
                _useRightHandRule = true;
                return GetObjectWithCoordinates(objAttr, PredefinedLabels.MultiPoint);
            }
            if (objAttr.HasLabel(PredefinedLabels.Point))
            {
                return GetObjectWithCoordinates(objAttr, PredefinedLabels.Point);
            }
            else return null;
        }

        private JObject? RoutesAttrTagOrder(ObjectAttribute objAttr)
        {

            if (objAttr.HasLabel(PredefinedLabels.Polygon))
            {
                objAttr.Labels.Remove(new LabelModel(PredefinedLabels.Polygon));
                objAttr.AddLabel(PredefinedLabels.LineString, 1);
            }

            if (objAttr.Labels.Contains(new LabelModel(PredefinedLabels.LineString)))
            {
                return GetObjectWithCoordinates(objAttr, PredefinedLabels.LineString);
            }
            if (objAttr.Labels.Contains(new LabelModel(PredefinedLabels.MultiPoint)))
            {
                _useRightHandRule = true;
                return GetObjectWithCoordinates(objAttr, PredefinedLabels.MultiPoint);
            }
            if (objAttr.HasLabel(PredefinedLabels.Point))
            {
                return GetObjectWithCoordinates(objAttr, PredefinedLabels.Point);
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
            if (objAttr.Labels.Contains(new LabelModel(PredefinedLabels.Polygon)))
            {
                return new JProperty("coordinates", GetCoordinates(objAttr));
            }
            else if (objAttr.HasLabel(PredefinedLabels.Point))
            {
                GenericCoordinate coord = GetCoordinate(objAttr);
                JArray jArray = new JArray(coord.Longitude, coord.Latitude);
                return new JProperty("coordinates", jArray);
            }
            else
            {
                JArray jArray = new JArray();
                List<GenericCoordinate> coords = new List<GenericCoordinate>();

                foreach (var coord in (List<ObjectAttribute>)objAttr.Value)
                {
                    coords.Add(new GenericCoordinate(coord, _crs));
                }
                if(_useRightHandRule)
                    coords = GenericCoordinate.SortAccordingToRightHandRule(coords);
                foreach (var gCoord in coords)
                {
                    jArray.Add(new JArray(gCoord.Longitude, gCoord.Latitude));
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

            if(_useRightHandRule)
                coords = GenericCoordinate.SortAccordingToRightHandRule(coords);

            foreach(GenericCoordinate coord in coords)
            {
                coordinates.Add(new JArray(coord.Longitude, coord.Latitude));
            }

            jArray.Add(coordinates);
            return jArray;
        }

        private GenericCoordinate GetCoordinate(ObjectAttribute coord)
        {
            GenericCoordinate genericCoord;
            genericCoord = new GenericCoordinate(coord, _crs);
            return genericCoord;
        }
    }
}

