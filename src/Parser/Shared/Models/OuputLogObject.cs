using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Models
{

    public struct DatasetClassification
    {
        public string Name { get; set; }
        public float Score { get; set; }
    }

    public struct LabelClassification
    {
        public int Amount { get; set; }
        public string Label { get; set; }
        public float Confidence { get; set; }
    }


    public class OutputLogObject
    {
        public OutputLogObject(string fileName, bool succesfullyClassified, DatasetClassification datasetClassification, List<DatasetClassification> otherClassifications, List<LabelClassification> labels, int totalDataSetObjects, int totalClassifiedObjects, int customLabeledObjects)
        {
            FileName = fileName;
            SuccesfullyClassified = succesfullyClassified;
            DatasetClassification = datasetClassification;
            OtherClassifications = otherClassifications;
            Labels = labels;
            TotalDataSetObjects = totalDataSetObjects;
            TotalClassifiedObjects = totalClassifiedObjects;
            CustomLabeledObjects = customLabeledObjects;
        }

        public string FileName { get; }
        public bool SuccesfullyClassified { get; }
        public DatasetClassification DatasetClassification { get; }
        public List<DatasetClassification> OtherClassifications { get; } = new List<DatasetClassification>();
        public List<LabelClassification> Labels { get; } = new List<LabelClassification>();
        public int TotalDataSetObjects { get; }
        public int TotalClassifiedObjects { get; }
        public int CustomLabeledObjects { get; }
        public int UnclassifiedObjects { 
            get
            {
                return TotalDataSetObjects - TotalClassifiedObjects;
            }
        }

        public float PercentageOfClassifiedObjects { 
            get
            {
                return TotalDataSetObjects / TotalClassifiedObjects * 100f;
            } 
        }
    }
}
