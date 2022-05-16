using Shared.Models;
using Shared.Models.ObjectAttributes;

namespace DatasetParser.Helper
{
    public class LabelResult : IComparable<LabelResult>
    {
        public float Probability { get; set; }
        public int Count { get; set; }
        public ObjectAttribute Attribute { get; set; }

        public LabelResult(float probability, int count, ObjectAttribute attribute)
        {
            Probability = probability;
            Count = count;
            Attribute = attribute;
        }

        public int CompareTo(LabelResult? other)
        {
            if(Count == other.Count)
                return Probability.CompareTo(other.Probability);
            else 
                return Count.CompareTo(other.Count);
        }
    }

    public class LabelFindeResult
    {
        public Dictionary<string, List<LabelResult>> Result { get; private set; } = new();

        public void AddFind(string label, float probability, int count, ObjectAttribute attribute)
        {
            if (Result.ContainsKey(label))
                Result[label].Add(new LabelResult(probability, count, attribute));
            else
            {
                var list = new List<LabelResult>();
                list.Add(new LabelResult(probability, count, attribute));
                Result.Add(label, list);
            }
        }

        internal void AddFindings(LabelFindeResult labelFindings)
        {
            foreach (var finding in labelFindings.Result)
            {
                AddFind(finding.Key, finding.Value);
            }
        }

        internal ObjectAttribute BestFit(string label)
        {
            if (Result.ContainsKey(label))
            {
                Result[label].Sort();
                return Result[label].Last().Attribute;
            }
            else
                return null;

        }

        private void AddFind(string label, List<LabelResult> findings)
        {
            foreach (var find in findings)
            {
                AddFind(label, find.Probability, find.Count, find.Attribute);
            }
        }
    }
}
