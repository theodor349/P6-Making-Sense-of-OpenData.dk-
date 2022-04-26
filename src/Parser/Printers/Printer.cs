using Microsoft.Extensions.Configuration;
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

        public Printer(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public Task Print(OutputDataset dataset, int i)
        {
            throw new NotImplementedException();
        }
    }
}
