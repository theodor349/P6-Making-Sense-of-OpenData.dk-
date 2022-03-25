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
        void ParseIntermediateToJson(DatasetObject datasetObject, int iteration);
    }

    public struct Coordinate
    {
        public float lattitude;
        public float longitude;
    }

    public class ParseToJson : IParseToJson
    {
        private readonly IConfiguration _configuration;


        public ParseToJson(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ParseIntermediateToJson(DatasetObject dataset, int iteration)
        {
            // parse data
            //var json = JsonSerializer.Serialize(dataset);
            var GeoJson = new JObject(
                new JProperty("type", "FeatureCollection"),
                new JProperty("features", new JArray(
                    FindPolygonsInDataset(dataset)
                    ))
                );
            string output = _configuration["Output:JsonText"] + iteration.ToString() + ".geojson";
            File.WriteAllText(output, GeoJson.ToString());
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
            List<Coordinate> coords = new List<Coordinate>();

            foreach (var coord in (List<ObjectAttribute>) objAttr.Value)
            {
                coords.Add(GetCoordinate(coord));
            }
            SortAccordingToRightHandRule(coords);

            foreach(Coordinate coord in coords)
            {
                coordinates.Add(new JArray(coord.lattitude, coord.longitude));
            }

            jArray.Add(coordinates);
            return jArray;
        }

        private Coordinate GetCoordinate(ObjectAttribute coord)
        {
            var coordValues = (List<ObjectAttribute>)coord.Value;
            float coord1 = Convert.ToSingle(coordValues[0].Value);
            float coord2 = Convert.ToSingle(coordValues[1].Value);

            return new Coordinate{ lattitude = coord1, longitude = coord2};
        }

        private void SortAccordingToRightHandRule(List<Coordinate> coords)
        {

        }


    }
}

