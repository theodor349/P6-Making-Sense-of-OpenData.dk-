using LabelRecognizer.Tests.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shared.Models.ObjectAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;


namespace LabelRecognizer.Tests.Labeling
{
    [TestClass]
    public class Labeling
    {
        [DataRow(10,ObjectLabel.Long,10,ObjectLabel.Double)]
        [TestMethod]
        public void LabelGenerator_Dataset_Correctness(int amount1, ObjectLabel label1, int amount2, ObjectLabel label2)
        {
            var inputDataset = ModelFactory.GetDatasetObject(ModelFactory.GetIntermediateObjectList(amount1, () => ModelFactory.GetObjectAttr(label1), amount2, () => ModelFactory.GetObjectAttr(label2)));

            var setup = new TestSetup();
            var labelGenerator = setup.LabelGenerator();
            var res = labelGenerator.AddLabels(inputDataset);
            res.Wait();

            int countLong = 0;
            int countDouble = 0;

            foreach (var intermediateObj in inputDataset.Objects)
            {
                foreach (var objectAttr in intermediateObj.Attributes)
                {
                    if (objectAttr.Label.Labels == ObjectLabel.Long)
                    {
                        countLong++;
                    }
                    else if (objectAttr.Label.Labels == ObjectLabel.Double)
                    {
                        countDouble++;
                    }
                }
            }

            countLong.Should().Be(10);
            countDouble.Should().Be(10);
        }
    }
}
