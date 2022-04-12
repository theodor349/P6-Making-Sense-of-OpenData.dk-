using LabelRecognizer.Helpers;
using Shared.ComponentInterfaces;
using Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace LabelRecognizer
{
    public partial class LabelGenerator : ILabelGenerator
    {
        private readonly ITypeLabeler _typeLabeler;
        private readonly IGeoLabeler _geoLabeler;
        private readonly ILabelNameLookupTable _labelNameLookupTable;

        public LabelGenerator(ITypeLabeler typeLabeler, IGeoLabeler geoLabeler, ILabelNameLookupTable labelNameLookupTable)
        {
            _typeLabeler = typeLabeler;
            _geoLabeler = geoLabeler;
            _labelNameLookupTable = labelNameLookupTable;
        }

        public async Task AddLabels(DatasetObject dataset)
        {
            await _typeLabeler.AssignTypes(dataset);
            await _geoLabeler.AssignLabels(dataset);
            await _labelNameLookupTable.AssignLabels(dataset);
        }
    }
}
