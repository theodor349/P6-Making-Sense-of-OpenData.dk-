using Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.ComponentInterfaces
{
    public interface IIntermediateGenerator
    {
        Task<DatasetObject> GenerateAsync();
    }
}

