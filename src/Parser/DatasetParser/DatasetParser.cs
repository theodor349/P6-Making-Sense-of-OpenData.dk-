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

        public Task Parse(DatasetObject dataset, DatasetType datasetType)
        {

            switch (datasetType)
            {
                case DatasetType.Parking:
                    _parseToJson.ParseIntermediateToJson(dataset);
                    break;
            }



            return Task.CompletedTask;
        }
    }
}
