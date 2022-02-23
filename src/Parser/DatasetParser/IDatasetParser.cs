using Shared.Models;

namespace DatasetParser
{
    public interface IDatasetParser
    {
        Task Parse(DatasetObject dataset);
    }
}