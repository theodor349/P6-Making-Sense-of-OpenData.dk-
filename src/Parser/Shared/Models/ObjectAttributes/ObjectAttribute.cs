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
