using FluentAssertions;
using LabelRecognizer.Tests.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shared.Models;
using Shared.Models.ObjectAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabelRecognizer.Tests.Labeling.GeoTests
{
    [TestClass]
    public class Polygons
    {
        [DataRow(4)]
        [DataRow(5)]
        [TestMethod]
        public void LabelGenerator_Polygon_Correct(int amountPoints)
        {
            var ios = new List<IntermediateObject>();
            var polygon = ModelFactory.GetPolygonAttr(amountPoints);
            ios.Add(ModelFactory.GetIntermediateObject(polygon));
            var inputDataset = ModelFactory.GetDatasetObject(ios);

            var setup = new TestSetup();
            var labelGenerator = setup.LabelGenerator();
            labelGenerator.AddLabels(inputDataset).Wait();

            // Assert 
            var res = inputDataset.Objects[0].Attributes[0].Labels.FirstOrDefault(x => x.Label == ObjectLabel.Polygon);
            res.Should().NotBeNull();
        }

        [DataRow(2)]
        [DataRow(3)]
        [TestMethod]
        public void LabelGenerator_Polygon_LessThan4CoordsShouldBeNull(int amountPoints)
        {
            var ios = new List<IntermediateObject>();
            var polygon = ModelFactory.GetPolygonAttr(amountPoints);
            ios.Add(ModelFactory.GetIntermediateObject(polygon));
            var inputDataset = ModelFactory.GetDatasetObject(ios);

            var setup = new TestSetup();
            var labelGenerator = setup.LabelGenerator();
            labelGenerator.AddLabels(inputDataset).Wait();

            // Assert 
            var res = inputDataset.Objects[0].Attributes[0].Labels.FirstOrDefault(x => x.Label == ObjectLabel.Polygon);
            res.Should().BeNull();
        }
    }
}
