using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using Shared.ComponentInterfaces;
using Shared.Models;
using System.IO;

namespace IntermediateGenerator
{
    public class IntermediateGenerator : IIntermediateGenerator
    {
        
        public Task<DatasetObject> GenerateAsync()
        {
            ParseFile(new FileInfo("C:\\Users\\Emil-\\Desktop\\Dataset parking\\34.20.12_Parkeringsarealer.geojson"));
            return Task.FromResult(new DatasetObject());
        }
    }
}

