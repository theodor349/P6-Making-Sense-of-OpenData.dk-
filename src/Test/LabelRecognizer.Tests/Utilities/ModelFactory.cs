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

        internal static ObjectAttribute GetObjectAttr(ObjectLabel label)
        {
            switch (label)
            {
                case ObjectLabel.Null:
                    return new NullAttribute("name");
                case ObjectLabel.Text:
                    return new TextAttribute("name","Text");
                case ObjectLabel.Long:
                    return new LongAttribute("name", 32);
                case ObjectLabel.Double:
                    return new DoubleAttribute("name", 69.420);
                case ObjectLabel.Date:
                    return new DateAttribute("name", DateTime.Now);
                case ObjectLabel.List:
                    return new ListAttribute("name");
                default:
                    throw new Exception("Label was not found " + label.ToString());
            }
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
    }
}
