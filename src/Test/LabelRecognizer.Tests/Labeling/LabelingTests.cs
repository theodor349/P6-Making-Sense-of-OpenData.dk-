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
        [DataRow(10, ObjectLabel.Long, 11, ObjectLabel.Double)]
        [TestMethod]
        public void LabelGenerator_Dataset_Correctness(int amount1, ObjectLabel label1, int amount2, ObjectLabel label2)
        {
            var ios = new List<IntermediateObject>();
            ios.AddRange(ModelFactory.GetIntermediateObjectList(amount1, () => ModelFactory.GetObjectAttr(label1)));
            ios.AddRange(ModelFactory.GetIntermediateObjectList(amount2, () => ModelFactory.GetObjectAttr(label2)));
            var inputDataset = ModelFactory.GetDatasetObject(ios);

            var setup = new TestSetup();
            var labelGenerator = setup.LabelGenerator();
            var res = labelGenerator.AddLabels(inputDataset);
            res.Wait();

            int count1 = 0;
            int count2 = 0;

            foreach (var intermediateObj in inputDataset.Objects)
            {
                foreach (var objectAttr in intermediateObj.Attributes)
                {
                    if (objectAttr.Labels.Count(x => x.Label == label1) == 1)
                    {
                        count1++;
                    }
                    else if (objectAttr.Labels.Count(x => x.Label == label2) == 1)
                    {
                        count2++;
                    }
                }
            }

            count1.Should().Be(amount1);
            count2.Should().Be(amount2);
        }
    }
}
