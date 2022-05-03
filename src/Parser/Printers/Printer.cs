using Microsoft.Extensions.Configuration;
using Printers.GeoJson;
using Shared.ComponentInterfaces;
using Shared.Models.Output;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Printers
{
    public class Printer : IPrinter
    {
        private readonly IConfiguration _configuration;
        private readonly IGeoJsonPrinter _geoJsonPrinter;

        public Printer(IConfiguration configuration, IGeoJsonPrinter geoJsonPrinter)
        {
            _configuration = configuration;
            _geoJsonPrinter = geoJsonPrinter;
        }

        public async Task Print(OutputDataset dataset, int iteration)
        {
            await _geoJsonPrinter.Print(dataset, iteration);
        }
    }
}
