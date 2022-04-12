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
        public ParseToJson() 
        {
            _configuration = null;
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
                    FindPolygonsInDataset(dataset)
                    ))
                );
            return GeoJson;
        }

        private List<JObject> FindPolygonsInDataset(DatasetObject dataset)
        {
            List<JObject> polygons = new List<JObject>();
            foreach(IntermediateObject obj in dataset.Objects)
            {
                foreach (ObjectAttribute objAttr in obj.Attributes)
                {
                    List<JObject> newPolygons = CheckObjAttrForPolygons(objAttr);
                    if(newPolygons != null)
                    {
                        polygons.AddRange(newPolygons);
                    }
                }
            }
                
            return polygons;  
        }
        
        private List<JObject> CheckObjAttrForPolygons(ObjectAttribute objAttr)
        {
            
            List<JObject> polygons = new List<JObject>();

            if (objAttr.Labels.Contains(new LabelModel(ObjectLabel.Polygon)))
            {
                polygons.Add(GetPolygon(objAttr));
            }

            else if (objAttr.Labels.Contains(new LabelModel(ObjectLabel.List)))
            {
                List<JObject> newPolygons = CheckObjAttrForPolygons((List<ObjectAttribute>)objAttr.Value);
                if (newPolygons != null)
                {
                    polygons.AddRange(newPolygons);
                }
            }


            return polygons;
        }

        private List<JObject> CheckObjAttrForPolygons(List<ObjectAttribute> objAttr)
        {
            List<JObject> polygons = new List<JObject>();
            foreach (ObjectAttribute obj in objAttr)
            {

                if (obj.Labels.Contains(new LabelModel(ObjectLabel.Polygon)))
                {
                    polygons.Add(GetPolygon(obj));
                }

                else if (obj.Labels.Contains(new LabelModel(ObjectLabel.List)))
                {
                    List<JObject> newPolygons = CheckObjAttrForPolygons((List<ObjectAttribute>)obj.Value);
                    if (newPolygons != null)
                    {
                        polygons.AddRange(newPolygons);
                    }
                }

            }
            return polygons;
        }


        private JObject GetPolygon(ObjectAttribute objAttr)
        {

            return new JObject(
                       new JProperty("type", "Feature"),
                       new JProperty("geometry", new JObject(
                                new JProperty("type", "Polygon"),
                                new JProperty("coordinates", GetCoordinates(objAttr)))
                           ),
                       new JProperty("properties", new JObject())
                   );
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
            //  !(under construction)
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

