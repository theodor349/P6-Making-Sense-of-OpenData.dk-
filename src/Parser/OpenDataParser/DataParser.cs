using IntermediateGenerator;
using Microsoft.Extensions.Configuration;
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
        private readonly IConfiguration _configuration;
        private readonly ILogger<DataParser> _logger;
        private readonly IIntermediateGenerator _intermediateGenerator;
        private readonly ILabelGenerator _labelGenerator;
        private readonly IDatasetClassifier _datasetClassifier;
        private readonly IDatasetParser _datasetParser;

        public DataParser(IConfiguration configuration, ILogger<DataParser> logger, IIntermediateGenerator intermediateGenerator, ILabelGenerator labelGenerator, IDatasetClassifier datasetClassifier, IDatasetParser datasetParser)
        {
            _configuration = configuration;
            _logger = logger;
            _intermediateGenerator = intermediateGenerator;
            _labelGenerator = labelGenerator;
            _datasetClassifier = datasetClassifier;
            _datasetParser = datasetParser;
        }

        public async Task Run()
        {
            _logger.LogInformation("Hello World");
            _logger.LogInformation(_configuration["HelloWorldString"]);
            var datasetList = Directory.GetFiles(_configuration["Input:FolderPath"]);
            foreach (var dataset in datasetList)
            {
                await ParseDataset(dataset);
            }
        }

        private async Task ParseDataset(string file)
        {
            var dataset = await _intermediateGenerator.GenerateAsync(file);
            await _labelGenerator.AddLabels(dataset);
            await _datasetParser.Parse(dataset, await _datasetClassifier.Classify(dataset));
        }
    }
    
}
