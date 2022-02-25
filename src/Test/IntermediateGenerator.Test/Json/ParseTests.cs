using FluentAssertions;
using IntermediateGenerator.Test.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
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
    public class ParseTests
    {
        [TestMethod]
        public void Parse_NonNested_CorrectOutput()
        {
            string fileName = "fileName";
            string fileExtension = ".geojson";
            var jsonObj = new
            {
                attr1 = "One",
                attr2 = "Two",
                attr3 = "Three",
            };
            string inputString = JsonConvert.SerializeObject(jsonObj);
            var setup = new TestSetup();

            var objects = new List<IntermediateObject>();
            objects.Add(new IntermediateObject(new List<ObjectAttribute>()
            {
                new TextAttribute("attr1", "One"),
                new TextAttribute("attr2", "Two"),
                new TextAttribute("attr3", "Three"),
            }));
            var expected = new DatasetObject(fileExtension.ToLower(), fileName.ToLower(), objects);

            var parser = setup.GetParseJson();
            var task = parser.Parse(inputString, fileExtension, fileName);
            task.Wait();
            var res = task.Result;

            res.Should().BeEquivalentTo(expected);
        }
        [TestMethod]
        public void Parse_OneNested_CorrectOutput()
        {
            string fileName = "fileName";
            string fileExtension = ".geojson";
            var nestedObj = new
            {
                attr1 = "One",
                attr2 = "Two",
                attr3 = "Three",
            };
            var jsonObj = new
            {
                nestedObj = nestedObj,
            };
            string inputString = JsonConvert.SerializeObject(jsonObj);
            var setup = new TestSetup();

            var objects = new List<IntermediateObject>();
            objects.Add(new IntermediateObject(new List<ObjectAttribute>()
            {
                new ListAttribute("nestedObj", new List<ObjectAttribute>()
                {
                    new TextAttribute("attr1", "One"),
                    new TextAttribute("attr2", "Two"),
                    new TextAttribute("attr3", "Three"),
                }),
            }));
            var expected = new DatasetObject(fileExtension.ToLower(), fileName.ToLower(), objects);

            var parser = setup.GetParseJson();
            var task = parser.Parse(inputString, fileExtension, fileName);
            task.Wait();
            var res = task.Result;

            res.Should().BeEquivalentTo(expected);
        }

    }
}
