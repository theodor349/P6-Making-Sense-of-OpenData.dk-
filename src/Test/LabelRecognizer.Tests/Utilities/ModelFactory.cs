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

        internal static ObjectAttribute GetObjectAttr(string name, ObjectLabel label)
        {
            switch (label)
            {
                case ObjectLabel.Null:
                    return new NullAttribute(name);
                case ObjectLabel.Text:
                    return new TextAttribute(name, "Text");
                case ObjectLabel.Long:
                    return new LongAttribute(name, 32);
                case ObjectLabel.Double:
                    return new DoubleAttribute(name, 69.420);
                case ObjectLabel.Date:
                    return new DateAttribute(name, DateTime.Now);
                case ObjectLabel.List:
                    return new ListAttribute(name);
                default:
                    throw new Exception("Label was not found " + label.ToString());
            }
        }

        internal static ObjectAttribute GetObjectAttr(ObjectLabel label)
        {
            return GetObjectAttr("name", label);
        }

        internal static IntermediateObject CreateNested(int nestings, ObjectLabel buttomObject)
        {
            var attr = GetNestedAttr(nestings, buttomObject);
            return new IntermediateObject(new List<ObjectAttribute>() { attr });
        }

        private static ObjectAttribute GetNestedAttr(int nestings, ObjectLabel buttomObject)
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

        internal static ListAttribute GetPolygonAttr(int amountPoints)
        {
            var res = new ListAttribute("Polygon");
            var list = (List<ObjectAttribute>)res.Value;
            for (int i = 0; i < amountPoints; i++)
            {
                list.Add(GetCoordinateAttr(i));
            }
            return res;
        }

        internal static ListAttribute GetCoordinateAttr(int seed)
        {
            var rnd = new Random(seed);
            var latitude = GetObjectAttr("FloatValue", rnd.NextDouble());
            var longitude = GetObjectAttr("FloatValue", rnd.NextDouble());
            var res = GetListAttribute("coordinates", latitude, longitude);

            return res;
        }
    }
}
