using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Models.Output
{
    public class OutputDataset
    {
        public List<IntermediateOutput> Objects { get; set; } = new List<IntermediateOutput>();
        public string originalFileName { get; set; }
        public string OriginalFileExstension { get; set; }

        public OutputDataset(string originalFileName, string originalFileExstension)
        {
            this.originalFileName = originalFileName;
            OriginalFileExstension = originalFileExstension;
        }
    }
}
