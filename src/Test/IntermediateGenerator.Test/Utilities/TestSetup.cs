using DatasetGenerator.ParseFile;
using Microsoft.Extensions.Logging;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatasetGenerator.Test.Utilities
{
    internal class TestSetup
    {
        
        public TestSetup()
        {

        }
        public JsonParser GetParseJson()
        {
            ILogger<JsonParser> logger = Substitute.For<ILogger<JsonParser>>();
            return new JsonParser(logger);
        }
        public DatasetObjectSplitter IntermediateObjectSplitter()
        {
            return new DatasetObjectSplitter();
        }
    }
}
