using Microsoft.Extensions.Configuration;
using Shared.Models;
using Shared.Models.ObjectAttributes;
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
        public Task AssignLabels(DatasetObject dataset)
        {
            foreach (var obj in dataset.Objects)
            {
                foreach (var attr in obj.Attributes)
                {
                    SetLabels(attr);
                }
            }
            return Task.CompletedTask;
        }

        private void SetLabels(ObjectAttribute attr)
        {
            //  ASSIGN Actual labels
            AssignLabelFromLookup();
            if (attr.GetType() == typeof(ListAttribute))
            {
               // Get children
               //    for each child
               //    SetLabels
            }
        }

        private void AssignLabelFromLookup()
        {
            throw new NotImplementedException();
        }

        private void GenerateLookuptable(string lookupTablePath)
        {
            var json = File.ReadAllText(lookupTablePath);
            LookupTable? table = JsonSerializer.Deserialize<LookupTable>(json);
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
