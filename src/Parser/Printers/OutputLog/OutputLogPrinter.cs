using Microsoft.Extensions.Configuration;
using Shared.ComponentInterfaces;
using Shared.Models;
using Shared.Models.Output;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Printers.OutputLog
{

    public interface IOutputLogPrinter {
        Task<string> GetOutputLog(OutputLogObject logObject);
        Task Print(string output, int iteration, string fileName);
    }


    internal class OutputLogPrinter : IOutputLogPrinter
    {
        private readonly IConfiguration _configuration;

        public OutputLogPrinter(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<string> GetOutputLog(OutputLogObject logObject)
        {

            List<string> outputLines = GetOutputLines(logObject);
            string output = string.Empty;

            foreach (string line in outputLines)
            {
                output += line;
                output += "\n";
            }

            return await Task.FromResult(output);
        }



        public async Task Print(string output, int iteration, string originalName)
        {
            string outputPath = GetOutputPath(originalName, iteration);
            await File.WriteAllTextAsync(outputPath, output);
        }

        private string GetOutputPath(string originalName, int iteration)
        {
            var splits = originalName.Split('.');
            string fileName = splits.Count() == 1 ? splits.First() : splits.Take(splits.Count() - 1).Aggregate((x, y) => x += y);
            return Path.Combine(_configuration["Output:JsonText"], fileName + "-" + iteration.ToString() + ".txt");
        }

        private List<string> GetOutputLines(OutputLogObject logObject)
        {
            var outputLines = new List<string>();
            outputLines.Add("Input file: " + logObject.FileName);
            outputLines.Add("Classification was: " + (logObject.SuccesfullyClassified ? "successful" : "unsuccessful"));

            if (logObject.SuccesfullyClassified)
            {
                outputLines.Add("Classified as: " + logObject.DatasetClassification.Name + " with the confidence score of: " + logObject.DatasetClassification.Score);

                outputLines.Add("");

                outputLines.Add("List of other classifications and scores (In descending order):");
                foreach (var classification in logObject.OtherClassifications)
                {
                    outputLines.Add(classification.Name + " had a score of " + classification.Score);
                }
            }
            else
            {
                outputLines.Add("Classification set to default: " + logObject.DatasetClassification.Name);
            }

            
            outputLines.Add("");

            outputLines.Add("List of labels found in dataset with occurance and confidence (In descending order):");
            foreach (var classification in logObject.Labels)
            {
                outputLines.Add(classification.Label + " found " + classification.Amount + " times with an average confidence of " + classification.Confidence);
            }

            outputLines.Add("");

            outputLines.Add("Total amount of data objects: " + logObject.TotalDataSetObjects);
            outputLines.Add("Classified data objects: " + logObject.TotalClassifiedObjects);
            outputLines.Add("Custom labeled data objects: " + logObject.CustomLabeledObjects);
            outputLines.Add("Percentage of data that has custom labels: " + logObject.PercentageOfCustomObjects);

            return outputLines;
        }

    }
}
