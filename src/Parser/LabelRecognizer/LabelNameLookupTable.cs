using Microsoft.Extensions.Configuration;

namespace LabelRecognizer
{
    public enum TargetKey { Count, Address }
    public enum LookupLanguages { DK, ENG }

    class LabelNameLookupTable : ILabelNameLookupTable
    {
        private Dictionary<TargetKey, LookupValue> _dic = new();
        private readonly IConfiguration _configuration;

        public LabelNameLookupTable(IConfiguration configuration)
        {
            _configuration = configuration;
            GenerateLookuptable(configuration.GetSection("LabelNameLookupTable").ToString());
        }

        private void GenerateLookuptable(string configurationSection)
        {
            Console.WriteLine(configurationSection);
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
