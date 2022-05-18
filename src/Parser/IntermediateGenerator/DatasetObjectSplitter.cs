using Shared.Models;
using Shared.Models.ObjectAttributes;
using DatasetGenerator.Models;

namespace DatasetGenerator
{
    public class IntermediateDatasetSplitter : IDatasetObjectSplitter
    {
        List<Pattern> patternList = new List<Pattern>();

        int depth = 1;

        public async Task<DatasetObject> SplitObject(DatasetObject datasetObject)
        {
            foreach (IntermediateObject intermediateObject in datasetObject.Objects){
                RegisterPattern(new Pattern(intermediateObject, depth));
                foreach (ObjectAttribute attribute in intermediateObject.Attributes)
                {
                    CheckAttributeForSearch(attribute);
                }
            }
            return GenerateIntermediateObjects(datasetObject);
        }

        private DatasetObject GenerateIntermediateObjects(DatasetObject datasetObject)
        {
            patternList.Sort();

            foreach (Pattern item in patternList)
            {
                if (item.Count > 1)
                {
                    datasetObject = GenerateDatasetObject(item, datasetObject);
                    break;
                } 
            }

            return datasetObject;
        }

        private DatasetObject GenerateDatasetObject(Pattern item, DatasetObject datasetObject)
        {
            depth = 1;
            DatasetObject newDatasetObject = new DatasetObject(datasetObject);
            foreach (IntermediateObject intermediateObject in datasetObject.Objects)
            {
                CheckPatternAndDepth(item, intermediateObject, newDatasetObject);
            }
            return newDatasetObject;
        }

        private void CheckPatternAndDepth(Pattern item, IntermediateObject intermediateObject, DatasetObject datasetObject)
        {
            if (item.Depth < depth)
                return;
            else if (item.Depth > depth)
            {
                foreach (ObjectAttribute attribute in intermediateObject.Attributes)
                {
                    if (attribute.GetType() == typeof(ListAttribute))
                    {
                        depth++;
                        CheckPatternAndDepth(item, (ListAttribute)attribute, datasetObject);
                        depth--;
                    }
                }
            }
            else
            {
                if (item.Equals(new Pattern(intermediateObject, depth)))
                {
                    datasetObject.Objects.Add(intermediateObject);
                }
            }
        }

        private void CheckPatternAndDepth(Pattern item, ListAttribute attribute, DatasetObject datasetObject)
        {
            if (item.Depth < depth)
                return;
            else if (item.Depth > depth)
            {
                foreach (ObjectAttribute objAttr in (List<ObjectAttribute>)attribute.Value)
                {
                    if (objAttr.GetType() == typeof(ListAttribute))
                    {
                        depth++;
                        CheckPatternAndDepth(item, (ListAttribute)objAttr, datasetObject);
                        depth--;
                    }
                }
            }
            else
            {
                if (item.Equals(new Pattern(attribute, depth)))
                {
                    datasetObject.Objects.Add(new IntermediateObject(attribute));
                }
            }
        }
    

        private void CheckAttributeForSearch(ObjectAttribute attribute)
        {
            if (attribute.GetType() == typeof(ListAttribute))
            {
                SearchListObjectForPatterns((ListAttribute)attribute);
            }
        }

        private void RegisterPattern(Pattern pattern)
        {
            if (patternList.Contains(pattern))
            {
                patternList.Find(x => x.Equals(pattern)).Count++;
            }
            else patternList.Add(pattern);
        }

        private void SearchListObjectForPatterns(ListAttribute list)
        {
            depth++;
            RegisterPattern(new Pattern(list, depth));
            foreach (ObjectAttribute attribute in (List<ObjectAttribute>)list.Value)
            {
                CheckAttributeForSearch((ObjectAttribute)attribute);
            }
            depth--;
        }
    }
}

