using Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PostProcessing.Helpers
{
    internal interface IGeoMetaData
    {
        Task AssignGeoMetaData(DatasetObject dataset);
    }

    internal class GeoMetaData : IGeoMetaData
    {
        public Task AssignGeoMetaData(DatasetObject dataset)
        {
            if(!dataset.HasProperty("CoordinateReferenceSystem"))
                AssignCoordinateReferenceSystem(dataset);
            return Task.CompletedTask;
        }

        private void AssignCoordinateReferenceSystem(DatasetObject dataset)
        {
        }
    }
}
