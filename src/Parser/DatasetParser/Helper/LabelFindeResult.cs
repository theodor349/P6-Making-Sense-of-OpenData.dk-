using Shared.Models.ObjectAttributes;

namespace DatasetParser.Helper
{
    public class LabelFindeResult
    {
        public Dictionary<string, SortedList<float, ObjectAttribute>> Result { get; private set; } = new();

        public void AddFind(string label, float probability, ObjectAttribute attribute)
        {
            if (Result.ContainsKey(label))
                Result[label].Add(probability, attribute);
            else
            {
                var list = new SortedList<float, ObjectAttribute>();
                list.Add(probability, attribute);
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
                return Result[label].Last().Value;
            else
                return null;

        }

        private void AddFind(string label, SortedList<float, ObjectAttribute> findings)
        {
            foreach (var find in findings)
            {
                AddFind(label, find.Key, find.Value);
            }
        }
    }
}
