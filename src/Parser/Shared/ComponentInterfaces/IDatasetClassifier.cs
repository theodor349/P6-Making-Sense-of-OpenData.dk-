using Shared.Models;

namespace Shared.ComponentInterfaces
{
    public interface IDatasetClassifier
    {
        Task Classify(DatasetObject dataset);
    }
}