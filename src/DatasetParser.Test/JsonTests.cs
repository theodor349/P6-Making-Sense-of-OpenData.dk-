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
        public void Test_ParseToGeojsonPolygon_Correct()
        {
            var expected = new JObject(
                 new JProperty("type", "FeatureCollection"),
                 new JProperty("features", 
                    new JArray(
                        new JObject(
                            new JProperty("type", "Feature"),
                            new JProperty("geometry", 
                                new JObject(
                                    new JProperty("type", "Polygon"),
                                    new JProperty("coordinates", new JArray(
                                        new JArray(
                                            new JArray(1.1, 1.2),
                                            new JArray(1.3, 1.4),
                                            new JArray(1.5, 1.6),
                                            new JArray(1.1, 1.2)
                                            ))))),
                            new JProperty("properties", new JObject())))));


        }
    }
}
