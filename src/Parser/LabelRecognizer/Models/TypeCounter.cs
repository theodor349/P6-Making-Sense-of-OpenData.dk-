using Shared.Models.ObjectAttributes;

namespace LabelRecognizer.Models
{
    internal class TypeCounter
    {
        public string AttrName { get; set; }
        public List<TypeCounter> Types { get; } = new List<TypeCounter>();
        public bool IsList => Types.Count > 1;
        public Dictionary<ObjectLabel, long> Counter { get; } = new Dictionary<ObjectLabel, long>();

        public TypeCounter(string attrName)
        {
            AttrName = attrName;
        }

        public void Increment(ObjectLabel type)
        {
            if (Counter.ContainsKey(type))
                Counter[type]++;
            else
                Counter.Add(type, 1);
        }

        internal TypeCounter Get(string attrName)
        {
            var res = Types.FirstOrDefault(x => x.AttrName == attrName);
            if (res == null)
            {
                res = new TypeCounter(attrName);
                Types.Add(res);
            }
            return res;
        }
    }
}
