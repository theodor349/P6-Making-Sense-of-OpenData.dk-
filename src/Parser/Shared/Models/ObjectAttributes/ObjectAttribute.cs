namespace Shared.Models.ObjectAttributes
{
    public enum ObjectLabel { Text = 0, Long = 1, Double = 2, Date = 3, List = 4, Null = 5, Coordinate = 6, Polygon = 7 }
    public class LabelModel
    {
        public ObjectLabel Label { get; set; }
        public float Probability { get; set; }

        public LabelModel(ObjectLabel label, float probability)
        {
            Label = label;
            Probability = probability;
        }

        public LabelModel(ObjectLabel label)
        {
            Label = label;
        }

        public override bool Equals(object? obj)
        {
            if (obj == null) return false;

            LabelModel? label = obj as LabelModel;

            if (label != null)
                return Equals(label);
            else return false;
        }
        private bool Equals(LabelModel label)
        {
            return Label == label.Label;
        }

        public override int GetHashCode()
        {
            return Label.GetHashCode();
        }
    }
    
    public abstract class ObjectAttribute
    {
        public string Name { get; }
        public object Value { get; }
        public List<LabelModel> Labels { get; set; } = new List<LabelModel>();

        public ObjectAttribute(string name, object value)
        {
            Name = name;
            Value = value;
        }

        public void AddLabel(ObjectLabel label, float probability)
        {
            Labels.Add(new LabelModel(label, probability));
        }

        public bool HasLabel(ObjectLabel label)
        {
           return GetLabel(label) != null;
        }
        public LabelModel? GetLabel(ObjectLabel label)
        {
            return Labels.FirstOrDefault(x => x.Label == label);
        }
    }

    public class LongAttribute : ObjectAttribute
    {
        public LongAttribute(string name, long value) : base(name, value)
        {
        }
    }

    public class DoubleAttribute : ObjectAttribute
    {
        public DoubleAttribute(string name, double value) : base(name, value)
        {
        }
    }

    public class DateAttribute : ObjectAttribute
    {
        public DateAttribute(string name, DateTime value) : base(name, value)
        {
        }
    }

    public class TextAttribute : ObjectAttribute
    {
        public TextAttribute(string name, string value) : base(name, value)
        {
        }
    }
    public class NullAttribute : ObjectAttribute
    {
        public NullAttribute(string name) : base(name, "null")
        {
        }
    }

    public class ListAttribute : ObjectAttribute
    {
        public ListAttribute(string name) : base(name, new List<ObjectAttribute>())
        {
        }

        public ListAttribute(string name, List<ObjectAttribute> list) : base(name, list)
        {  
        }
    }

}
