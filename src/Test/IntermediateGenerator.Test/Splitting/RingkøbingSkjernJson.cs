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
        [TestMethod]
        public void Split()
        {
            var featureItems = new List<ObjectAttribute>();
            featureItems.Add(ModelFactory.GetRingkøbingSkjernParking());
            var features = new ListAttribute("features", featureItems);
            var intermediateObject = ModelFactory.GetIntermediateObject(features);
            var inputDataset = ModelFactory.GetDatasetObject(intermediateObject);

            var parkingSpot = ModelFactory.GetRingkøbingSkjernParking();
            var expectedIntermediate = ModelFactory.GetIntermediateObject(parkingSpot);
            var expectedDataset = ModelFactory.GetDatasetObject(expectedIntermediate);

            var setup = new TestSetup();
            var splitter = setup.IntermediateObjectSplitter();
            var res = splitter.SplitObject(inputDataset);

            res.Should().BeEquivalentTo(expectedDataset);
        }
    }
}
