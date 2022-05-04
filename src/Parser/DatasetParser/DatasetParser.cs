using DatasetParser.Factories;
using Newtonsoft.Json.Linq;
using Shared.ComponentInterfaces;
using Shared.Models;
using Shared.Models.Output;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatasetParser
{
    public class DatasetParser : IDatasetParser
    {
        private readonly IRouteFactory _parkingspotFactory;

        public DatasetParser(IRouteFactory parkingspotFactory)
        {
            _parkingspotFactory = parkingspotFactory;
        }

        public async Task<OutputDataset> Parse(DatasetObject dataset, int iteration)
        {
            OutputDataset res = new OutputDataset(dataset.originalName, dataset.originalExtensionName);
            switch (dataset.DatasetType)
            {
                case DatasetType.Parking:
                case DatasetType.Routes:
                    res.Objects = await _parkingspotFactory.BuildDataset(dataset, iteration);
                    break;
            }
            return res;
        }
    }
}
