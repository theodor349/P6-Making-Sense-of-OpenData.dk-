using Shared.Models.ObjectAttributes;

namespace LabelRecognizer.Models
{
    internal class TypeCounter
    {
        public string AttrName { get; set; }
        public List<TypeCounter> Types { get; } = new List<TypeCounter>();
        public Dictionary<ObjectLabel, long> Counter { get; } = new Dictionary<ObjectLabel, long>();

        private int parsableStrings = 0;
        public bool CanParseTextAsOtherType () => Counter.Count == 2 && Counter.ContainsKey(ObjectLabel.Text) && parsableStrings == Counter[ObjectLabel.Text];
  
        public bool ContainsOnlyDoubleAndLong() => Counter.Count == 2 && Counter.ContainsKey(ObjectLabel.Long) && Counter.ContainsKey(ObjectLabel.Double);

        public bool ContainsNullAndOtherType() => Counter.Count == 2 && Counter.ContainsKey(ObjectLabel.Null);

        public bool ContainsTextAndOtherType() => Counter.Count == 2 && Counter.ContainsKey(ObjectLabel.Text);

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

        public void CheckStringParse(ObjectAttribute attribute)
        {
            var textAttr = attribute as TextAttribute;
            if (textAttr != null)
            {
                if (Counter.Count == 2)
                {
                    ObjectLabel tryParseType = ObjectLabel.Null;
                    foreach (var KeyValuePair in Counter)
                    {
                        if (KeyValuePair.Key != ObjectLabel.Text)
                            tryParseType = KeyValuePair.Key;
                    }

                    string text = (string)attribute.Value;
                    switch (tryParseType)
                    {
                        case ObjectLabel.Long:
                            if (long.TryParse(text, out long test))
                            {
                                parsableStrings++;
                            }
                            break;
                        case ObjectLabel.Double:
                            if (double.TryParse(text, out double test2))
                            {
                                parsableStrings++;
                            }
                            break;
                        case ObjectLabel.Date:
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

        public ObjectLabel GetNotNullType()
        {
            foreach (var KeyValuePair in Counter)
            {
                if (KeyValuePair.Key != ObjectLabel.Null)
                    return KeyValuePair.Key;
            }
            throw new NullReferenceException("Expected atleast one other type than null in TypeCounter");
        }

        public ObjectLabel GetNotTextType()
        {
            foreach (var KeyValuePair in Counter)
            {
                if (KeyValuePair.Key != ObjectLabel.Text)
                    return KeyValuePair.Key;
            }
            throw new NullReferenceException("Expected atleast one other type than Text in TypeCounter");
        }
    }
}