using IntermediateGenerator.ParseFile;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using Shared.ComponentInterfaces;
using Shared.Models;
using System.IO;

namespace IntermediateGenerator
{
    public class IntermediateGenerator : IIntermediateGenerator
    {
        private readonly IParseJson _parseJson;

        public IntermediateGenerator(IParseJson parseJson)
        {
            _parseJson = parseJson;
        }
        
        public Task<DatasetObject> GenerateAsync(string filePath)
        {
            var file = new FileInfo(filePath);
            switch (file.Extension.ToLower())
            {
                case "geojson":
                    return _parseJson.Parse(file);
                    
            }
            throw new NotImplementedException("File extension `" + file.Extension + "` not found");
        }
    }
}

