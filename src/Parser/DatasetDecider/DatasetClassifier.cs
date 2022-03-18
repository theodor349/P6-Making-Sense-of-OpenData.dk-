using Shared.ComponentInterfaces;
using Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatasetDecider
{
    public class DatasetClassifier : IDatasetClassifier
    {
        public async Task<DatasetType> Classify(DatasetObject dataset)
        {
            return await Task.Run(() => { return DatasetType.Parking; });
        }

    }

}
