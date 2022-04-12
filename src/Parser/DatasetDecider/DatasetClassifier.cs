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
        public async Task Classify(DatasetObject dataset)
        {
            await Task.Run(() => {

                if (dataset.originalName == "vesthimmerland_cykel_og_vandre_ruter.geojson")
                {
                    dataset.DatasetType = DatasetType.Routes;
                }
                else
                {
                    dataset.DatasetType = DatasetType.Parking;
                }
            });
        }

    }

}
