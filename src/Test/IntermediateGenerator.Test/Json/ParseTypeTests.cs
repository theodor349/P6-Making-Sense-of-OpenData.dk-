using FluentAssertions;
using DatasetGenerator.Test.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Shared.Models;
using Shared.Models.ObjectAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatasetGenerator.Test.Json
{
    [TestClass]
    public class ParseTypeTests
    {
        [TestMethod]
        public void Parse_Text_CorrectOutput()
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
        public void Parse_Long_CorrectOutput()
        {
            string fileName = "fileName";
            string fileExtension = ".geojson";
            var jsonObj = new
            {
                attr1 = 1,
                attr2 = 2,
                attr3 = 3,
            };
            string inputString = JsonConvert.SerializeObject(jsonObj);
            var setup = new TestSetup();

            var objects = new List<IntermediateObject>();
            objects.Add(new IntermediateObject(new List<ObjectAttribute>()
            {
                new LongAttribute("attr1", 1),
                new LongAttribute("attr2", 2),
                new LongAttribute("attr3", 3),
            }));
            var expected = new DatasetObject(fileExtension.ToLower(), fileName.ToLower(), objects);

            var parser = setup.GetParseJson();
            var task = parser.Parse(inputString, fileExtension, fileName);
            task.Wait();
            var res = task.Result;

            res.Should().BeEquivalentTo(expected);
        }

        [TestMethod]
        public void Parse_Double_CorrectOutput()
        {
            string fileName = "fileName";
            string fileExtension = ".geojson";
            var jsonObj = new
            {
                attr1 = 1.1,
                attr2 = 2.2,
                attr3 = 3.3,
            };
            string inputString = JsonConvert.SerializeObject(jsonObj);
            var setup = new TestSetup();

            var objects = new List<IntermediateObject>();
            objects.Add(new IntermediateObject(new List<ObjectAttribute>()
            {
                new DoubleAttribute("attr1", 1.1),
                new DoubleAttribute("attr2", 2.2),
                new DoubleAttribute("attr3", 3.3),
            }));
            var expected = new DatasetObject(fileExtension.ToLower(), fileName.ToLower(), objects);

            var parser = setup.GetParseJson();
            var task = parser.Parse(inputString, fileExtension, fileName);
            task.Wait();
            var res = task.Result;

            res.Should().BeEquivalentTo(expected);
        }

        [TestMethod]
        [DataRow("2000-12-24T18:30:10", "2000-12-24 18:30:10:000")]
        [DataRow("2000-12-24T18:30:10.420", "2000-12-24 18:30:10:420")]
        public void Parse_DateTime_CorrectOutput(string dateString, string expectedDateString)
        {
            string fileName = "fileName";
            string fileExtension = ".geojson";
            DateTime expectedDate = DateTime.ParseExact(expectedDateString, "yyyy-MM-dd HH:mm:ss:fff", null);
            var jsonObj = new
            {
                attr1 = dateString,
            };
            string inputString = JsonConvert.SerializeObject(jsonObj);
            var setup = new TestSetup();

            var objects = new List<IntermediateObject>();
            objects.Add(new IntermediateObject(new List<ObjectAttribute>()
            {
                new DateAttribute("attr1", expectedDate),
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
