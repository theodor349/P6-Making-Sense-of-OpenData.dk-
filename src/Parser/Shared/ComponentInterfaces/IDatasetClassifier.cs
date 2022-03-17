using Shared.Models;

namespace Shared.ComponentInterfaces
{
    public interface IDatasetClassifier
    {
        DatasetType Classify(DatasetObject dataset);
    }
}