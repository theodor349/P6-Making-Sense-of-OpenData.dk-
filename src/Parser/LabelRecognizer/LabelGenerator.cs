using LabelRecognizer.Helpers;
using Shared.ComponentInterfaces;
using Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabelRecognizer
{
    public class LabelGenerator : ILabelGenerator
    {
        private readonly ITypeLabeler _typeLabeler;
        private readonly IGeoLabeler _geoLabeler;

        public LabelGenerator(ITypeLabeler typeLabeler, IGeoLabeler geoLabeler)
        {
            _typeLabeler = typeLabeler;
            _geoLabeler = geoLabeler;   

        }

        public async Task AddLabels(DatasetObject dataset)
        {
            await _typeLabeler.AssignTypes(dataset);
            await _geoLabeler.AssignLabels(dataset);
        }
    }
}
