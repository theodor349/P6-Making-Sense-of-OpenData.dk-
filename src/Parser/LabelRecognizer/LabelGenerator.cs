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

        public Task AddLabels(DatasetObject dataset)
        {
            return Task.CompletedTask;
            //await _typeLabeler.AssignTypes(dataset);
        }
    }
}
