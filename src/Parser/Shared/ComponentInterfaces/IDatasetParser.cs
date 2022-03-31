using Shared.Models;

namespace Shared.ComponentInterfaces
{
    public interface IDatasetParser
    {
        Task<string> Parse(DatasetObject dataset, DatasetType datasetType, int iteration);
    }
}