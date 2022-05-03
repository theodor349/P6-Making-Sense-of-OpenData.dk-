using Shared.Models;
using Shared.Models.ObjectAttributes;

namespace LabelRecognizer.Models
{
    internal class TypeCounter
    {
        public string AttrName { get; set; }
        public List<TypeCounter> Types { get; } = new List<TypeCounter>();
        public Dictionary<string, long> Counter { get; } = new Dictionary<string, long>();

        private int parsableStrings = 0;
        public bool CanParseTextAsOtherType () => Counter.Count == 2 && Counter.ContainsKey(PredefinedLabels.Text) && parsableStrings == Counter[PredefinedLabels.Text];
  
        public bool ContainsOnlyDoubleAndLong() => Counter.Count == 2 && Counter.ContainsKey(PredefinedLabels.Long) && Counter.ContainsKey(PredefinedLabels.Double);

        public bool ContainsNullAndOtherType() => Counter.Count == 2 && Counter.ContainsKey(PredefinedLabels.Null);

        public bool ContainsTextAndOtherPrimitiveType() => Counter.Count == 2 && Counter.ContainsKey(PredefinedLabels.Text) && !Counter.ContainsKey(PredefinedLabels.List);

        public TypeCounter(string attrName)
        {
            AttrName = attrName;
        }

        public void Increment(string type)
        {
            if (Counter.ContainsKey(type))
                Counter[type]++;
            else
                Counter.Add(type, 1);
        }

        public void CheckStringParse(ObjectAttribute attribute)
        {
            var textAttr = attribute as TextAttribute;
            if (textAttr != null)
            {
                if (Counter.Count == 2)
                {
                    string tryParseType = PredefinedLabels.Null;
                    foreach (var KeyValuePair in Counter)
                    {
                        if (KeyValuePair.Key != PredefinedLabels.Text)
                            tryParseType = KeyValuePair.Key;
                    }

                    string text = (string)attribute.Value;
                    switch (tryParseType)
                    {
                        case PredefinedLabels.Long:
                            if (long.TryParse(text, out long test))
                            {
                                parsableStrings++;
                            }
                            break;
                        case PredefinedLabels.Double:
                            if (double.TryParse(text, out double test2))
                            {
                                parsableStrings++;
                            }
                            break;
                        case PredefinedLabels.Date:
                            break;
                    }
                }
            }
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

        public string GetNotNullType()
        {
            foreach (var KeyValuePair in Counter)
            {
                if (KeyValuePair.Key != PredefinedLabels.Null)
                    return KeyValuePair.Key;
            }
            throw new NullReferenceException("Expected atleast one other type than null in TypeCounter");
        }

        public string GetNotTextType()
        {
            foreach (var KeyValuePair in Counter)
            {
                if (KeyValuePair.Key != PredefinedLabels.Text)
                    return KeyValuePair.Key;
            }
            throw new NullReferenceException("Expected atleast one other type than Text in TypeCounter");
        }
    }
}