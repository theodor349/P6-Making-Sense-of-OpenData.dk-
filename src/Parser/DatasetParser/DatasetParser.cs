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
        private readonly IParkingspotFactory _parkingspotFactory;

        public DatasetParser(IParkingspotFactory parkingspotFactory)
        {
            _parkingspotFactory = parkingspotFactory;
        }

        public async Task<IntermediateOutput> Parse(DatasetObject dataset, int iteration)
        {
            IntermediateOutput? res = null;
            switch (dataset.DatasetType)
            {
                case DatasetType.Parking:
                case DatasetType.Routes:
                    res = await _parkingspotFactory.BuildDataset(dataset, iteration);
                    break;
            }
            return res;
        }
    }
}
