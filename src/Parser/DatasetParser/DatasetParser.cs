using Newtonsoft.Json.Linq;
using Shared.ComponentInterfaces;
using Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatasetParser
{
    public class DatasetParser : IDatasetParser
    {
        private readonly IParseToJson _parseToJson;

        public DatasetParser(IParseToJson parseToJson)
        {
            _parseToJson = parseToJson;
        }

        public Task<JObject> Parse(DatasetObject dataset, int iteration)
        {
            JObject? res = null;
            switch (dataset.DatasetType)
            {
                case DatasetType.Parking:
                case DatasetType.Routes:
                    res = _parseToJson.ParseDatasetObjectToJson(dataset, iteration);
                    break;
            }
            return Task.FromResult(res);
        }
    }
}
