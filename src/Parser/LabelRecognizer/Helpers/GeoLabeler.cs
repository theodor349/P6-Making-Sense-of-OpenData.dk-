using Shared.Models;
using Shared.Models.ObjectAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabelRecognizer.Helpers
{
    public interface IGeoLabeler
    {
        Task AssignLabels(DatasetObject dataset);
    }

    public class GeoLabeler : IGeoLabeler
    {
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

        // Base case = Long/Double/Text...
        // Inductive = List
        private void SetLabels(ObjectAttribute attr)
        {
            if (attr.GetType() != typeof(ListAttribute))
                return;
            var children = (List<ObjectAttribute>)attr.Value;
            foreach (var a in children)
                SetLabels(a);

            // Logic
            AddCoordinateLabel(attr, children);
            AddPolygon(attr, children);
        }

        private void AddPolygon(ObjectAttribute attr, List<ObjectAttribute> children)
        {
            int points = 0;
            foreach (var child in children)
            {
                if(child.Labels.FirstOrDefault(x => x.Label == ObjectLabel.Coordinate) != null)
                    points++;
            }
            if (points > 1)
                attr.AddLabel(ObjectLabel.Polygon, 1);
        }

        private void AddCoordinateLabel(ObjectAttribute attr, List<ObjectAttribute> children)
        {
            if (children.Count != 2)
                return;
            var left = children[0].GetLabel(ObjectLabel.Double);
            if (left == null)
                return;
            var right = children[1].GetLabel(ObjectLabel.Double);
            if (right == null)
                return;
            attr.AddLabel(ObjectLabel.Coordinate, left.Probability * right.Probability);
        }
    }
}
