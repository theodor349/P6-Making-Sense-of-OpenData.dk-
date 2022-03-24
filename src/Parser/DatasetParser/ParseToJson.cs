using System;
using System.Text.Json;
using Shared.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft;
using System.Linq;
using Newtonsoft.Json.Linq;
using Shared.Models.ObjectAttributes;

namespace DatasetParser
{
    public interface IParseToJson
    {
        void ParseIntermediateToJson(DatasetObject datasetObject);
    }

    public class ParseToJson : IParseToJson
    {
        private readonly IConfiguration _configuration;


        public ParseToJson(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ParseIntermediateToJson(DatasetObject dataset)
        {
            // parse data
            //var json = JsonSerializer.Serialize(dataset);
            var GeoJson = new JObject(
                new JProperty("type", "FeatureCollection"),
                new JProperty("features", new JArray(
                    FindPolygonsInDataset(dataset)
                    ))
                );

            File.WriteAllText(_configuration["Output:JsonText"], GeoJson.ToString());
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
            foreach(ObjectAttribute coord in (List<ObjectAttribute>)objAttr.Value)
            {
                GetCoordinate(coord, coordinates);
            }
            jArray.Add(coordinates);
            return jArray;
        }

        private void GetCoordinate(ObjectAttribute coord, JArray coordinates)
        {
            var coordValues = (List<ObjectAttribute>)coord.Value;

            float coord1 = Convert.ToSingle(coordValues[0].Value);
            float coord2 = Convert.ToSingle(coordValues[1].Value);

            coordinates.Add(new JArray(coord1, coord2));
        }
    }
}

