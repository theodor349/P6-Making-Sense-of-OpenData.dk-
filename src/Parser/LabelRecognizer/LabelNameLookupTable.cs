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
        private readonly List<string> _hasBeenSearchedDanish = new List<string>();
        private readonly List<string> _hasBeenSearchedEnglish = new List<string>();
        public LabelNameLookupTable(IConfiguration configuration)
        {
            _configuration = configuration;

            _hunspellDanish = new Hunspell("Dictionaries/da_dk.aff", "Dictionaries/da_dk.dic");
            _hunspellEnglish = new Hunspell("Dictionaries/en_us.aff", "Dictionaries/en_us.dic");
            _thesDanish = new MyThes("Dictionaries/th_da_dk.dat");
            _thesEnglish = new MyThes("Dictionaries/th_en_us_v2.dat");

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
            if (!ContainsPredefinedName(attr))
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
                    return true;
                default:
                    return false;
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

            foreach (var target in _lookupTable.LookupTargets)
            {
                bool targetFound = false;
                foreach (var lang in target.Languages)
                {
                    if (targetFound)
                    {
                        break;
                    }
                    foreach (var value in lang.Values)
                    {
                        if (attr.Name.Contains(value, StringComparison.InvariantCultureIgnoreCase))
                        {
                            list.Add(new Tuple<float, ObjectLabel>(1f, (ObjectLabel)Enum.Parse(typeof(ObjectLabel), target.Target)));
                            targetFound = true;
                            break;
                        }
                        else
                        {
                            foreach (var synonym in lang.Synonyms)
                            {
                                if (attr.Name.Contains(synonym))
                                {
                                    list.Add(new Tuple<float, ObjectLabel>(0.5f, (ObjectLabel)Enum.Parse(typeof(ObjectLabel), target.Target)));
                                    targetFound = true;
                                    break;
                                }
                            }
                            if (targetFound)
                            {
                                break;
                            }
                        }  
                    }
                }
            }
            return list;
        }

        private LookupTable GenerateLookuptable(string lookupTablePath)
        {
            var json = File.ReadAllText(lookupTablePath);
            LookupTable? table = JsonSerializer.Deserialize<LookupTable>(json);

            foreach (LookupTarget target in table.LookupTargets)
            {
                foreach (LanguageObject lang in target.Languages)
                {
                    if (lang.Language == LookupLanguages.DK.ToString())
                    {
                        foreach (var value in lang.Values)
                        {
                            ThesResult resDanish = _thesDanish.Lookup(value, _hunspellDanish);
                            if (resDanish != null)
                            {
                                foreach (ThesMeaning meaning in resDanish.Meanings)
                                {
                                    foreach (string synonym in meaning.Synonyms)
                                    {
                                        lang.Synonyms.Add(synonym);
                                    }
                                }
                            }
                        }
                    }
                    else if (lang.Language == LookupLanguages.ENG.ToString())
                    {
                        foreach (var value in lang.Values)
                        {
                            ThesResult resEnglish = _thesEnglish.Lookup(value, _hunspellEnglish);
                            if (resEnglish != null)
                            {
                                foreach (ThesMeaning meaning in resEnglish.Meanings)
                                {
                                    foreach (string synonym in meaning.Synonyms)
                                    {
                                        lang.Synonyms.Add(synonym);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return table;
        }
    }    
}
