using Shared.Models.Output;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.ComponentInterfaces
{
    public interface IPrinter
    {
        Task Print(OutputDataset dataset);
    }
}
