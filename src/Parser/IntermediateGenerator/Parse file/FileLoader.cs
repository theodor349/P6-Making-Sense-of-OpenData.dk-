using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntermediateGenerator.Parse_file
{
    internal class FileLoader
    {
        private readonly ILogger<FileLoader> _logger;
        public FileLoader(ILogger<FileLoader> logger)
        {
            _logger = logger;
        }
        public void ParseFile(FileInfo file)
        {
            if (file.Extension == ".geojson")
            {
               // ParseJson(file);
            }
            else
            {
                _logger.LogError("Wrong file format " + file.Extension);
            }
        }
    }
}
