using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DatasetParser;
using DatasetParser.Factories;

namespace DatasetParser.Test.Utilities
{
    internal class TestSetup
    {
        public TestSetup()
        {

        }

        internal RouteFactory RouteFactory()
        {
            return new RouteFactory(); 
        }
    }
}
