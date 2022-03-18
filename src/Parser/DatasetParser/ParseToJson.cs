using System;
using System.Text.Json;
using Shared.Models;
using Microsoft.Extensions.Configuration;

namespace DatasetParser
{
    public class ParseToJson : IParseToJson
    {
        private readonly IConfiguration _configuration;


        public ParseToJson(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ParseIntermediateToJson(DatasetObject dataset)
        {

            // parse data

            var json = JsonSerializer.Serialize(dataset);
            File.WriteAllText(_configuration["Output:JsonText"], json);

        }

    }
}

