using PostProcessing.Helpers;
using Shared.ComponentInterfaces;
using Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PostProcessing
{
    internal class PostProcessor : IPostProcessor
    {
        private readonly IGeoMetaData _geoMetaData;

        public PostProcessor(IGeoMetaData geoMetaData)
        {
            _geoMetaData = geoMetaData;
        }

        public async Task Process(DatasetObject dataset)
        {
            await _geoMetaData.AssignGeoMetaData(dataset);
        }
    }
}
