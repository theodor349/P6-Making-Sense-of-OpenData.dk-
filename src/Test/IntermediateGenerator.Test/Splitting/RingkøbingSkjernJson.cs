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
        [DataRow(1)]
        [DataRow(2)]
        [TestMethod]
        public void Split_Features_Fluent2(int amount)
        {
            var featureItems = new List<ObjectAttribute>();
            for (int i = 0; i < amount; i++)
            {
                featureItems.Add(ModelFactory.GetRingkøbingSkjernParking());
            }
            var features = new ListAttribute("features", featureItems);
            var intermediateObject = ModelFactory.GetIntermediateObject(features);
            var inputDataset = ModelFactory.GetDatasetObject(intermediateObject);

            var parkingSpots = new List<ObjectAttribute>();
            for (int i = 0; i < amount; i++)
            {
                parkingSpots.Add(ModelFactory.GetRingkøbingSkjernParking());
            }
            var expectedIntermediate = ModelFactory.GetIntermediateObject(parkingSpots);
            var expectedDataset = ModelFactory.GetDatasetObject(expectedIntermediate);

            var setup = new TestSetup();
            var splitter = setup.IntermediateObjectSplitter();
            var res = splitter.SplitObject(inputDataset);

            res.Should().BeEquivalentTo(expectedDataset);
        }
    }
}
