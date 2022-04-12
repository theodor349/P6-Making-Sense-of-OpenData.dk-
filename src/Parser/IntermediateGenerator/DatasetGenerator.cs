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
        private readonly IJsonParser _jsonParser;
        private readonly IDatasetObjectSplitter _intermediateSplitter;
        private readonly ICsvParser _csvParser;

        public DatasetGenerator(IJsonParser parseJson, IDatasetObjectSplitter intermediate, ICsvParser csvParser)
        {
            _jsonParser = parseJson;
            _intermediateSplitter = intermediate;
            _csvParser = csvParser;
        }
        
        public async Task<DatasetObject> GenerateAsync(string filePath)
        {
            DatasetObject? datasetObject = null;
            var file = new FileInfo(filePath);
            switch (file.Extension.ToLower())
            {
                case ".geojson":
                case ".json":
                    datasetObject = await _jsonParser.Parse(File.ReadAllText(file.FullName), file.Extension, file.Name);
                    datasetObject = await _intermediateSplitter.SplitObject(datasetObject);
                    break;
                case ".csv":
                    datasetObject = await _csvParser.Parse(File.ReadAllText(file.FullName), file.Extension, file.Name);
                    break;
            }

            return datasetObject;
        }
    }
}

