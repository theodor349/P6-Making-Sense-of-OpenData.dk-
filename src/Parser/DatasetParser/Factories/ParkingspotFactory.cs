using Shared.Models;
using Shared.Models.Output;
using Shared.Models.Output.Specializations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatasetParser.Factories
{
    public interface IParkingspotFactory : IDatasetOutputFactory { }

    internal class ParkingspotFactory : IParkingspotFactory
    {
        public Task<List<IntermediateOutput>> BuildDataset(DatasetObject dataset, int iteration)
        {
            return null;
        }
    }
}
