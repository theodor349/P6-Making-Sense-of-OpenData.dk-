using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Models
{
    public class DatasetObject
    {
        public string originalExtensionName { get; }
        public string originalName { get; }
        public DatasetObject(string originalExtensionName, string originalName)
        {
            this.originalExtensionName = originalExtensionName;
            this.originalName = originalName;
        }
       
        public List<IntermediateObject> Objects { get; set; } = new List<IntermediateObject>();
        public IntermediateObject? ObjectSchema { get; set; }
    }
}
