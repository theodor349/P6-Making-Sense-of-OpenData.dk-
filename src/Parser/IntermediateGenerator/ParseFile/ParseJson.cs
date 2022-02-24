using Newtonsoft.Json.Linq;
using Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.ComponentInterfaces;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Shared.Models.ObjectAttributes;

namespace IntermediateGenerator.ParseFile
{
    public class ParseJson : IParseJson
    {
        
        private readonly ILogger<ParseJson> _logger;
        public ParseJson(ILogger<ParseJson> logger)
        {
            _logger = logger;
        }

        public Task<DatasetObject> Parse(string stringFile, string extensionName, string fileName)
        {
            DatasetObject datasetObj = new DatasetObject(extensionName.ToLower(), fileName.ToLower());
            JsonTextReader reader = new JsonTextReader(new StringReader(stringFile));

            IntermediateObject currentParent = new IntermediateObject();
            while (reader.Read())
            {
                if (reader.Value != null)
                {
                    _logger.LogInformation("Token: " + reader.TokenType + " Value: " + reader.Value);
                }
                else
                {
                    if (reader.TokenType.Equals(JsonToken.StartArray) || reader.TokenType.Equals(JsonToken.StartObject))
                    {

                        //currentParent.Attributes.Add();
                    }
                    _logger.LogInformation("Token: " + reader.TokenType);
                }

            }


            //_logger.LogInformation();

            return Task.FromResult(datasetObj);
        }
    }
}
