using Shared.Models;

namespace Shared.ComponentInterfaces
{
    public interface IDatasetClassifier
    {
        Task<DatasetType> Classify(DatasetObject dataset);
    }
}