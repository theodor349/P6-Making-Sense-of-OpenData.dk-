using LabelRecognizer.Models;
using Shared.Models;
using Shared.Models.ObjectAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabelRecognizer.Helpers
{
    public interface ITypeLabeler
    {
        Task AssignTypes(DatasetObject dataset);
    }

    public class TypeLabeler : ITypeLabeler
    {
        public Task AssignTypes(DatasetObject dataset)
        {
            var typeCounter = new TypeCounter("");
            IncrementTypes(dataset, typeCounter);
            SetTypes(dataset, typeCounter);

            return Task.CompletedTask;
        }

        private void SetTypes(DatasetObject dataset, TypeCounter typeCounter)
        {
            foreach (var intermediateObject in dataset.Objects)
            {
                foreach (var attr in intermediateObject.Attributes)
                {
                    var counter = typeCounter.Get(attr.Name);
                    SetType(attr, counter);
                }
            }
        }

        private void SetType(ObjectAttribute attribute, TypeCounter typeCounter)
        {
            AddLabels(attribute, typeCounter);
            AddLabelsToChildren(attribute, typeCounter);
        }

        private void AddLabelsToChildren(ObjectAttribute attribute, TypeCounter typeCounter)
        {
            if (attribute.GetType() == typeof(ListAttribute))
            {
                var list = (List<ObjectAttribute>)attribute.Value;
                for (int i = 0; i < list.Count; i++)
                {
                    SetType(list[i], typeCounter.Get(list[i].Name));
                }
            }
        }

        private static void AddLabels(ObjectAttribute attribute, TypeCounter typeCounter)
        {
            var totalLabelCount = typeCounter.Counter.Sum(x => x.Value);
            if (typeCounter.ContainsOnlyDoubleAndLong())
                attribute.AddLabel(ObjectLabel.Double, 1);
            else
            {
                foreach (var label in typeCounter.Counter)
                {
                    attribute.AddLabel(label.Key, ((float)label.Value) / totalLabelCount);
                }
            }
        }

        private void IncrementTypes(DatasetObject dataset, TypeCounter typeCounter)
        {
            foreach (var intermediateObject in dataset.Objects)
            {
                foreach (var attr in intermediateObject.Attributes)
                {
                    var counter = typeCounter.Get(attr.Name);
                    Increment(attr, counter);
                }
            }
        }

        private void Increment(ObjectAttribute attribute, TypeCounter typeCounter)
        {
            switch (attribute)
            {
                case NullAttribute a:
                    IncrementNull(a, typeCounter);
                    break;
                case ListAttribute a:
                    IncrementList(a, typeCounter);
                    break;
                case DateAttribute a:
                    IncrementDate(a, typeCounter);
                    break;
                case DoubleAttribute a:
                    IncrementDouble(a, typeCounter);
                    break;
                case LongAttribute a:
                    IncrementLong(a, typeCounter);
                    break;
                case TextAttribute a:
                    IncrementText(a, typeCounter);
                    break;

                default:
                    throw new NotImplementedException("Type not handled: " + attribute.GetType());
            }
        }

        private void IncrementList(ListAttribute attribute, TypeCounter typeCounter)
        {
            typeCounter.Increment(ObjectLabel.List);
            var list = (List<ObjectAttribute>)attribute.Value;
            foreach (var attr in list)
            {
                var newTypeCounter = typeCounter.Get(attr.Name);
                Increment(attr, newTypeCounter);
            }
        }

        private void IncrementNull(NullAttribute attribute, TypeCounter typeCounter)
        {
            typeCounter.Increment(ObjectLabel.Null);
        }

        private void IncrementDate(DateAttribute attribute, TypeCounter typeCounter)
        {
            typeCounter.Increment(ObjectLabel.Date);
        }

        private void IncrementDouble(DoubleAttribute attribute, TypeCounter typeCounter)
        {
            typeCounter.Increment(ObjectLabel.Double);
        }

        private void IncrementLong(LongAttribute attribute, TypeCounter typeCounter)
        {
            typeCounter.Increment(ObjectLabel.Long);
        }

        private void IncrementText(TextAttribute attribute, TypeCounter typeCounter)
        {
            typeCounter.Increment(ObjectLabel.Text);
        }
    }
}