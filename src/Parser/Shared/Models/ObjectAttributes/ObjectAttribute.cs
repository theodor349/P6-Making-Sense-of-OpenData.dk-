﻿namespace Shared.Models.ObjectAttributes
{
    public enum ObjectLabel { Text = 0, Long = 1, Double = 2, Date = 4, List = 8, Null = 16 }
    public class LabelModel
    {
        public ObjectLabel Label { get; set; }
        public float Probability { get; set; }

        public LabelModel(ObjectLabel label, float probability)
        {
            Label = label;
            Probability = probability;
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
