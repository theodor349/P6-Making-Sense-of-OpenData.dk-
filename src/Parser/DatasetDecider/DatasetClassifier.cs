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

        private class LabelInfo
        {
            public int amount;
            public float confidence;
        }


        private readonly IConfiguration _configuration;
        private readonly DatasetLookupTable _lookupTable;
        private readonly Dictionary<string, LabelInfo> _objectLabels = new Dictionary<string, LabelInfo>();

        private bool succesfullyClassified;
        private int totalDataSetObjects;
        private int totalClassifiedObjects;
        private int customLabeledObjects;

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

        public Task<OutputLogObject> Classify(DatasetObject dataset)
        {
             GetAllLabels(dataset);
             Dictionary<string, float> datasetTypeScore = GenerateDataTypeScores(dataset);

             var datasetTypeScoreMax = datasetTypeScore.Aggregate((x, y) => x.Value > y.Value ? x : y);

             if (datasetTypeScoreMax.Value == 0)
             {
                 succesfullyClassified = false;
                 dataset.DatasetType = (DatasetType)Enum.Parse(typeof(DatasetType), _lookupTable.DefaultSpecification);
             }
             else
             {
                 succesfullyClassified = true;
                 dataset.DatasetType = (DatasetType)Enum.Parse(typeof(DatasetType), datasetTypeScoreMax.Key);
             }

             List<DatasetClassification> datasetClassifications = new List<DatasetClassification>();

             foreach (var datasetClassification in datasetTypeScore)
             {
                 if (datasetClassification.Key != datasetTypeScoreMax.Key)
                 {
                     datasetClassifications.Add(new DatasetClassification() { Name = datasetClassification.Key, Score = datasetClassification.Value });
                 }
             }

             List<LabelClassification> datasetLabelClassifications = new List<LabelClassification>();

             foreach (var label in _objectLabels)
             {
                 datasetLabelClassifications.Add(new LabelClassification() { Label =  label.Key, Amount = label.Value.amount, Confidence = label.Value.confidence});
             }

             var result = new OutputLogObject(
                 dataset.originalName,
                 succesfullyClassified,
                 new DatasetClassification()
                 {
                     Name = datasetTypeScoreMax.Key.ToString(),
                     Score = datasetTypeScoreMax.Value
                 },
                 datasetClassifications,
                 datasetLabelClassifications,
                 totalDataSetObjects,
                 totalClassifiedObjects,
                 customLabeledObjects
            );

            return Task.FromResult(result);
        }

        private void GetAllLabels(DatasetObject dataset)
        {
            foreach (var obj in dataset.Objects)
            {
                foreach (var attr in obj.Attributes)
                {
                    GetLabels(attr);
                }
            }
        }

        private void GetLabels(ObjectAttribute attr)
        {
            totalDataSetObjects++;
            if(attr.Labels.Count > 0)
            {
                totalClassifiedObjects++;
            }
            bool hasCustomLabel = false;

            foreach (var label in attr.Labels)
            {              
                if (_objectLabels.ContainsKey(label.Label) == false)
                {
                    _objectLabels.Add(label.Label, new LabelInfo() { amount = 1, confidence = label.Probability});
                }
                else
                {
                    LabelInfo labelInfo = _objectLabels[label.Label];

                    float oldAmount = labelInfo.amount;
                    float oldPercentage = labelInfo.confidence;
                    float newPercentage = label.Probability;

                    float averagePercentage = ((oldAmount * oldPercentage) + newPercentage) / (oldAmount + 1);

                    _objectLabels[label.Label] = new LabelInfo() { amount = ++labelInfo.amount, confidence = averagePercentage};
                }
                if (PredefinedLabels.Labels.Contains(label.Label) == false)
                {
                    hasCustomLabel = true;
                }
            }

            if (hasCustomLabel)
            {
                customLabeledObjects++;
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

        private Dictionary<string, float> GenerateDataTypeScores(DatasetObject dataset)
        {
            var datasetTypeScore = new Dictionary<string, float>();
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
            return datasetTypeScore;
        }

        private float GetContentScore(KeyValuePair<string, float> item)
        {
            var contentSpecification = _lookupTable.ContentSpecification.Find(x => x.DatasetClassification == item.Key);
            bool reqMet = true;
            if (contentSpecification != null)
            {
                foreach (var req in contentSpecification.Requirements)
                {
                    if (_objectLabels.ContainsKey(req) == false)
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
