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
        private readonly IConfiguration _configuration;
        private readonly LookupTable _lookupTable;

        public LabelNameLookupTable(IConfiguration configuration)
        {
            _configuration = configuration;
            _lookupTable = GenerateLookuptable(configuration["Input:LabelNameLookupTablePath"]);
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
            // ASSIGN Actual labels
            AssignLabelFromLookup(attr);
            if (attr.GetType() == typeof(ListAttribute))
            {
                // Get children
                //   for each child
                //   SetLabels
                var children = (List<ObjectAttribute>)attr.Value;
                foreach (var child in children)
                {
                    SetLabels(child);
                }
            }
        }

        private void AssignLabelFromLookup(ObjectAttribute attr)
        {
            var labelTuple = Lookup(attr);
            if (labelTuple.Item1)
            {
                attr.AddLabel(labelTuple.Item3, labelTuple.Item2);
            }
        }

        private Tuple<bool, float, ObjectLabel> Lookup(ObjectAttribute attr)
        {
            foreach (var target in _lookupTable.LookupTargets)
            {
                foreach (var lang in target.Languages)
                {
                    foreach (var value in lang.Values)
                    {
                        if (attr.Name.Contains(value, StringComparison.InvariantCultureIgnoreCase))
                        {
                            return new Tuple<bool, float, ObjectLabel>(true, 1, (ObjectLabel) Enum.Parse(typeof(ObjectLabel), target.Target));
                        }
                    }
                }
            }
            return new Tuple<bool, float, ObjectLabel>(false, 0, new ObjectLabel());
        }

        private LookupTable GenerateLookuptable(string lookupTablePath)
        {
            var json = File.ReadAllText(lookupTablePath);
            LookupTable? table = JsonSerializer.Deserialize<LookupTable>(json);
            return table;
        }
    }    
}
