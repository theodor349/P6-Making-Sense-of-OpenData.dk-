using System;
using System.Text.Json;
using Shared.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft;
using Shared.Models;
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
                new JProperty("type", "Multipolygon"),
                new JArray("coordinates", 
                    FindPolygonsInDataset(dataset)
                    )
                );


            File.WriteAllText(_configuration["Output:JsonText"], GeoJson.ToString());

        }
        private List<JArray> FindPolygonsInDataset(DatasetObject dataset)
        {
            List<JArray> polygons = new List<JArray>();
            foreach(IntermediateObject obj in dataset.Objects)
            {
                foreach (ObjectAttribute objAttr in obj.Attributes)
                {
                    List<JArray> newPolygons = CheckObjAttrForPolygons(objAttr);
                    if(newPolygons != null)
                    {
                        polygons.AddRange(newPolygons);
                    }
                }
            }
                
            return polygons;  
        }

        private List<JArray> CheckObjAttrForPolygons(ObjectAttribute objAttr)
        {
            List<JArray> polygons = new List<JArray>();
            if (objAttr.Labels.Contains(new LabelModel(ObjectLabel.List)))
            {
                List<JArray> newPolygons = CheckObjAttrForPolygons((List<ObjectAttribute>)objAttr.Value);
                if (newPolygons != null)
                {
                    polygons.AddRange(newPolygons);
                }
            }
            else if (objAttr.Labels.Contains(new LabelModel(ObjectLabel.Polygon)))
            {
                polygons.Add(GetPolygon(objAttr));
            }

            return polygons;
        }

        private List<JArray> CheckObjAttrForPolygons(List<ObjectAttribute> objAttr)
        {
            List<JArray> polygons = new List<JArray>();
            foreach (ObjectAttribute obj in objAttr)
            {
                if (obj.Labels.Contains(new LabelModel(ObjectLabel.List)))
                {
                    List<JArray> newPolygons = CheckObjAttrForPolygons((List<ObjectAttribute>)obj.Value);
                    if (newPolygons != null)
                    {
                        polygons.AddRange(newPolygons);
                    }
                }
                else if (obj.Labels.Contains(new LabelModel(ObjectLabel.Polygon)))
                {
                    polygons.Add(GetPolygon(obj));
                }
            }
            return polygons;
        }


        private JArray GetPolygon(ObjectAttribute objAttr)
        {
            return new JArray(GetCoordinates(objAttr));
        }

        private JArray GetCoordinates(ObjectAttribute objAttr)
        {
            JArray coordinates = new JArray();
            foreach(ObjectAttribute coord in (List<ObjectAttribute>)objAttr.Value)
            {
                GetCoordinate(coord, coordinates);
            }

            return coordinates;
        }

        private void GetCoordinate(ObjectAttribute coord, JArray coordinates)
        {
            var coord1 = (ListAttribute)coord;
            var coordValues = (List<ObjectAttribute>)coord.Value;
            coordinates.Add(new JArray(coordValues[0], coordValues[1]));
        }
    }
}

