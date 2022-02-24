using Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntermediateGenerator.ParseFile
{
    public interface IParseJson
    {
        Task<DatasetObject> Parse(FileInfo file);
    }
}
