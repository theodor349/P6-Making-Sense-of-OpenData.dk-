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
        public Task AddLabels(DatasetObject dataset)
        {
            return Task.CompletedTask;
        }

    }
}
