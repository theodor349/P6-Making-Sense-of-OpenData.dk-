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
                List<JArray> newPolygons = CheckObjAttrForPolygons((ObjectAttribute)objAttr.Value);
                if (newPolygons != null)
                {
                    polygons.AddRange(newPolygons);
                }
            }
            else if (objAttr.Labels.Contains(new LabelModel(ObjectLabel.Polygon)))
            {
                polygons.Add(GetCoordinates(objAttr));
            }

            return polygons;
        }

        private JArray GetCoordinates(ObjectAttribute objAttr)
        {
            throw new NotImplementedException();
        }
    }
}

