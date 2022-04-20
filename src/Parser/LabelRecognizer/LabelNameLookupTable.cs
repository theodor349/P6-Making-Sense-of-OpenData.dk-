using Microsoft.Extensions.Configuration;
using NHunspell;
using Shared.Models;
using Shared.Models.ObjectAttributes;
using System.Text.Json;

namespace LabelRecognizer
{
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
           // FindSynonym("blomst", LookupLanguages.DK);
           // FindSynonym("park", LookupLanguages.ENG);

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
            // If a value is found
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
                            return new Tuple<bool, float, ObjectLabel>(true, 1, (ObjectLabel)Enum.Parse(typeof(ObjectLabel), target.Target));
                        }
                    }
                }
            }
            return new Tuple<bool, float, ObjectLabel>(false, 0, new ObjectLabel());
        }

        public void FindSynonym(string inputWord, LookupLanguages lang)
        {
            if (lang == LookupLanguages.DK)
            {
                Hunspell hunspellDanish = new Hunspell("da_dk.aff", "da_dk.dic");
                MyThes thesDanish = new MyThes("th_da_dk.dat");

                ThesResult resDanish = thesDanish.Lookup(inputWord, hunspellDanish);
                if (resDanish.IsGenerated)
                {
                    Console.WriteLine("Generated over stem (The original word form wasn't in the thesaurus)");
                }
                foreach (ThesMeaning meaning in resDanish.Meanings)
                {
                    Console.WriteLine();
                    Console.WriteLine("  DK Meaning: " + meaning.Description);
                    foreach (string synonym in meaning.Synonyms)
                    {
                        Console.WriteLine("    DK Synonym: " + synonym);
                    }
                }
            } 
            else if (lang == LookupLanguages.ENG)
            {
                Hunspell hunspellEnglish = new Hunspell("en_us.aff", "en_us.dic");
                MyThes thesEnglish = new MyThes("th_en_us_v2.dat");

                ThesResult resEnglish = thesEnglish.Lookup(inputWord, hunspellEnglish);
                if (resEnglish.IsGenerated)
                {
                    Console.WriteLine("Generated over stem (The original word form wasn't in the thesaurus)");
                }
                foreach (ThesMeaning meaning in resEnglish.Meanings)
                {
                    Console.WriteLine();
                    Console.WriteLine("  ENG Meaning: " + meaning.Description);
                    foreach (string synonym in meaning.Synonyms)
                    {
                        Console.WriteLine("    ENG Synonym: " + synonym);
                    }
                }
            }
        }

        private LookupTable GenerateLookuptable(string lookupTablePath)
        {
            var json = File.ReadAllText(lookupTablePath);
            LookupTable? table = JsonSerializer.Deserialize<LookupTable>(json);
            return table;
        }
    }    
}
