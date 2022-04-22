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
        private readonly Hunspell _hunspellDanish;
        private readonly MyThes _thesDanish;
        private readonly Hunspell _hunspellEnglish;
        private readonly MyThes _thesEnglish;

        public LabelNameLookupTable(IConfiguration configuration)
        {
            _configuration = configuration;
            _lookupTable = GenerateLookuptable(configuration["Input:LabelNameLookupTablePath"]);

            _hunspellDanish = new Hunspell("Dictionaries/da_dk.aff", "Dictionaries/da_dk.dic");
            _hunspellEnglish = new Hunspell("Dictionaries/en_us.aff", "Dictionaries/en_us.dic");
            _thesDanish = new MyThes("Dictionaries/th_da_dk.dat");
            _thesEnglish = new MyThes("Dictionaries/th_en_us_v2.dat");

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
            if (ContainsPredefinedName(attr))
            {
                AssignLabelFromLookup(attr);
            }
            
            if (attr.GetType() == typeof(ListAttribute))
            {
                var children = (List<ObjectAttribute>)attr.Value;
                foreach (var child in children)
                {
                    SetLabels(child);
                }
            }
        }

        private bool ContainsPredefinedName(ObjectAttribute attr)
        {
            switch (attr.Name)
            {
                // StartArray and StartObject won't have Value appended
                case "StartObject":
                case "StartArray":
                case "FloatValue":
                case "NoneValue":
                case "StartConstructorValue":
                case "PropertyNameValue":
                case "CommentValue":
                case "RawValue":
                case "IntegerValue":
                case "StringValue":
                case "BooleanValue":
                case "NullValue":
                case "UndefinedValue":
                case "EndObjectValue":
                case "EndArrayValue":
                case "EndConstructorValue":
                case "DateValue":
                case "BytesValue":
                case "DoubleValue":
                    return false;
                default:
                    return true;
            }
        }

        private void AssignLabelFromLookup(ObjectAttribute attr)
        {
            var labelTuple = Lookup(attr);
            // If a value is found
            foreach (var tuple in labelTuple)
            {
                attr.AddLabel(tuple.Item2, tuple.Item1);
            }
        }

        private List<Tuple<float, ObjectLabel>> Lookup(ObjectAttribute attr)
        {
            var list = new List<Tuple<float, ObjectLabel>>();
            var hasBeenSearched = new List<string>();

            foreach (var target in _lookupTable.LookupTargets)
            {
                bool targetFound = false;
                foreach (var lang in target.Languages)
                {
                    if (targetFound == true)
                    {
                        break;
                    }
                    foreach (var value in lang.Values)
                    {
                        List<string> synonyms = new List<string>();
                        if (attr.Name.Contains(value, StringComparison.InvariantCultureIgnoreCase))
                        {
                            list.Add(new Tuple<float, ObjectLabel>(1f, (ObjectLabel)Enum.Parse(typeof(ObjectLabel), target.Target)));
                            targetFound = true;
                            break;
                        }
                        else
                        {
                            if (lang.Language == "ENG" && !hasBeenSearched.Contains(attr.Name))
                            {
                                synonyms = FindSynonyms(value, LookupLanguages.ENG);
                                hasBeenSearched.Add(attr.Name);
                            }
                            else if (lang.Language == "DK" && !hasBeenSearched.Contains(attr.Name))
                            {
                                synonyms = FindSynonyms(value, LookupLanguages.DK);
                                hasBeenSearched.Add(attr.Name);
                            }
                            foreach (var synonym in synonyms)
                            {
                                if (attr.Name.Contains(synonym, StringComparison.InvariantCultureIgnoreCase))
                                {
                                    list.Add(new Tuple<float, ObjectLabel>(0.5f, (ObjectLabel)Enum.Parse(typeof(ObjectLabel), target.Target)));
                                }
                            }
                        }  
                    }
                }
            }
            return list;
        }

        public List<string> FindSynonyms(string inputWord, LookupLanguages lang)
        {
            List<string> listOfSynonyms = new List<string>();
            if (lang == LookupLanguages.DK)
            {   
                ThesResult resDanish = _thesDanish.Lookup(inputWord, _hunspellDanish);
                if (resDanish != null)
                {
                    foreach (ThesMeaning meaning in resDanish.Meanings)
                    {
                        foreach (string synonym in meaning.Synonyms)
                        {
                            listOfSynonyms.Add(synonym);
                        }
                    }
                }
            } 
            else if (lang == LookupLanguages.ENG)
            {
                ThesResult resEnglish = _thesEnglish.Lookup(inputWord, _hunspellEnglish);
                if (resEnglish != null)
                {
                    foreach (ThesMeaning meaning in resEnglish.Meanings)
                    {
                        foreach (string synonym in meaning.Synonyms)
                        {
                            listOfSynonyms.Add(synonym);
                        }
                    }
                }
            }
            return listOfSynonyms;
        }

        private LookupTable GenerateLookuptable(string lookupTablePath)
        {
            var json = File.ReadAllText(lookupTablePath);
            LookupTable? table = JsonSerializer.Deserialize<LookupTable>(json);
            return table;
        }
    }    
}
