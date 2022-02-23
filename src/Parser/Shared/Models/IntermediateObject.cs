using Shared.Models.ObjectAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Models
{
    public class IntermediateObject
    {
        public List<ObjectAttribute> Attributes { get; set; } = new List<ObjectAttribute>();
    }
}
