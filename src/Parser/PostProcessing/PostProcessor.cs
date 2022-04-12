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
        public Task Process(DatasetObject dataset)
        {
            return Task.CompletedTask;
        }
    }
}
