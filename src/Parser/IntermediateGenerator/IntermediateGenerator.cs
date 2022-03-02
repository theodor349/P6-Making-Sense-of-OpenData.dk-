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
        private readonly IIntermediateObjectSplitter _intermediate;

        public IntermediateGenerator(IParseJson parseJson, IIntermediateObjectSplitter intermediate)
        {
            _parseJson = parseJson;
            _intermediate = intermediate;
        }
        
        public async Task<DatasetObject> GenerateAsync(string filePath)
        {
            DatasetObject? datasetObject;
            var file = new FileInfo(filePath);
            switch (file.Extension.ToLower())
            {
                case ".geojson":
                case ".json":
                    datasetObject = await _parseJson.Parse(File.ReadAllText(file.FullName), file.Extension, file.Name);
                    break;

                default:
                    throw new NotImplementedException("File extension `" + file.Extension + "` not found");
            }

            return datasetObject;
        }
    }
}

