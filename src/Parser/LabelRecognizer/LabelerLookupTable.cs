using Shared.Models.ObjectAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabelRecognizer
{
    public class LabelerLookupTable
    {
        public List<LookupTarget> LookupTargets { get; set; } = new List<LookupTarget>();
    }

    public class LookupTarget
    {
        public string? Target { get; set; }
        public List<LanguageObject> Languages { get; set; } = new List<LanguageObject>();

    }

    public class LanguageObject
    {
        public string? Language { get; set; }
        public List<string> Values { get; set; } = new List<string>();
        public List<string> Synonyms { get; set; } = new List<string>();
    }
}
