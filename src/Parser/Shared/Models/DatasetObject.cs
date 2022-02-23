using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Models
{
    internal class DatasetObject
    {
        public List<IntermediateObject> Objects { get; set; } = new List<IntermediateObject>();
        public IntermediateObject? ObjectSchema { get; set; }
    }
}
