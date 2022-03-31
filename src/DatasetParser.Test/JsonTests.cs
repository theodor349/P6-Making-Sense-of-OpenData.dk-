using FluentAssertions;
using DatasetParser.Test.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using Shared.Models;
using Shared.Models.ObjectAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatasetParser.Test
{
    [TestClass]
    public class JsonTests
    {
        [TestMethod]
        public void Test_ParseToJson_Correct()
        {
            var expected = new JObject(
                );
        }
    }
}
