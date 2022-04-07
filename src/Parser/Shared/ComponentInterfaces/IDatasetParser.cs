using Newtonsoft.Json.Linq;
using Shared.Models;

namespace Shared.ComponentInterfaces
{
    public interface IDatasetParser
    {
        Task<JObject> Parse(DatasetObject dataset, int iteration);
    }
}