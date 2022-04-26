using Newtonsoft.Json.Linq;
using Shared.Models;
using Shared.Models.Output;

namespace Shared.ComponentInterfaces
{
    public interface IDatasetParser
    {
        Task<OutputDataset> Parse(DatasetObject dataset, int iteration);
    }
}