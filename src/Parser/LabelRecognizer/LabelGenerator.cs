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

        public LabelGenerator(ITypeLabeler typeLabeler)
        {
            _typeLabeler = typeLabeler;
        }

        public async Task AddLabels(DatasetObject dataset)
        {
            await _typeLabeler.AssignTypes(dataset);
        }
    }
}
