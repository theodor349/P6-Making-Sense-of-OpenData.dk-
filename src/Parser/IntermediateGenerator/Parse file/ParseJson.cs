using Newtonsoft.Json.Linq;
using Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.ComponentInterfaces;
using Microsoft.Extensions.Logging;

namespace IntermediateGenerator.Parse_file
{
    internal class ParseJson
    {
        private readonly ILogger<IntermediateGenerator> _logger;
        public ParseJson(ILogger<IntermediateGenerator> logger)
        {
            _logger = logger;
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
            string prop = (string)jsonObject.SelectToken("$.features[0].properties.EJER");
            _logger.LogInformation(prop);
        }
    }
}
