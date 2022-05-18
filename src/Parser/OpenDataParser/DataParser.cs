using DatasetGenerator;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using Printers.OutputLog;
using Shared.ComponentInterfaces;
using Shared.Models;
using Shared.Models.Output;
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
            _logger.LogInformation("Iteration: {i}, File: {file}", new object[] { iteration, new FileInfo(file).Name });

            var datasetGenerator = _serviceProvider.GetService<IDatasetGenerator>();
            var datasetParser = _serviceProvider.GetService<IDatasetParser>();

            var dataset = await datasetGenerator.GenerateAsync(file);
            _logger.LogInformation("Dataset generated");
            if(dataset != null)
            {
                await Preprocessing(dataset);
                _logger.LogInformation("Pre processing");

                await AddLabels(dataset);
                _logger.LogInformation("Labels added");

                await PostProcess(dataset);
                _logger.LogInformation("Post processing done");

                foreach (var p in dataset.Properties)
                {
                    _logger.LogWarning("{0}: {1}", p.Key, p.Value);
                }

                var outputLog = await GetClassification(dataset, iteration);
                _logger.LogInformation("Dataset classified");

                var output = await datasetParser.Parse(dataset, iteration);
                _logger.LogInformation("Output generated");

                await PrintToFile(iteration, output);
                _logger.LogInformation("Output printed to file");
            }
        }

        private Task Preprocessing(DatasetObject dataset)
        { 
            // TODO: Preprocessing
            return Task.CompletedTask;
        }

        private async Task PostProcess(DatasetObject dataset)
        {
            var postProcessor = _serviceProvider.GetService<IPostProcessor>();
            await postProcessor.Process(dataset);
        }

        private async Task PrintToFile(int iteration, OutputDataset dataset)
        {
            var printer = _serviceProvider.GetService<IPrinter>();
            await printer.Print(dataset, iteration);
        }

        private async Task<OutputLogObject> GetClassification(DatasetObject dataset, int iteration)
        {
            var datasetClassifier = _serviceProvider.GetService<IDatasetClassifier>();
            return await datasetClassifier.Classify(dataset, iteration);
        }

        private async Task AddLabels(DatasetObject dataset)
        {
            var labelGenerator = _serviceProvider.GetService<ILabelGenerator>();
            await labelGenerator.AddLabels(dataset);
        }
    }
}
