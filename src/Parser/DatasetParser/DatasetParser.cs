﻿using Shared.ComponentInterfaces;
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

        public Task<string> Parse(DatasetObject dataset, DatasetType datasetType, int iteration)
        {
            string res = "";
            switch (datasetType)
            {
                case DatasetType.Parking:
                    res = _parseToJson.ParseIntermediateToJson(dataset, iteration);
                    break;
            }
            return Task.FromResult(res);
        }
    }
}
