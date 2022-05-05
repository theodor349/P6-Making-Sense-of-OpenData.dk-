using Shared.Models.Output.Specializations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Models.Output
{
    public abstract class IntermediateOutput
    {
        public List<SpecializationProperty> Properties { get; set; } = new();
    }
}
