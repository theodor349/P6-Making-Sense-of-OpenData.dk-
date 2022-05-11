using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Shared.Models
{
    public class DatasetObject
    {
        public string originalExtensionName { get; }
        public string originalName { get; }
        public DatasetType DatasetType { get; set; }

        public List<IntermediateObject> Objects { get; set; } = new List<IntermediateObject>();
        public IntermediateObject? ObjectSchema { get; set; }
        public Dictionary<string, string> Properties { get; set; } = new Dictionary<string, string>();
        public bool HasProperty(string key) => Properties.ContainsKey(key);
        public string GetProperty(string key) => Properties[key];
        public CoordinateReferenceSystem Crs => JsonSerializer.Deserialize<CoordinateReferenceSystem>(Properties["CoordinateReferenceSystem"]);

        public bool HasCrs => HasProperty("CoordinateReferenceSystem");

        public DatasetObject(string originalExtensionName, string originalName)
        {
            this.originalExtensionName = originalExtensionName;
            this.originalName = originalName;
        }
        public DatasetObject(DatasetObject dataset)
        {
            this.Properties = dataset.Properties;
            this.originalExtensionName = dataset.originalExtensionName;
            this.originalName = dataset.originalName;
        }


        public DatasetObject(string originalExtensionName, string originalName, List<IntermediateObject> objects)
        {
            this.originalExtensionName = originalExtensionName;
            this.originalName = originalName;
            Objects = objects;
        }

    }
}
