using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Models.Output.Specializations
{
    public class RouteSpecialization : GeodataOutput<LineString>
    {
        public string Description { get; set; }
        public string Name { get; set; }
    }
}
