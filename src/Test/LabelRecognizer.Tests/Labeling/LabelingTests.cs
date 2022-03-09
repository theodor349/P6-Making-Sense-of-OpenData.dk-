﻿using LabelRecognizer.Tests.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shared.Models.ObjectAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Shared.Models;

namespace LabelRecognizer.Tests.Labeling
{
    [TestClass]
    public class LabelingTests
    {
        [DataRow(10, 11)]
        [TestMethod]
        public void LabelGenerator_LongDouble_LabelDoulbeOnly(int numLongs, int numDoubles)
        {
            var ios = new List<IntermediateObject>();
            ios.AddRange(ModelFactory.GetIntermediateObjectList(numLongs, () => ModelFactory.GetObjectAttr(ObjectLabel.Long)));
            ios.AddRange(ModelFactory.GetIntermediateObjectList(numDoubles, () => ModelFactory.GetObjectAttr(ObjectLabel.Double)));
            var inputDataset = ModelFactory.GetDatasetObject(ios);

            var setup = new TestSetup();
            var labelGenerator = setup.LabelGenerator();
            var res = labelGenerator.AddLabels(inputDataset);
            res.Wait();

            int longCount = 0;
            int doubleCount = 0;

            foreach (var intermediateObj in inputDataset.Objects)
            {
                foreach (var objectAttr in intermediateObj.Attributes)
                {
                    if (objectAttr.Labels.Count(x => x.Label == ObjectLabel.Long) == 1)
                    {
                        longCount++;
                    }
                    if (objectAttr.Labels.Count(x => x.Label == ObjectLabel.Double) == 1)
                    {
                        doubleCount++;
                    }
                }
            }

            longCount.Should().Be(0);
            doubleCount.Should().Be(numLongs + numDoubles);
        }

        [DataRow(10, 10, 10)]
        [TestMethod]
        public void LabelGenerator_TextLongDouble_AllLabels(int numText, int numLongs, int numDoubles)
        {
            var ios = new List<IntermediateObject>();
            ios.AddRange(ModelFactory.GetIntermediateObjectList(numText, () => ModelFactory.GetObjectAttr(ObjectLabel.Text)));
            ios.AddRange(ModelFactory.GetIntermediateObjectList(numLongs, () => ModelFactory.GetObjectAttr(ObjectLabel.Long)));
            ios.AddRange(ModelFactory.GetIntermediateObjectList(numDoubles, () => ModelFactory.GetObjectAttr(ObjectLabel.Double)));
            var inputDataset = ModelFactory.GetDatasetObject(ios);

            var setup = new TestSetup();
            var labelGenerator = setup.LabelGenerator();
            var res = labelGenerator.AddLabels(inputDataset);
            res.Wait();

            int textCount = 0;
            int longCount = 0;
            int doubleCount = 0;

            foreach (var intermediateObj in inputDataset.Objects)
            {
                foreach (var objectAttr in intermediateObj.Attributes)
                {
                    if (objectAttr.Labels.Count(x => x.Label == ObjectLabel.Text) == 1)
                    {
                        textCount++;
                    }
                    if (objectAttr.Labels.Count(x => x.Label == ObjectLabel.Long) == 1)
                    {
                        longCount++;
                    }
                    if (objectAttr.Labels.Count(x => x.Label == ObjectLabel.Double) == 1)
                    {
                        doubleCount++;
                    }
                }
            }

            textCount.Should().Be(numText + numLongs + numDoubles);
            longCount.Should().Be(numText + numLongs + numDoubles);
            doubleCount.Should().Be(numText + numLongs + numDoubles);
        }
    }
}
