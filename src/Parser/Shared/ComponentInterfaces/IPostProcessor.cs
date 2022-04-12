using Shared.Models;

namespace Shared.ComponentInterfaces
{
    public interface IPostProcessor
    {
        Task Process(DatasetObject dataset);
    }
}