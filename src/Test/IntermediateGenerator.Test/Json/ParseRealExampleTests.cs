using FluentAssertions;
using IntermediateGenerator.Test.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shared.Models;
using Shared.Models.ObjectAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntermediateGenerator.Test.Json
{
    [TestClass]
    public class ParseRealExampleTests
    {
        [TestMethod]
        public void Test()
        {
            string fileName = "fileName";
            string fileExtension = ".geojson";
            var setup = new TestSetup();

            var inputString = "{\"type\": \"FeatureCollection\",\"name\": \"34.20.12_Parkeringsarealer\",\"crs\": {\"type\": \"name\",\"properties\": {\"name\": \"urn:ogc:def:crs:OGC:1.3:CRS84\"}}}";
            var expected = new DatasetObject(fileExtension.ToLower(), fileName.ToLower(), new List<IntermediateObject>()
            {
                new IntermediateObject(new List<ObjectAttribute>()
                {
                    new TextAttribute("type", "FeatureCollection"),
                    new TextAttribute("name", "34.20.12_Parkeringsarealer"),
                    new ListAttribute("crs", new List<ObjectAttribute>()
                    {
                        new TextAttribute("type", "name"),
                        new ListAttribute("properties", new List<ObjectAttribute>()
                        {
                            new TextAttribute("name", "urn:ogc:def:crs:OGC:1.3:CRS84"),
                        }),
                    })
                })
            });

            var parser = setup.GetParseJson();
            var task = parser.Parse(inputString, fileExtension, fileName);
            task.Wait();
            var res = task.Result;

            res.Should().BeEquivalentTo(expected);
        }
    }
}
