using Shared.Models;
using Shared.Models.ObjectAttributes;

namespace IntermediateGenerator
{
    public class IntermediateObjectSplitter : IIntermediateObjectSplitter
    {
        public async Task<DatasetObject> SplitObject(DatasetObject datasetObject)
        {
            foreach (IntermediateObject intermediateObject in datasetObject.Objects){
                RegisterPattern();
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
            throw new NotImplementedException();
        }

        private void SearchListObjectForPatterns(ListAttribute list)
        {
            RegisterPattern();
            foreach (ObjectAttribute attribute in (List<ObjectAttribute>)list.Value)
            {
                CheckAttributeForSearch((ObjectAttribute)attribute);
            }
        }
    }
}

