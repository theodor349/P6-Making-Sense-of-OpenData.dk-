using LabelRecognizer.Tests.Utilities;
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
        [DataRow(1)]
        [DataRow(2)]
        [TestMethod]
        public void LabelGenerator_AllTypes_All(int num)
        {
            var res = new Dictionary<ObjectLabel, long>();
            var ios = new List<IntermediateObject>();
            foreach (var t in Enum.GetValues(typeof(ObjectLabel)))
            {
                ios.AddRange(ModelFactory.GetIntermediateObjectList(num, () => ModelFactory.GetObjectAttr((ObjectLabel)t)));
                res.Add((ObjectLabel)t, 0);
            }
            var expected = num * res.Count();
            var inputDataset = ModelFactory.GetDatasetObject(ios);

            var setup = new TestSetup();
            var labelGenerator = setup.LabelGenerator();
            labelGenerator.AddLabels(inputDataset).Wait();

            foreach (var intermediateObj in inputDataset.Objects)
                foreach (var objectAttr in intermediateObj.Attributes)
                    foreach (var label in objectAttr.Labels)
                        res[label.Label]++;

            foreach (var t in res)
                t.Value.Should().Be(expected);
        }

        // IO
        //  - List Attr     - Nesting: 1
        //    - List Attr   - Nesting: 2
        //      - Attr      - Nesting: 3
        [DataRow(2, 2, ObjectLabel.Long)]
        [DataRow(4, 3, ObjectLabel.Long)]
        [DataRow(5, 10, ObjectLabel.Long)]
        [TestMethod]
        public void LabelGenerator_Nested_Labels(int ioCount, int nestings, ObjectLabel buttomAttr)
        {
            var ios = new List<IntermediateObject>();
            for (int i = 0; i < ioCount; i++)
                ios.Add(ModelFactory.CreateNested(nestings, buttomAttr));
            var inputDataset = ModelFactory.GetDatasetObject(ios);

            var setup = new TestSetup();
            var labelGenerator = setup.LabelGenerator();
            labelGenerator.AddLabels(inputDataset).Wait();

            foreach (var intermediateObj in inputDataset.Objects)
            {
                foreach (var objectAttr in intermediateObj.Attributes)
                {
                    TestNesting(objectAttr, nestings, buttomAttr);
                }
            }
        }

        private void TestNesting(ObjectAttribute objectAttr, int nestings, ObjectLabel buttomAttr)
        {
            if(nestings == 1)
                objectAttr.Labels.First().Label.Should().Be(buttomAttr);
            else
            {
                objectAttr.Labels.First().Label.Should().Be(ObjectLabel.List);
                TestNesting(((List<ObjectAttribute>)objectAttr.Value).First(), --nestings, buttomAttr);
            }
        }

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
