using Shared.Models;
using Shared.Models.ObjectAttributes;
using Shared.Models.Output.Specializations;

namespace DatasetParser.Helper
{
    public static class LabelFinder
    {
        public static LabelFindeResult FindLabels(IntermediateObject io, List<SpecializationPropertyDescription> labels)
        {
            var result = new LabelFindeResult();
            foreach (var attribute in io.Attributes)
            {
                result.AddFindings(FindLabels(attribute, labels));
            }
            return result;
        }

        private static LabelFindeResult FindLabels(ObjectAttribute attribute, List<SpecializationPropertyDescription> labels)
        {
            var result = new LabelFindeResult();

            foreach (var labelTarget in labels)
            {
                int count = 0;
                float propability = 1;
                foreach (var target in labelTarget.Targets)
                {
                    if (attribute.HasLabel(target))
                    {
                        propability *= attribute.GetLabel(target).Probability;
                        count++;
                    }
                }
                if (count > 0)
                    result.AddFind(labelTarget.Name, propability, count, attribute);
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
