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
        public DatasetType Classify(DatasetObject dataset)
        {
            return DatasetType.Parking;
        }

    }

}
