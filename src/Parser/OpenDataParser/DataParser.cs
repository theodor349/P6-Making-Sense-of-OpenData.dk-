using IntermediateGenerator;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using Shared.ComponentInterfaces;
using Shared.Models;
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
            var datasetList = Directory.GetFiles(_configuration["Input:FolderPath"]);
            int iteration = 0;

            var tasks = new List<Task>();
            foreach (var dataset in datasetList)
            {
                iteration++;
                var task = ParseDataset(dataset, iteration);
                if (bool.Parse(_configuration["RuntimeConfig:ParseFilesInSync"]))
                    await task;
                tasks.Add(task);
            }
            Task.WaitAll(tasks.ToArray());
        }

        private async Task ParseDataset(string file, int iteration)
        {
            var intermediateGenerator = _serviceProvider.GetService<IIntermediateGenerator>();
            var datasetParser = _serviceProvider.GetService<IDatasetParser>();

            var dataset = await intermediateGenerator.GenerateAsync(file);
            if(dataset != null)
            {
                await AddLabels(dataset);
                var datasetType = await GetClassification(dataset);
                var output = await datasetParser.Parse(dataset, datasetType, iteration);
                PrintToFile(iteration, output, dataset);
            }
        }

        private void PrintToFile(int iteration, JObject output, DatasetObject dataset)
        {
            string outputPath = _configuration["Output:JsonText"] + dataset.originalName  + "-" + iteration.ToString() + ".geojson";
            File.Delete(outputPath);
            File.WriteAllText(outputPath, output.ToString());
        }

        private async Task<DatasetType> GetClassification(DatasetObject dataset)
        {
            var datasetClassifier = _serviceProvider.GetService<IDatasetClassifier>();
            return await datasetClassifier.Classify(dataset);
        }

        private async Task AddLabels(DatasetObject dataset)
        {
            var labelGenerator = _serviceProvider.GetService<ILabelGenerator>();
            await labelGenerator.AddLabels(dataset);
        }
    }
}
