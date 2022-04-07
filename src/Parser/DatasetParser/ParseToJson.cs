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
        JObject ParseIntermediateToJson(DatasetObject datasetObject, int iteration);
    }


    public class ParseToJson : IParseToJson
    {
        private readonly IConfiguration _configuration;

        private Shared.Models.GenericCoordinate firstCoordinate;

        private string geographicFormat = null;
        private string utmZoneLetter = null;
        private int utmZoneNumber = int.MaxValue;

        public ParseToJson(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public JObject ParseIntermediateToJson(DatasetObject dataset, int iteration)
        {
            // parse data
            //var json = JsonSerializer.Serialize(dataset);
            foreach (var prop in dataset.Properties)
            {
                if (prop.name == "geographicFormat")
                {
                    geographicFormat = prop.value;
                    
                }
                else if (prop.name == "utmZoneLetter")
                {
                    utmZoneLetter = prop.value;
                }
                else if (prop.name == "utmZoneNumber")
                {
                    utmZoneNumber = int.Parse(prop.value);
                }
            }
            
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
            else
            {
                JArray jArray = new JArray();
                List<GenericCoordinate> coords = new List<GenericCoordinate>();

                foreach (var coord in (List<ObjectAttribute>)objAttr.Value)
                {
                    coords.Add(new GenericCoordinate(coord, geographicFormat, utmZoneLetter, utmZoneNumber));
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
            if (geographicFormat == "utm" && utmZoneLetter != null)
            {
                genericCoord = new GenericCoordinate(coord, geographicFormat, utmZoneLetter, utmZoneNumber);
            }
            else
            {
                 genericCoord = new GenericCoordinate(coord);
            }
            return genericCoord;
        }
    }
}

