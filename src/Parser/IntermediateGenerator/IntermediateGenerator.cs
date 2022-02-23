using Shared.Models;

namespace IntermediateGenerator
{
    public class IntermediateGenerator : IIntermediateGenerator
    {
        public Task<DatasetObject> GenerateAsync()
        {
            return Task.FromResult(new DatasetObject());
        }
    }
}

