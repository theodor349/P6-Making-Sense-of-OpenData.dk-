using Shared.Models;

namespace IntermediateGenerator
{
    public interface IIntermediateObjectSplitter
    {
        Task<DatasetObject> SplitObject(DatasetObject dataset);
    }
}