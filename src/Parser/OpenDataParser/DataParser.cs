using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenDataParser
{
    internal interface IDataParser
    {
        Task Run();
    }

    internal class DataParser : IDataParser
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<DataParser> _logger;

        public DataParser(IConfiguration configuration, ILogger<DataParser> logger) 
        {
            _configuration = configuration;
            _logger = logger;
        }

        public async Task Run()
        {
            _logger.LogInformation("Hello World");
            _logger.LogInformation(_configuration["HelloWorldString"]);
        }
    }
    
}
