namespace Shared.Models.ObjectAttributes
{
    public abstract class ObjectAttribute
    {
        public string Name { get; }
        public object Value { get; }

        public ObjectAttribute(string name, object value)
        {
            Name = name;
            Value = value;
        }
    }

    public class IntegerAttribute : ObjectAttribute
    {
        public IntegerAttribute(string name, int value) : base(name, value)
        {
        }
    }

    public class DoubleAttribute : ObjectAttribute
    {
        public DoubleAttribute(string name, double value) : base(name, value)
        {
        }
    }


    public class TextAttribute : ObjectAttribute
    {
        public TextAttribute(string name, string value) : base(name, value)
        {
        }
    }

    public class ListAttribute : ObjectAttribute
    {
        public ListAttribute(string name) : base(name, new List<ObjectAttribute>())
        {
        }
       
    }
}
