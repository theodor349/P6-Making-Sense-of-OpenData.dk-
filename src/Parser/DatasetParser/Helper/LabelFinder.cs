using Shared.Models;
using Shared.Models.ObjectAttributes;

namespace DatasetParser.Helper
{
    public static class LabelFinder
    {
        public static LabelFindeResult FindLabels(IntermediateObject io, List<string> labels)
        {
            var result = new LabelFindeResult();
            foreach (var attribute in io.Attributes)
            {
                result.AddFindings(FindLabels(attribute, labels));
            }
            return result;
        }

        private static LabelFindeResult FindLabels(ObjectAttribute attribute, List<string> labels)
        {
            var result = new LabelFindeResult();
            foreach (var label in labels)
            {
                if (attribute.HasLabel(label))
                    result.AddFind(label, attribute.GetLabel(label).Probability, attribute);
            }

            if (attribute is ListAttribute)
            {
                var list = (List<ObjectAttribute>)attribute.Value;
                foreach (var a in list)
                {
                    result.AddFindings(FindLabels(a, labels));
                }
            }

            return result;
        }
    }
}
