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

        public IntermediateObject(List<ObjectAttribute> attributes)
        {
            Attributes = attributes;
        }
        public IntermediateObject()
        {
        }
        public IntermediateObject(ListAttribute attributes)
        {
            foreach (ObjectAttribute attribute in (List<ObjectAttribute>)attributes.Value)
            {
                Attributes.Add(attribute);
            }
        }
    }
}
