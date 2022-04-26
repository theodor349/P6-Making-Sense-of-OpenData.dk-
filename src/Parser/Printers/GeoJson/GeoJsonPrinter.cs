using Shared.ComponentInterfaces;
using Shared.Models.Output;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Printers.GeoJson
{
    public interface IGeoJsonPrinter : IPrinter { }

    internal class GeoJsonPrinter : IGeoJsonPrinter
    {
        public Task Print(OutputDataset dataset)
        {
            throw new NotImplementedException();
        }
    }
}
