using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Models.Output
{
    public class LineString : GeoFeature
    {
        public List<Point> Coordinates { get; set; } = new List<Point>();
    }
}
