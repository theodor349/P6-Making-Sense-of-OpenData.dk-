using Shared.Models;
using Shared.Models.ObjectAttributes;
using IntermediateGenerator.Models;

namespace IntermediateGenerator
{
    public class IntermediateObjectSplitter : IIntermediateObjectSplitter
    {
        int depth = 0;
        public async Task<DatasetObject> SplitObject(DatasetObject datasetObject)
        {
            foreach (IntermediateObject intermediateObject in datasetObject.Objects){
                RegisterPattern();
                depth++;
                foreach (ObjectAttribute attribute in intermediateObject.Attributes)
                {
                    CheckAttributeForSearch(attribute);
                }
            }
            return datasetObject;
        }

        private void CheckAttributeForSearch(ObjectAttribute attribute)
        {
            if (attribute.GetType() == typeof(List<ObjectAttribute>))
            {
                SearchListObjectForPatterns((ListAttribute)attribute);
            }
        }

        private void RegisterPattern()
        {
            List<Pattern> patternList = new List<Pattern>();

        }

        private void SearchListObjectForPatterns(ListAttribute list)
        {
            RegisterPattern();
            depth++;
            foreach (ObjectAttribute attribute in (List<ObjectAttribute>)list.Value)
            {
                CheckAttributeForSearch((ObjectAttribute)attribute);
            }
            depth--;
        }
    }
}

