using Shared.Models.ObjectAttributes;

namespace LabelRecognizer.Models
{
    internal class TypeCounter
    {
        public string AttrName { get; set; }
        public List<TypeCounter> Types { get; } = new List<TypeCounter>();
        public Dictionary<ObjectLabel, long> Counter { get; } = new Dictionary<ObjectLabel, long>();

        public bool ContainsOnlyDoubleAndLong() => Counter.Count == 2 && Counter.ContainsKey(ObjectLabel.Long) && Counter.ContainsKey(ObjectLabel.Double);

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
