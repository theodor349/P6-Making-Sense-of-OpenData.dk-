using IntermediateGenerator;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Shared.ComponentInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace OpenDataParser
{
    internal interface IDataParser
    {
        Task Run();
    }

    internal class DataParser : IDataParser
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IConfiguration _configuration;
        private readonly ILogger<DataParser> _logger;

        public DataParser(IServiceProvider serviceProvider, IConfiguration configuration, ILogger<DataParser> logger)
        {
            _serviceProvider = serviceProvider;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task Run()
        {
            _logger.LogInformation("Hello World");
            _logger.LogInformation(_configuration["HelloWorldString"]);
            var datasetList = Directory.GetFiles(_configuration["Input:FolderPath"]);
            int iteration = 0;
            foreach (var dataset in datasetList)
            {
                iteration++;
                await ParseDataset(dataset, iteration);
            }
        }

        private async Task ParseDataset(string file, int iteration)
        {
            var intermediateGenerator = _serviceProvider.GetService<IIntermediateGenerator>();
            var labelGenerator = _serviceProvider.GetService<ILabelGenerator>();
            var datasetParser = _serviceProvider.GetService<IDatasetParser>();
            var datasetClassifier = _serviceProvider.GetService<IDatasetClassifier>();

            var dataset = await intermediateGenerator.GenerateAsync(file);
            if(dataset != null)
            {
                await labelGenerator.AddLabels(dataset);
                await datasetParser.Parse(dataset, await datasetClassifier.Classify(dataset), iteration);
            }
        }
    }
}
