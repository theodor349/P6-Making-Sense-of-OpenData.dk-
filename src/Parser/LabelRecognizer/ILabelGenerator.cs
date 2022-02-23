using Shared.Models;

namespace LabelRecognizer
{
    public interface ILabelGenerator
    {
        Task AddLabels(DatasetObject dataset);
    }
}