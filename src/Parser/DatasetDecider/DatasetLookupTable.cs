using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatasetDecider
{
    public class DatasetLookupTable
    {
        public List<Specification> TitleSpecification { get; set; } = new List<Specification>();
        public List<Specification> ContentSpecification { get; set; } = new List<Specification>();
        public string DefaultSpecification { get; set; } = string.Empty;
    }

    public class Specification
    {
        public string DatasetClassification { get; set; } = string.Empty;
        public float Score { get; set; } = 0;
        public List<string> Requirements { get; set; } = new List<string>();
    }
}

