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

namespace LabelRecognizer.Tests.Labeling
{
    [TestClass]
    public class LabelProbabilityTests
    {
        [TestMethod]
        public void LabelGenerator_DifferentTypes_LabelAndProbability()
        {
            var ios = new List<IntermediateObject>();
            // Left IO
            var lList = ModelFactory.GetListAttribute("n1", ModelFactory.GetObjectAttr("n1", ObjectLabel.Double));
            var lAttr = ModelFactory.GetObjectAttr("n2", ObjectLabel.Date);
            ios.Add(ModelFactory.GetIntermediateObject(lList, lAttr));
            // Right IO
            var rAttr = ModelFactory.GetObjectAttr("n1", ObjectLabel.Text);
            var rList = ModelFactory.GetListAttribute("n2", ModelFactory.GetObjectAttr("n1", ObjectLabel.Long));
            ios.Add(ModelFactory.GetIntermediateObject(rAttr, rList));

            var inputDataset = ModelFactory.GetDatasetObject(ios);
            var setup = new TestSetup();
            var labelGenerator = setup.LabelGenerator();
            labelGenerator.AddLabels(inputDataset).Wait();

            // Left IO
            var lIO = inputDataset.Objects[0].Attributes;
            lIO[0].Labels.First(x => x.Label == ObjectLabel.List).Probability.Should().Be(0.5f);
            lIO[0].Labels.First(x => x.Label == ObjectLabel.Text).Probability.Should().Be(0.5f);
            ((List<ObjectAttribute>)lIO[0].Value).First().Labels.First(x => x.Label == ObjectLabel.Double).Probability.Should().Be(1f);
            lIO[1].Labels.First(x => x.Label == ObjectLabel.Date).Probability.Should().Be(0.5f);
            lIO[1].Labels.First(x => x.Label == ObjectLabel.List).Probability.Should().Be(0.5f);
            // Right IO
            var rIO = inputDataset.Objects[1].Attributes;
            rIO[0].Labels.First(x => x.Label == ObjectLabel.List).Probability.Should().Be(0.5f);
            rIO[0].Labels.First(x => x.Label == ObjectLabel.Text).Probability.Should().Be(0.5f);
            ((List<ObjectAttribute>)rIO[1].Value).First().Labels.First(x => x.Label == ObjectLabel.Long).Probability.Should().Be(1f);
            rIO[1].Labels.First(x => x.Label == ObjectLabel.Date).Probability.Should().Be(0.5f);
            rIO[1].Labels.First(x => x.Label == ObjectLabel.List).Probability.Should().Be(0.5f);
        }

        [DataRow(10, 11)]
        [TestMethod]
        public void LabelGenerator_LongDouble_Double100(int numLongs, int numDoubles)
        {
            float expected = 1;
            var ios = new List<IntermediateObject>();
            ios.AddRange(ModelFactory.GetIntermediateObjectList(numLongs, () => ModelFactory.GetObjectAttr(ObjectLabel.Long)));
            ios.AddRange(ModelFactory.GetIntermediateObjectList(numDoubles, () => ModelFactory.GetObjectAttr(ObjectLabel.Double)));
            var inputDataset = ModelFactory.GetDatasetObject(ios);

            var setup = new TestSetup();
            var labelGenerator = setup.LabelGenerator();
            var res = labelGenerator.AddLabels(inputDataset);
            res.Wait();

            inputDataset.Objects.Count.Should().BeGreaterThan(0);
            foreach (var intermediateObj in inputDataset.Objects)
            {
                intermediateObj.Attributes.Count.Should().BeGreaterThan(0);
                foreach (var objectAttr in intermediateObj.Attributes)
                {
                    var probability = objectAttr.Labels.First(x => x.Label == ObjectLabel.Double).Probability;
                    probability.Should().Be(expected);
                }
            }
        }

        [DataRow(10, 11, 12)]
        [TestMethod]
        public void LabelGenerator_TextLongDouble_EqualProbability(int numText, int numLongs, int numDoubles)
        {
            int numTotal = numText + numLongs + numDoubles;
            float expectedText = (float)numText / numTotal;
            float expectedLong = (float)numLongs / numTotal;
            float expectedDouble = (float)numDoubles / numTotal;

            var ios = new List<IntermediateObject>();
            ios.AddRange(ModelFactory.GetIntermediateObjectList(numText, () => ModelFactory.GetObjectAttr(ObjectLabel.Text)));
            ios.AddRange(ModelFactory.GetIntermediateObjectList(numLongs, () => ModelFactory.GetObjectAttr(ObjectLabel.Long)));
            ios.AddRange(ModelFactory.GetIntermediateObjectList(numDoubles, () => ModelFactory.GetObjectAttr(ObjectLabel.Double)));
            var inputDataset = ModelFactory.GetDatasetObject(ios);

            var setup = new TestSetup();
            var labelGenerator = setup.LabelGenerator();
            var res = labelGenerator.AddLabels(inputDataset);
            res.Wait();

            inputDataset.Objects.Count.Should().BeGreaterThan(0);
            foreach (var intermediateObj in inputDataset.Objects)
            {
                intermediateObj.Attributes.Count.Should().BeGreaterThan(0);
                foreach (var objectAttr in intermediateObj.Attributes)
                {
                    LabelModel? label = null;
                    label = objectAttr.Labels.First(x => x.Label == ObjectLabel.Text);
                    label.Probability.Should().Be(expectedText);

                    label = objectAttr.Labels.First(x => x.Label == ObjectLabel.Long);
                    label.Probability.Should().Be(expectedLong);

                    label = objectAttr.Labels.First(x => x.Label == ObjectLabel.Double);
                    label.Probability.Should().Be(expectedDouble);
                }
            }
        }
    }
}
