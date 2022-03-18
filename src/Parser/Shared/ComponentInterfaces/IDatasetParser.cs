using Shared.Models;

namespace Shared.ComponentInterfaces
{
    public interface IDatasetParser
    {
        Task Parse(DatasetObject dataset, DatasetType datasetType);
    }
}