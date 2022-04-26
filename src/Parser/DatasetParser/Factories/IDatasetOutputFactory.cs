using Shared.Models;
using Shared.Models.Output;

namespace DatasetParser.Factories
{
    public interface IDatasetOutputFactory
    {
        Task<IntermediateOutput> BuildDataset(DatasetObject dataset, int iteration);
    }
}