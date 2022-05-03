using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DatasetParser;
using Newtonsoft.Json.Linq;
using NSubstitute;
using Printers;
using Printers.GeoJson;
using Shared.Models.Output;

namespace DatasetParser.Test.Utilities
{
    internal class TestSetup
    {
        protected IFilePrinter filePrinter;

        public TestSetup()
        {
            filePrinter = Substitute.For<IFilePrinter>();
        }

        internal void OnPrintToFile(Action<JObject> action)
        {
            filePrinter.Print(Arg.Any<OutputDataset>(), Arg.Do(action), Arg.Any<int>());
        }

        internal IGeoJsonPrinter GeoJsonPrinter()
        {
            return new GeoJsonPrinter(filePrinter);
        }
    }
}
