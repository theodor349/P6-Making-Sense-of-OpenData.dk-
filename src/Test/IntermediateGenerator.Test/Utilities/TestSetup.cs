using IntermediateGenerator.ParseFile;
using Microsoft.Extensions.Logging;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntermediateGenerator.Test.Utilities
{
    internal class TestSetup
    {
        
        public TestSetup()
        {

        }
        public ParseJson GetParseJson()
        {
            ILogger<ParseJson> logger = Substitute.For<ILogger<ParseJson>>();
            return new ParseJson(logger);
        }
    }
}
