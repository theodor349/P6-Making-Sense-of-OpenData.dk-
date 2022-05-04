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
        [DataRow(2)]
        [DataRow(3)]
        [TestMethod]
        public void LabelGenerator_SameName_CorrectType(int num)
        {
            var expectedLabel = PredefinedLabels.List;
            var ios = new List<IntermediateObject>();
            var attributes = ModelFactory.GetObjectAttrList(num, () => ModelFactory.GetObjectAttr("StartList", expectedLabel));
            ios.Add(ModelFactory.GetIntermediateObject(ModelFactory.GetListAttribute("name", attributes)));
            var inputDataset = ModelFactory.GetDatasetObject(ios);

            var setup = new TestSetup();
            var labelGenerator = setup.LabelGenerator();
            labelGenerator.AddLabels(inputDataset).Wait();

            foreach (var intermediateObj in inputDataset.Objects)
                foreach (var objectAttr in intermediateObj.Attributes)
                    foreach (var label in objectAttr.Labels)
                        label.Label.Should().Be(expectedLabel);
        }

        [DataRow(1)]
        [DataRow(2)]
        [TestMethod]
        public void LabelGenerator_AllTypes_All(int num)
        {
            var res = new Dictionary<string, long>();
            var ios = new List<IntermediateObject>();
            var typeList = new List<string>()
            {
                PredefinedLabels.Long,
                PredefinedLabels.Double,
                PredefinedLabels.List,
                PredefinedLabels.Null,
                PredefinedLabels.Date,
                PredefinedLabels.Text,
            };
            foreach (var t in typeList)
            {
                ios.AddRange(ModelFactory.GetIntermediateObjectList(num, () => ModelFactory.GetObjectAttr(t)));
                res.Add(t, 0);
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
        [DataRow(2, 2, PredefinedLabels.Long)]
        [DataRow(4, 3, PredefinedLabels.Long)]
        [DataRow(5, 10, PredefinedLabels.Long)]
        [TestMethod]
        public void LabelGenerator_Nested_Labels(int ioCount, int nestings, string buttomAttr)
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

        private void TestNesting(ObjectAttribute objectAttr, int nestings, string buttomAttr)
        {
            if(nestings == 1)
                objectAttr.Labels.First().Label.Should().Be(buttomAttr);
            else
            {
                objectAttr.Labels.First().Label.Should().Be(PredefinedLabels.List);
                TestNesting(((List<ObjectAttribute>)objectAttr.Value).First(), --nestings, buttomAttr);
            }
        }

        [DataRow(10, 11)]
        [TestMethod]
        public void LabelGenerator_LongDouble_LabelDoubleOnly(int numLongs, int numDoubles)
        {
            var ios = new List<IntermediateObject>();
            ios.AddRange(ModelFactory.GetIntermediateObjectList(numLongs, () => ModelFactory.GetObjectAttr(PredefinedLabels.Long)));
            ios.AddRange(ModelFactory.GetIntermediateObjectList(numDoubles, () => ModelFactory.GetObjectAttr(PredefinedLabels.Double)));
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
                    if (objectAttr.Labels.Count(x => x.Label == PredefinedLabels.Long) == 1)
                    {
                        longCount++;
                    }
                    if (objectAttr.Labels.Count(x => x.Label == PredefinedLabels.Double) == 1)
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
            ios.AddRange(ModelFactory.GetIntermediateObjectList(numText, () => ModelFactory.GetObjectAttr(PredefinedLabels.Text)));
            ios.AddRange(ModelFactory.GetIntermediateObjectList(numLongs, () => ModelFactory.GetObjectAttr(PredefinedLabels.Long)));
            ios.AddRange(ModelFactory.GetIntermediateObjectList(numDoubles, () => ModelFactory.GetObjectAttr(PredefinedLabels.Double)));
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
                    if (objectAttr.Labels.Count(x => x.Label == PredefinedLabels.Text) == 1)
                    {
                        textCount++;
                    }
                    if (objectAttr.Labels.Count(x => x.Label == PredefinedLabels.Long) == 1)
                    {
                        longCount++;
                    }
                    if (objectAttr.Labels.Count(x => x.Label == PredefinedLabels.Double) == 1)
                    {
                        doubleCount++;
                    }
                }
            }

            textCount.Should().Be(numText + numLongs + numDoubles);
            longCount.Should().Be(numText + numLongs + numDoubles);
            doubleCount.Should().Be(numText + numLongs + numDoubles);
        }
        [DataRow(5, 7)]
        [DataRow(69, 1)]
        [TestMethod]
        public void LabelGenerator_LongNull_LabelLongOnly(int numLongs, int numNulls)
        {
            var ios = new List<IntermediateObject>();
            ios.AddRange(ModelFactory.GetIntermediateObjectList(numLongs, () => ModelFactory.GetObjectAttr(PredefinedLabels.Long)));
            ios.AddRange(ModelFactory.GetIntermediateObjectList(numNulls, () => ModelFactory.GetObjectAttr(PredefinedLabels.Null)));
            var inputDataset = ModelFactory.GetDatasetObject(ios);

            var setup = new TestSetup();
            var labelGenerator = setup.LabelGenerator();
            var res = labelGenerator.AddLabels(inputDataset);
            res.Wait();

            int longCount = 0;
            int nullCount = 0;

            foreach (var intermediateObj in inputDataset.Objects)
            {
                foreach (var objectAttr in intermediateObj.Attributes)
                {
                    if (objectAttr.Labels.Count(x => x.Label == PredefinedLabels.Long) == 1)
                    {
                        longCount++;
                    }
                    if (objectAttr.Labels.Count(x => x.Label == PredefinedLabels.Null) == 1)
                    {
                        nullCount++;
                    }
                }
            }

            nullCount.Should().Be(0);
            longCount.Should().Be(numLongs + numNulls);
        }

        [DataRow(17, 5)]
        [DataRow(1, 10)]
        [TestMethod]
        public void LabelGenerator_LongText_CantParseTextAsLongLabelTextOnly(int numLongs, int numText)
        {
            var ios = new List<IntermediateObject>();
            ios.AddRange(ModelFactory.GetIntermediateObjectList(numLongs, () => ModelFactory.GetObjectAttr(PredefinedLabels.Long)));
            ios.AddRange(ModelFactory.GetIntermediateObjectList(numText, () => ModelFactory.GetObjectAttr(PredefinedLabels.Text)));
            var inputDataset = ModelFactory.GetDatasetObject(ios);

            var setup = new TestSetup();
            var labelGenerator = setup.LabelGenerator();
            var res = labelGenerator.AddLabels(inputDataset);
            res.Wait();

            int longCount = 0;
            int textCount = 0;

            foreach (var intermediateObj in inputDataset.Objects)
            {
                foreach (var objectAttr in intermediateObj.Attributes)
                {
                    if (objectAttr.Labels.Count(x => x.Label == PredefinedLabels.Long) == 1)
                    {
                        longCount++;
                    }
                    if (objectAttr.Labels.Count(x => x.Label == PredefinedLabels.Text) == 1)
                    {
                        textCount++;
                    }
                }
            }

            textCount.Should().Be(numLongs + numText);
            longCount.Should().Be(0); //wat
        }

        [TestMethod]
        public void LabelGenerator_LongText_CanParseTextAsLongLabelLongOnly()
        {
            var ios = new List<IntermediateObject>();
            ios.Add(new IntermediateObject(new List<ObjectAttribute> { new TextAttribute("lars", "17892")}));
            ios.Add(new IntermediateObject(new List<ObjectAttribute> { new LongAttribute("lars", 819103) }));
            var inputDataset = ModelFactory.GetDatasetObject(ios);

            var setup = new TestSetup();
            var labelGenerator = setup.LabelGenerator();
            var res = labelGenerator.AddLabels(inputDataset);
            res.Wait();

            int longCount = 0;
            int textCount = 0;

            foreach (var intermediateObj in inputDataset.Objects)
            {
                foreach (var objectAttr in intermediateObj.Attributes)
                {
                    if (objectAttr.Labels.Count(x => x.Label == PredefinedLabels.Long) == 1)
                    {
                        longCount++;
                    }
                    if (objectAttr.Labels.Count(x => x.Label == PredefinedLabels.Text) == 1)
                    {
                        textCount++;
                    }
                }
            }

            textCount.Should().Be(0);
            longCount.Should().Be(2);
        }
    }
}
