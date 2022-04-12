using Shared.Models.ObjectAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabelRecognizer
{
    internal class LookupTable
    {
        public List<LookupTarget>? LookupTargets { get; set; }
    }

    public class LookupTarget
    {
        public string? Target { get; set; }
        public List<LanguageObject>? Languages { get; set; }

    }

    public class LanguageObject
    {
        public string? Language { get; set; }
        public List<string>? Values { get; set; }
    }
}
