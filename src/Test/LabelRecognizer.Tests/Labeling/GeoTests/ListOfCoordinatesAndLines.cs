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
    public class ListOfCoordinatesAndLines
    {
        [DataRow(4)]
        [DataRow(5)]
        [TestMethod]
        public void LabelGenerator_ListAndLine_Correct(int amountPoints)
        {
            var ios = new List<IntermediateObject>();
            var polygon = ModelFactory.GetListOfPointsAttr(amountPoints);
            ios.Add(ModelFactory.GetIntermediateObject(polygon));
            var inputDataset = ModelFactory.GetDatasetObject(ios);

            var setup = new TestSetup();
            var labelGenerator = setup.LabelGenerator();
            labelGenerator.AddLabels(inputDataset).Wait();

            // Assert 
            var isLine = inputDataset.Objects[0].Attributes[0].HasLabel(ObjectLabel.LineString);
            var isListOfPoints = inputDataset.Objects[0].Attributes[0].HasLabel(ObjectLabel.MultiPoint);
            isLine.Should().BeTrue();
            isListOfPoints.Should().BeTrue();
        }

        [DataRow(4)]
        [DataRow(5)]
        [TestMethod]
        public void LabelGenerator_Polygon_NotLabeled(int amountPoints)
        {
            var ios = new List<IntermediateObject>();
            var polygon = ModelFactory.GetPolygonAttr(amountPoints);
            ios.Add(ModelFactory.GetIntermediateObject(polygon));
            var inputDataset = ModelFactory.GetDatasetObject(ios);

            var setup = new TestSetup();
            var labelGenerator = setup.LabelGenerator();
            labelGenerator.AddLabels(inputDataset).Wait();

            // Assert 
            var isLine = inputDataset.Objects[0].Attributes[0].HasLabel(ObjectLabel.LineString);
            var isListOfPoints = inputDataset.Objects[0].Attributes[0].HasLabel(ObjectLabel.MultiPoint);
            isLine.Should().BeFalse();
            isListOfPoints.Should().BeFalse();
        }
    }
}
