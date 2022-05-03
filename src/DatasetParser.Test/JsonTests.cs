//using FluentAssertions;
//using DatasetParser.Test.Utilities;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using Newtonsoft.Json.Linq;
//using Shared.Models;
//using Shared.Models.ObjectAttributes;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace DatasetParser.Test
//{
//    [TestClass]
//    public class JsonTests
//    {
//        [TestMethod]
//        public void Test_ParseToGeojsonPolygon_Correct()
//        {
//            var expected = new JObject(
//                 new JProperty("type", "FeatureCollection"),
//                 new JProperty("features", 
//                    new JArray(
//                        new JObject(
//                            new JProperty("type", "Feature"),
//                            new JProperty("geometry", 
//                                new JObject(
//                                    new JProperty("type", "Polygon"),
//                                    ModelFactory.ReturnPolygonProperty())),
//                            new JProperty("properties", new JObject())))));

//            List<ObjectAttribute> coordsList = new List<ObjectAttribute>()
//            {
//                new ListAttribute("", new List<ObjectAttribute>()
//                {
//                    new DoubleAttribute("lat", 1.1),
//                    new DoubleAttribute("long", 1.2)
//                }),
//                new ListAttribute("", new List<ObjectAttribute>()
//                {
//                    new DoubleAttribute("lat", 1.3),
//                    new DoubleAttribute("long", 1.4)
//                }),
//                new ListAttribute("", new List<ObjectAttribute>()
//                {
//                    new DoubleAttribute("lat", 1.5),
//                    new DoubleAttribute("long", 1.6)
//                }),
//                new ListAttribute("", new List<ObjectAttribute>()
//                {
//                    new DoubleAttribute("lat", 1.1),
//                    new DoubleAttribute("long", 1.2)
//                })
//            };
//            var polygon = new ListAttribute("");
//            ((List<ObjectAttribute>)polygon.Value).AddRange(coordsList);
//            polygon.Labels.Add(new LabelModel(ObjectLabel.Polygon));

//            List<ObjectAttribute> attrs = new List<ObjectAttribute>() { polygon };

//            var ios = new List<IntermediateObject>() { new IntermediateObject(attrs) };
//            var inputDataset = ModelFactory.GetDatasetObject(ios);
//            inputDataset.DatasetType = DatasetType.Parking;
//            ModelFactory.AddCrs(inputDataset);

//            var setup = new TestSetup();
//            var ParseToJson = setup.DatasetParser("json");
//            var res = ParseToJson.Parse(inputDataset, 1).Result;

//            res.ToString().Should().Be(expected.ToString());
//        }

//        [TestMethod]
//        public void Test_ParseToGeojsonLine_Correct()
//        {
//            var expected = new JObject(
//                 new JProperty("type", "FeatureCollection"),
//                 new JProperty("features",
//                    new JArray(
//                        new JObject(
//                            new JProperty("type", "Feature"),
//                            new JProperty("geometry",
//                                new JObject(
//                                    new JProperty("type", ObjectLabel.LineString.ToString()),
//                                    ModelFactory.ReturnLineProperty())),
//                            new JProperty("properties", new JObject())))));

//            List<ObjectAttribute> coordsList = new List<ObjectAttribute>()
//            {
//                new ListAttribute("", new List<ObjectAttribute>()
//                {
//                    new DoubleAttribute("lat", 1.1),
//                    new DoubleAttribute("long", 1.2)
//                }),
//                new ListAttribute("", new List<ObjectAttribute>()
//                {
//                    new DoubleAttribute("lat", 1.3),
//                    new DoubleAttribute("long", 1.4)
//                }),
//                new ListAttribute("", new List<ObjectAttribute>()
//                {
//                    new DoubleAttribute("lat", 1.5),
//                    new DoubleAttribute("long", 1.6)
//                }),
//            };
//            var line = new ListAttribute("");
//            ((List<ObjectAttribute>)line.Value).AddRange(coordsList);
//            line.Labels.Add(new LabelModel(ObjectLabel.LineString));

//            List<ObjectAttribute> attrs = new List<ObjectAttribute>() { line };

