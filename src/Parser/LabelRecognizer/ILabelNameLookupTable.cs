using Shared.Models;

namespace LabelRecognizer
{
    public interface ILabelNameLookupTable
    {
    Task AssignLabels(DatasetObject dataset);
    bool IncludesTarget(TargetKey target, string name, LookupLanguages language);
    }
}