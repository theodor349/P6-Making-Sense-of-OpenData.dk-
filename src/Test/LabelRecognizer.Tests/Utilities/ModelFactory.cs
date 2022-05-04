using Shared.Models;
using Shared.Models.ObjectAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabelRecognizer.Tests.Utilities
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

        internal static DoubleAttribute GetObjectAttr(string name, double value)
        {
            return new DoubleAttribute(name, value);            
        }

        internal static ListAttribute GetListAttribute(string name, ObjectAttribute obj1, ObjectAttribute obj2)
        {
            var list = new List<ObjectAttribute> { obj1, obj2 };
            return new ListAttribute(name, list);
        }

        internal static ObjectAttribute GetListAttribute(string name, List<ObjectAttribute> attributes)
        {
            return new ListAttribute(name, attributes);
        }

        internal static IntermediateObject GetIntermediateObject(List<ObjectAttribute> attributes)
        {
            return new IntermediateObject(attributes);
        }
        internal static IntermediateObject GetIntermediateObject(ObjectAttribute attribute)
        {
            return new IntermediateObject(new List<ObjectAttribute>() { attribute });
        }

        internal static List<ObjectAttribute> GetObjectAttrList(int num, Func<ObjectAttribute> getObjectAttribute)
        {
            var list = new List<ObjectAttribute>();
            for (int i = 0; i < num; i++)
            {
                list.Add(getObjectAttribute());
            }
            return list;
        }

        internal static IntermediateObject GetIntermediateObject(ObjectAttribute attr1, ObjectAttribute attr2)
        {
            var list = new List<ObjectAttribute>()
            {
                attr1,
                attr2,
            };
            return new IntermediateObject(list);
        }

        internal static ListAttribute GetListAttribute(string name, ObjectAttribute objectAttribute)
        {
            return new ListAttribute(name, new List<ObjectAttribute>() { objectAttribute });
        }

        internal static List<IntermediateObject> GetIntermediateObjectList(int amount1, Func<ObjectAttribute> getObjectAttr1, int amount2, Func<ObjectAttribute> getObjectAttr2)
        {
            var res = new List<IntermediateObject>();
            for (int i = 0; i < amount1; i++)
            {
                var intermediate = new IntermediateObject();
                intermediate.Attributes.Add(getObjectAttr1());
                res.Add(intermediate);
            }
            for (int i = 0; i < amount2; i++)
            {
                var intermediate = new IntermediateObject();
                intermediate.Attributes.Add(getObjectAttr2());
                res.Add(intermediate);
            }
            return res;
        }

        internal static ObjectAttribute GetObjectAttr(string name, string label)
        {
            switch (label)
            {
                case PredefinedLabels.Null:
                    return new NullAttribute(name);
                case PredefinedLabels.Text:
                    return new TextAttribute(name, "Text");
                case PredefinedLabels.Long:
                    return new LongAttribute(name, 32);
                case PredefinedLabels.Double:
                    return new DoubleAttribute(name, 69.420);
                case PredefinedLabels.Date:
                    return new DateAttribute(name, DateTime.Now);
                case PredefinedLabels.List:
                    return new ListAttribute(name);
                default:
                    throw new Exception("Label was not found " + label.ToString());
            }
        }

        internal static ObjectAttribute GetObjectAttr(string label)
        {
            return GetObjectAttr("name", label);
        }

        internal static IntermediateObject CreateNested(int nestings, string buttomObject)
        {
            var attr = GetNestedAttr(nestings, buttomObject);
            return new IntermediateObject(new List<ObjectAttribute>() { attr });
        }

        private static ObjectAttribute GetNestedAttr(int nestings, string buttomObject)
        {
            if(nestings == 1)
                return GetObjectAttr(buttomObject);
            else
            {
                var list = new List<ObjectAttribute>();
                list.Add(GetNestedAttr(--nestings, buttomObject));
                return new ListAttribute("name", list);
            }

        }

        internal static ObjectAttribute GetListOfPointsAttr(int amountPoints)
        {
            var res = new ListAttribute("listOfPoints");
            var list = (List<ObjectAttribute>)res.Value;
            for (int i = 0; i < amountPoints; i++)
            {
                list.Add(GetCoordinateAttr(i + 0.1234d, i * 2 + 0.1234d));
            }
            return res;
        }

        internal static ListAttribute GetPolygonAttr(int amountPoints)
        {
            var res = new ListAttribute("Polygon");
            var list = (List<ObjectAttribute>)res.Value;
            for (int i = 0; i < amountPoints; i++)
            {
                list.Add(GetCoordinateAttr(i));
            }
            list[list.Count - 1] = list.First();
            return res;
        }

        internal static ListAttribute GetCoordinateAttr(int seed)
        {
            var rnd = new Random(seed);
            return GetCoordinateAttr(rnd.NextDouble(), rnd.NextDouble());
        }

        internal static ListAttribute GetCoordinateAttr(double latitude, double longitude)
        {
            var latitudeO = GetObjectAttr("FloatValue", latitude);
            var longitudeO = GetObjectAttr("FloatValue", longitude);
            var res = GetListAttribute("coordinates", latitudeO, longitudeO);
            return res;
        }
    }
}