//            var ios = new List<IntermediateObject>() { new IntermediateObject(attrs) };
//            var inputDataset = ModelFactory.GetDatasetObject(ios);
//            inputDataset.DatasetType = DatasetType.Parking;
//            ModelFactory.AddCrs(inputDataset);

//            var setup = new TestSetup(); 
//            var ParseToJson = setup.DatasetParser("json");
//            var res = ParseToJson.Parse(inputDataset, 1).Result;

//            res.ToString().Should().Be(expected.ToString());
//        }

//        [TestMethod]
//        public void Test_ParseToGeojsonPoint_Correct()
//        {
//            var expected = new JObject(
//                 new JProperty("type", "FeatureCollection"),
//                 new JProperty("features",
//                    new JArray(
//                        new JObject(
//                            new JProperty("type", "Feature"),
//                            new JProperty("geometry",
//                                new JObject(
//                                    new JProperty("type", ObjectLabel.Point.ToString()),
//                                    ModelFactory.ReturnPointProperty())),
//                            new JProperty("properties", new JObject())))));

//            List<ObjectAttribute> coordsList = new List<ObjectAttribute>()
//            {
//                    new DoubleAttribute("lat", 1.1),
//                    new DoubleAttribute("long", 1.2)
//            };
//            var point = new ListAttribute("");
//            ((List<ObjectAttribute>)point.Value).AddRange(coordsList);
//            point.Labels.Add(new LabelModel(ObjectLabel.Point));

//            List<ObjectAttribute> attrs = new List<ObjectAttribute>() { point };

//            var ios = new List<IntermediateObject>() { new IntermediateObject(attrs) };
//            var inputDataset = ModelFactory.GetDatasetObject(ios);
//            inputDataset.DatasetType = DatasetType.Parking;
//            ModelFactory.AddCrs(inputDataset);

//            var setup = new TestSetup();
//            var ParseToJson = setup.DatasetParser("json");
//            var res = ParseToJson.Parse(inputDataset, 1).Result;

//            res.ToString().Should().Be(expected.ToString());
//        }

//        [TestMethod]
//        public void Test_ParseToGeojsonMultiPoint_Correct()
//        {
//            var expected = new JObject(
//                 new JProperty("type", "FeatureCollection"),
//                 new JProperty("features",
//                    new JArray(
//                        new JObject(
//                            new JProperty("type", "Feature"),
//                            new JProperty("geometry",
//                                new JObject(
//                                    new JProperty("type", ObjectLabel.MultiPoint.ToString()),
//                                    ModelFactory.ReturnLineProperty())),
//                            new JProperty("properties", new JObject())))));

//            List<ObjectAttribute> coordsList = new List<ObjectAttribute>()
//            {
//                new ListAttribute("", new List<ObjectAttribute>()
//                {
//                    new DoubleAttribute("lat", 1.1),
//                    new DoubleAttribute("long", 1.2)
//                }),
//                new ListAttribute("", new List<ObjectAttribute>()
//                {
//                    new DoubleAttribute("lat", 1.3),
//                    new DoubleAttribute("long", 1.4)
//                }),
//                new ListAttribute("", new List<ObjectAttribute>()
//                {
//                    new DoubleAttribute("lat", 1.5),
//                    new DoubleAttribute("long", 1.6)
//                }),
//            };
//            var multiPoint = new ListAttribute("");
//            ((List<ObjectAttribute>)multiPoint.Value).AddRange(coordsList);
//            multiPoint.Labels.Add(new LabelModel(ObjectLabel.MultiPoint));

//            List<ObjectAttribute> attrs = new List<ObjectAttribute>() { multiPoint };

//            var ios = new List<IntermediateObject>() { new IntermediateObject(attrs) };
//            var inputDataset = ModelFactory.GetDatasetObject(ios);
//            inputDataset.DatasetType = DatasetType.Parking;
//            ModelFactory.AddCrs(inputDataset);

//            var setup = new TestSetup();
//            var ParseToJson = setup.DatasetParser("json");
//            var res = ParseToJson.Parse(inputDataset, 1).Result;

//            res.ToString().Should().Be(expected.ToString());
//        }
//    }
//}
