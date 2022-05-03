using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using Shared.Models.Output;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Printers
{
    public interface IFilePrinter
    {
        Task Print(OutputDataset dataset, JObject content, int iteration);
    }

    internal class FilePrinter : IFilePrinter
    {
        private readonly IConfiguration _configuration;

        public FilePrinter(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task Print(OutputDataset dataset, JObject content, int iteration)
        {
            var splits = dataset.OriginalFileName.Split('.');
            string fileName = splits.Count() == 1 ? splits.First() : splits.Take(splits.Count() - 1).Aggregate((x, y) => x += y);
            string outputPath = Path.Combine(_configuration["Output:JsonText"], fileName + "-" + iteration.ToString() + ".geojson");
            File.Delete(outputPath);
            await File.WriteAllTextAsync(outputPath, content.ToString());
        }
    }
}
