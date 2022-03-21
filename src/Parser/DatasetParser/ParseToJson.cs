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
                polygons.Add(CheckObjectForCoordsPolygons(obj, polygons));
            }
                
            return polygons;  
        }

        private J CheckObjectForCoordsPolygons(IntermediateObject obj, List<JArray> polygons)
        {
            foreach(ObjectAttribute attr in obj.Attributes)
            {
                if (attr.Labels.Contains(new LabelModel(ObjectLabel.Polygon)))
                {
                    polygons.Add(new JArray("coordinates", GetPolygon(attr)));
                }
            }
        }

        private JArray GetPolygon(ObjectAttribute attr)
        {
            JArray polygon = new JArray(GetCoordinates(attr));
            return polygon;
        }
    }
}

