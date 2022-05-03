using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Models
{
    public static class PredefinedLabels
    {
        public const string Text = "Text";
        public const string Long = "Long";
        public const string Double = "Double";
        public const string Date = "Date";
        public const string List = "List";
        public const string Null = "Null";
        public const string Point = "Point";
        public const string Polygon = "Polygon";
        public const string Bool = "Bool";
        public const string LineString = "LineString";
        public const string MultiPoint = "MultiPoint";
        public const string Amount = "Amount";
        public const string Address = "Address";
        public const string Remark = "Remark";
        public const string Parking = "Parking";
        public const string Route = "Route";

        public static List<string> Labels { get
            {
                var t = typeof(string);
                return new List<string>(t.GetFields().Select(x => x.Name));           
            } 
        }

    }

}
