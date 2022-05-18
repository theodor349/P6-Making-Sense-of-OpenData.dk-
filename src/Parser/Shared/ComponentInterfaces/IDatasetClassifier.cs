using Shared.Models;

namespace Shared.ComponentInterfaces
{
    public interface IDatasetClassifier
    {
        Task<OutputLogObject> Classify(DatasetObject dataset, int iteration);
    }
}