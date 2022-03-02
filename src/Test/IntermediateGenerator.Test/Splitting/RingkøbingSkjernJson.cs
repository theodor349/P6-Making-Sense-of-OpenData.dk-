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

namespace IntermediateGenerator.Test.Splitting
{
    [TestClass]
    public class RingkøbingSkjernJson
    {
        [DataRow(2)]
        [DataRow(3)]
        [TestMethod]
        public void Split_Features_Fluent2(int amount)
        {
            var featureItems = ModelFactory.GetListOfObjectsAttributes(amount, ModelFactory.GetRingkøbingSkjernParking);
            var features = new ListAttribute("features", featureItems);
            var intermediateObject = ModelFactory.GetIntermediateObject(features);
            var inputDataset = ModelFactory.GetDatasetObject(intermediateObject);

            var parkingSpots = ModelFactory.GetListOfObjectsAttributes(amount, ModelFactory.GetRingkøbingSkjernParking);
            var expectedIntermediate = ModelFactory.GetIntermediateObject(parkingSpots);
            var expectedDataset = ModelFactory.GetDatasetObject(expectedIntermediate);

            var setup = new TestSetup();
            var splitter = setup.IntermediateObjectSplitter();
            var res = splitter.SplitObject(inputDataset);

            res.Should().BeEquivalentTo(expectedDataset);
        }

        [DataRow(2)]
        [DataRow(3)]
        [TestMethod]
        public void Split_RingkøbingHeader_Removed(int amount)
        {
            var attributes = new List<ObjectAttribute>();
            attributes.Add(new TextAttribute("type", "FeatureCollection"));
            attributes.Add(new TextAttribute("name", "34.20.12_Parkeringsarealer"));
            attributes.Add(new ListAttribute("crs", new List<ObjectAttribute>()
            {
                new TextAttribute("type", "mame"),
                new ListAttribute("properties", new List<ObjectAttribute>()
                {
                    new TextAttribute("name", "urn:ogc:def:crs:OGC:1.3:CRS84")
                })
            }));
            var featureItems = ModelFactory.GetListOfObjectsAttributes(amount, ModelFactory.GetRingkøbingSkjernParking);
            var features = new ListAttribute("features", featureItems);
            attributes.Add(features);
            var intermediateObject = ModelFactory.GetIntermediateObject(attributes);
            var inputDataset = ModelFactory.GetDatasetObject(intermediateObject);

            var parkingSpots = ModelFactory.GetListOfObjectsAttributes(amount, ModelFactory.GetRingkøbingSkjernParking);
            var expectedIntermediate = ModelFactory.GetIntermediateObject(parkingSpots);
            var expectedDataset = ModelFactory.GetDatasetObject(expectedIntermediate);

            var setup = new TestSetup();
            var splitter = setup.IntermediateObjectSplitter();
            var res = splitter.SplitObject(inputDataset);

            res.Should().BeEquivalentTo(expectedDataset);
        }

        [DataRow(2)]
        [DataRow(3)]
        [TestMethod]
        public void Split_OdenseHeader_Removed(int amount)
        {
            var attributes = new List<ObjectAttribute>();
            attributes.Add(new TextAttribute("type", "FeatureCollection"));
            var featureItems = ModelFactory.GetListOfObjectsAttributes(amount, ModelFactory.GetRingkøbingSkjernParking);
            var features = new ListAttribute("features", featureItems);
            attributes.Add(features);
            var intermediateObject = ModelFactory.GetIntermediateObject(attributes);
            var inputDataset = ModelFactory.GetDatasetObject(intermediateObject);

            var parkingSpots = ModelFactory.GetListOfObjectsAttributes(amount, ModelFactory.GetRingkøbingSkjernParking);
            var expectedIntermediate = ModelFactory.GetIntermediateObject(parkingSpots);
            var expectedDataset = ModelFactory.GetDatasetObject(expectedIntermediate);

            var setup = new TestSetup();
            var splitter = setup.IntermediateObjectSplitter();
            var res = splitter.SplitObject(inputDataset);

            res.Should().BeEquivalentTo(expectedDataset);
        }
    }
}
