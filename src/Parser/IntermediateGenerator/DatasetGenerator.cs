using DatasetGenerator.ParseFile;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using Shared.ComponentInterfaces;
using Shared.Models;
using System.IO;

namespace DatasetGenerator
{
    public class DatasetGenerator : IDatasetGenerator
    {
        private readonly IParseJson _parseJson;
        private readonly IDatasetObjectSplitter _intermediateSplitter;

        public DatasetGenerator(IParseJson parseJson, IDatasetObjectSplitter intermediate)
        {
            _parseJson = parseJson;
            _intermediateSplitter = intermediate;
        }
        
        public async Task<DatasetObject> GenerateAsync(string filePath)
        {
            DatasetObject? datasetObject = null;
            var file = new FileInfo(filePath);
            switch (file.Extension.ToLower())
            {
                case ".geojson":
                case ".json":
                    datasetObject = await _parseJson.Parse(File.ReadAllText(file.FullName), file.Extension, file.Name);
                    datasetObject = await _intermediateSplitter.SplitObject(datasetObject);
                    break;
            }

            return datasetObject;
        }
    }
}

