using Shared.Models;

namespace DatasetGenerator
{
    public interface IDatasetObjectSplitter
    {
        Task<DatasetObject> SplitObject(DatasetObject dataset);
    }
}