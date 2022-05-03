using Microsoft.Extensions.Configuration;
using Shared.ComponentInterfaces;
using Shared.Models;
using Shared.Models.ObjectAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DatasetDecider
{
    public class DatasetClassifier : IDatasetClassifier
    {
        private readonly IConfiguration _configuration;
        private readonly DatasetLookupTable _lookupTable;
        private readonly List<string> _objectLabels = new List<string>();
        public DatasetClassifier(IConfiguration configuration)
        {
            _configuration = configuration;
            _lookupTable = GenerateLookuptable(configuration["Input:DatasetDeciderLookupPath"]);
        }

        private DatasetLookupTable GenerateLookuptable(string lookupTablePath)
        {
            var json = File.ReadAllText(lookupTablePath);
            DatasetLookupTable? table = JsonSerializer.Deserialize<DatasetLookupTable>(json);
            return table;
        }

        public async Task Classify(DatasetObject dataset)
        {
         await Task.Run(() => {
            foreach (var obj in dataset.Objects)
            {
                foreach (var attr in obj.Attributes)
                {
                    GetLabels(attr);
                }
            }
            Dictionary<string, float> datasetTypeScore = new Dictionary<string, float>();
            foreach (var item in _lookupTable.TitleSpecification)
            {
                if (datasetTypeScore.ContainsKey(item.DatasetClassification) == false)
                {
                    datasetTypeScore.Add(item.DatasetClassification, 0);
                }
            }
            foreach (var item in _lookupTable.ContentSpecification)
            {
                if (datasetTypeScore.ContainsKey(item.DatasetClassification) == false)
                {
                    datasetTypeScore.Add(item.DatasetClassification, 0);
                }
            }
            foreach (var item in datasetTypeScore)
            {
                float score = 0;
                score += GetTitleScore(item, dataset);
                score += GetContentScore(item);
                datasetTypeScore[item.Key] = score;
            }
                var pairMaxValue = datasetTypeScore.Aggregate((x, y) => x.Value > y.Value ? x : y);

                if (pairMaxValue.Value == 0)
                {
                    dataset.DatasetType = (DatasetType)Enum.Parse(typeof(DatasetType), _lookupTable.DefaultSpecification);
                }
                else
                {
                    dataset.DatasetType = (DatasetType)Enum.Parse(typeof(DatasetType), pairMaxValue.Key);
                }
            });
        }

        private void GetLabels(ObjectAttribute attr)
        {
            foreach (var label in attr.Labels)
            {
                if (_objectLabels.Contains(label.Label) == false)
                {
                    _objectLabels.Add(label.Label);
                }
            }
            if (attr.GetType() == typeof(ListAttribute))
            {
                var children = (List<ObjectAttribute>)attr.Value;
                foreach (var child in children)
                {
                    GetLabels(child);
                }
            }
        }

        private float GetContentScore(KeyValuePair<string, float> item)
        {
            var contentSpecification = _lookupTable.ContentSpecification.Find(x => x.DatasetClassification == item.Key);
            bool reqMet = true;
            if (contentSpecification != null)
            {
                foreach (var req in contentSpecification.Requirements)
                {
                    if (_objectLabels.Contains(req) == false)
                    {
                        reqMet = false;
                    }
                }
                if (reqMet)
                {
                    return contentSpecification.Score;
                }  
                else
                {
                    return 0;
                }
            }
            else
            {
                return 0;
            }
        }

        private float GetTitleScore(KeyValuePair<string, float> item, DatasetObject dataset)
        {
            var titleSpecification = _lookupTable.TitleSpecification.Find(x => x.DatasetClassification == item.Key);
            if (titleSpecification != null)
            {
                foreach (var req in titleSpecification.Requirements)
                {
                    if (dataset.originalName.Contains(req, StringComparison.InvariantCultureIgnoreCase))
                    {
                        return titleSpecification.Score;
                    }
                }
            }
            return 0;
        }
    }

}
