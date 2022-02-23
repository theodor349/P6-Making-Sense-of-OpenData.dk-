using Shared.Models;

namespace DatasetDecider
{
    public interface IDatasetClassifier
    {
        Task Classify(DatasetObject dataset);
    }
}