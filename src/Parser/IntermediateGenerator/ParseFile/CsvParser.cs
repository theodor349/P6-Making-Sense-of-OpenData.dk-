using Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatasetGenerator.ParseFile
{
    public interface ICsvParser : IFileParser
    {
    }

    internal class CsvParser : ICsvParser
    {
        public Task<DatasetObject> Parse(string stringFile, string extensionName, string fileName)
        {
            throw new NotImplementedException();
        }
    }
}
