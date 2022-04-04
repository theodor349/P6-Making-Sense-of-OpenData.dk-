using Shared.Models;
using Shared.Models.ObjectAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntermediateGenerator.Models
{
    internal class Pattern : IComparable<Pattern>
    {
        public int Depth { get; }
        public int Count { get; set; } = 1;
        public List<PatternItem> Signature { get; } = new List<PatternItem>();

        public Pattern(ListAttribute list, int depth)
        {
            Depth = depth;
            foreach (ObjectAttribute item in (List<ObjectAttribute>)list.Value)
            {
                GeneratePatternItem(item);
            }
        }
        public Pattern(IntermediateObject obj, int depth)
        {
            Depth = depth;
            foreach (ObjectAttribute item in obj.Attributes)
            {
                GeneratePatternItem(item);
            }
        }

        private void GeneratePatternItem(ObjectAttribute item)
        {
            string name = item.Name;
            PatternType type = PatternType.@null;
            switch (item)
            {
                case TextAttribute:
                    type = PatternType.@string;
                    break;
                case LongAttribute:
                    type = PatternType.@int;
                    break;
                case DoubleAttribute:
                    type = PatternType.@double;
                    break;
                case DateAttribute:
                    type = PatternType.date;
                    break;
                case NullAttribute:
                    type = PatternType.@null;
                    break;
                case ListAttribute:
                    type = PatternType.@list;
                    break;
            }
            Signature.Add(new PatternItem() { Name = name, Type = type});
        }
        public override bool Equals(object? obj)
        {
            if (obj == null) return false;

            Pattern? pattern = obj as Pattern;

            if (pattern != null)
                return Equals(pattern);
            else return false;
        }
        private bool Equals(Pattern pattern)
        {
            if (pattern.Depth != Depth)
                return false;

            if (pattern.Signature.Count != Signature.Count) {
                return false;
            }
            
            int counter = 0;
            foreach (PatternItem item in pattern.Signature)
            {
                if (item.Name != Signature[counter].Name || item.Type != Signature[counter].Type)
                {
                    return false;
                }
                counter++;
            }
            return true;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public int CompareTo(Pattern? other)
        {
            if (other != null)
            {
                if (Depth == other.Depth)
                    return 0;
                else if (other.Depth < Depth)
                    return 1;
                else return -1;
            }
            throw new NullReferenceException();
        }
    }
}
