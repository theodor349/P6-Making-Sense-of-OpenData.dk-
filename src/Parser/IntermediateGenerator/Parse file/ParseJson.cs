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
        private readonly ILogger<ParseJson> _logger;
        public ParseJson(ILogger<ParseJson> logger)
        {
            _logger = logger;
        }

        private void parseJson(FileInfo file)
        {
            string jsonString = File.ReadAllText(file.FullName);
            JObject jsonObject = JObject.Parse(jsonString);
            string prop = (string)jsonObject.SelectToken("$.features[0].properties.EJER");
            _logger.LogInformation(prop);
        }
    }
}
