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
        public Task Parse(DatasetObject dataset, DatasetType datasetType)
        {
            return Task.CompletedTask;
        }
    }
}
