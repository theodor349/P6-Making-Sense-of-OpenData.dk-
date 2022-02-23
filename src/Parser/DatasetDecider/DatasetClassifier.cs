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
        public Task Classify(DatasetObject dataset)
        {
            return Task.CompletedTask;
        }

    }

}
