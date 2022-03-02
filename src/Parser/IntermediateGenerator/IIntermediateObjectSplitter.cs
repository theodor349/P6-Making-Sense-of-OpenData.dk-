using Shared.Models;

namespace IntermediateGenerator
{
    public interface IIntermediateObjectSplitter
    {
        DatasetObject SplitObject(DatasetObject dataset);
    }
}