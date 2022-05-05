using Microsoft.Extensions.Configuration;
using Printers.GeoJson;
using Printers.OutputLog;
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
        private readonly IOutputLogPrinter _outputLogPrinter;

        public Printer(IConfiguration configuration, IGeoJsonPrinter geoJsonPrinter, IOutputLogPrinter outputLogPrinter)
        {
            _configuration = configuration;
            _geoJsonPrinter = geoJsonPrinter;
            _outputLogPrinter = outputLogPrinter;
        }

        public async Task Print(OutputDataset dataset, int iteration)
        {
            await _geoJsonPrinter.Print(dataset, iteration);
        }
    }
}
