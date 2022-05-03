using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Models
{

    internal struct DatasetClassification
    {
        public string Name { get; set; }
        public float Score { get; set; }
    }

    internal struct LabelClassification
    {
        public int Amount { get; set; }
        public string Label { get; set; }
        public float Confidence { get; set; }
    }


    internal class OuputLogObject
    {
        public string FileName { get;}
        public DatasetClassification DatasetClassification {get;}
        public List<DatasetClassification> OtherClassifications {get;} = new List<DatasetClassification>();
        public List<LabelClassification> Labels {get;} = new List<LabelClassification>();
        public int TotalDataSetObjects { get; }
        public int TotalClassifiedObjects { get; }
        public int CustomLabeledObjects { get; }
        public int UnclassifiedObjects { get; }

        public float PercentageOfClassifiedObjects { 
            get
            {
                return TotalDataSetObjects / TotalClassifiedObjects * 100f;
            } 
        }
    }
}
