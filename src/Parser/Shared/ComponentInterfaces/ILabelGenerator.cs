using Shared.Models;

namespace Shared.ComponentInterfaces
{
    public interface ILabelGenerator
    {
        Task AddLabels(DatasetObject dataset);
    }
}