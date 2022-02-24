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

namespace IntermediateGenerator.ParseJson
{
    public class ParseJson
    {
        public Task<DatasetObject> GenerateAsync()
        {
            ParseFile(new FileInfo("C:\\Users\\Emil-\\Desktop\\Dataset parking\\34.20.12_Parkeringsarealer.geojson"));
            return Task.FromResult(new DatasetObject());
        }
        private readonly ILogger<ParseJson> _logger;
        public ParseJson(ILogger<ParseJson> logger)
        {
            _logger = logger;
        }


        public void ParseFile(FileInfo file)
        {
            {
                if (file.Extension == ".geojson")
                {
                    Parse(file);
                }
                else
                {
                    _logger.LogError("Wrong file format " + file.Extension);
                }
            }
        }
        public DatasetObject Parse(FileInfo file)
        {
            string stringFile = File.ReadAllText(file.FullName);
            JsonTextReader reader = new JsonTextReader(new StringReader(stringFile));
            while (reader.Read())
            {
                if (reader.Value != null)
                {
                    

                }
            }

        
            //_logger.LogInformation();

        return null;
        }
    }
}
