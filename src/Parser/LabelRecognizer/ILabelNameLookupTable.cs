using Shared.Models;

namespace LabelRecognizer
{
    public interface ILabelNameLookupTable
    {
    Task AssignLabels(DatasetObject dataset);
    }
}