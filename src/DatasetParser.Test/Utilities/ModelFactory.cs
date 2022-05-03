using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.Models;
using Shared.Models.ObjectAttributes;
using Newtonsoft.Json.Linq;
using System.Text.Json;

namespace DatasetParser.Test.Utilities
{
    internal class ModelFactory
    {
        public static DatasetObject GetDatasetObject(List<IntermediateObject> list)
        {
            return new DatasetObject(".json", "filename", list);
        }

        internal static List<IntermediateObject> GetIntermediateObjectList(int amount, Func<ObjectAttribute> getObjectAttr)
        {
            var res = new List<IntermediateObject>();

            for (int i = 0; i < amount; i++)
            {
                var intermediate = new IntermediateObject();
                intermediate.Attributes.Add(getObjectAttr());
                res.Add(intermediate);
            }
            return res;
        }

        //internal static ObjectAttribute GetObjectAttr(string name, double value, ObjectLabel label)
        //{
        //    switch (label)
        //    {
        //        case ObjectLabel.Null:
        //            return new NullAttribute(name);
        //        case ObjectLabel.Text:
        //            return new TextAttribute(name, "Text");
        //        case ObjectLabel.Long:
        //            return new LongAttribute(name, (long)value);
        //        case ObjectLabel.Double:
        //            return new DoubleAttribute(name, value);
        //        case ObjectLabel.Date:
        //            return new DateAttribute(name, DateTime.Now);
        //        case ObjectLabel.List:
        //            return new ListAttribute(name);
        //        default:
        //            throw new Exception("Label was not found " + label.ToString());
        //    }
        //}

        internal static JProperty ReturnPolygonProperty()
        {
            JArray coords = new JArray();
            coords.Add(new JArray(1.1, 1.2));
            coords.Add(new JArray(1.3, 1.4));
            coords.Add(new JArray(1.5, 1.6));
            coords.Add(new JArray(1.1, 1.2));
            JArray JArr = new JArray();
            JArr.Add(coords);
            JProperty JProp = new JProperty("coordinates", JArr);
            return JProp;
        }

        internal static JProperty ReturnLineProperty()
        {
            JArray coords = new JArray();
            coords.Add(new JArray(1.1, 1.2));
            coords.Add(new JArray(1.3, 1.4));
            coords.Add(new JArray(1.5, 1.6));
            JProperty JProp = new JProperty("coordinates", coords);
            return JProp;
        }

        internal static JProperty ReturnPointProperty()
        {
            JArray coords = new JArray(1.1, 1.2);
            JProperty JProp = new JProperty("coordinates", coords);
            return JProp;
        }

        internal static void AddCrs(DatasetObject inputDataset)
        {
            var crs = new CoordinateReferenceSystem(true);
            inputDataset.Properties.Add("CoordinateReferenceSystem", JsonSerializer.Serialize(crs));
        }
    }
}
