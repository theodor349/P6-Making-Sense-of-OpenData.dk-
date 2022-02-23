using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using Shared.ComponentInterfaces;
using Shared.Models;
using System.IO;

namespace IntermediateGenerator
{
    public class IntermediateGenerator : IIntermediateGenerator
    {
        private readonly ILogger<IntermediateGenerator> _logger;

        public IntermediateGenerator(ILogger<IntermediateGenerator> logger)
        {
            _logger = logger;
        }
        public Task<DatasetObject> GenerateAsync()
        {
            ParseFile(new FileInfo("C:\\Users\\Emil-\\Desktop\\Dataset parking\\34.20.12_Parkeringsarealer.geojson"));
            return Task.FromResult(new DatasetObject());
        }
        public void ParseFile(FileInfo file)
        {
            if (file.Extension == ".geojson")
            {
                ParseJson(file);
            }
            else
            {
                _logger.LogError("Wrong file format " + file.Extension);
            }
        }

        private void ParseJson(FileInfo file)
        {
            string jsonString = File.ReadAllText(file.FullName);
            JObject jsonObject = JObject.Parse(jsonString);
            string prop = jsonObject.SelectToken("$.features.EJER").Value<string>();
            _logger.LogInformation(prop);
        }
    }
}

