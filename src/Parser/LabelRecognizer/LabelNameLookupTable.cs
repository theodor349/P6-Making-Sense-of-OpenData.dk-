using Microsoft.Extensions.Configuration;
using System.Text.Json;

namespace LabelRecognizer
{
    public enum TargetKey { Count, Address }
    public enum LookupLanguages { DK, ENG }

    class LabelNameLookupTable : ILabelNameLookupTable
    {
        private Dictionary<TargetKey, LookupValue>? _dic = new();
        private readonly IConfiguration _configuration;

        public LabelNameLookupTable(IConfiguration configuration)
        {
            _configuration = configuration;
            GenerateLookuptable(configuration["Input:LabelNameLookupTablePath"]);
        }

        private void GenerateLookuptable(string lookupTablePath)
        {
            var json = File.ReadAllText(lookupTablePath);
            LookupTable table = JsonSerializer.Deserialize<LookupTable>(json);
        }

        public bool IncludesTarget(TargetKey target, string name, LookupLanguages language)
        {
            return _dic[target].Contains(name, language);
        }
    }

    internal class LookupValue
    {
        private Dictionary<LookupLanguages, List<string>> LangaugeValues = new();

        public bool Contains(string name, LookupLanguages language)
        {
            return LangaugeValues[language].Contains(name) ? true : false;
        }
    }
}
