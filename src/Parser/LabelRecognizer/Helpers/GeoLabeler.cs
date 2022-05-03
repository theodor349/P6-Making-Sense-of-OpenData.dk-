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
            foreach (var child in children)
                SetLabels(child);

            // Logic
            AddCoordinateLabel(attr, children);
            AddGeographicStructure(attr, children);
        }

        private void AddGeographicStructure(ObjectAttribute attr, List<ObjectAttribute> children)
        {
            if (children.Count == 0)
                return;

            GenericCoordinate.FixSymetetricPolygonStructure(children, GenericCoordinate.IsSymetric(children));
            int numCoordinates = GetCordinateCount(children);
            if (children.Count == 0 || numCoordinates != children.Count)
                return;

            bool sameStartAndEnd = GenericCoordinate.IsSameCoordinate(children.First(), children.Last());
            if(IsPolygon(numCoordinates, sameStartAndEnd))
                attr.AddLabel(PredefinedLabels.Polygon, 1);
            if (IsLine(numCoordinates, sameStartAndEnd))
                attr.AddLabel(PredefinedLabels.LineString, 1);
            if (IsMultiPoint(numCoordinates, sameStartAndEnd))
                attr.AddLabel(PredefinedLabels.MultiPoint, 1);
        }

        private bool IsMultiPoint(int numCoordinates, bool sameStartAndEnd)
        {
            return numCoordinates > 0 && !sameStartAndEnd;
        }

        private bool IsLine(int numCoordinates, bool sameStartAndEnd)
        {
            return numCoordinates > 0 && !sameStartAndEnd;
        }

        private bool IsPolygon(int numCoordinates, bool sameStartAndEnd)
        {
            return numCoordinates > 3 && sameStartAndEnd;
        }

        private static int GetCordinateCount(List<ObjectAttribute> children)
        {
            int coordinates = 0;
            foreach (var child in children)
            {
                if (child.HasLabel(PredefinedLabels.Point))
                    coordinates++;
            }

            return coordinates;
        }

        private void AddCoordinateLabel(ObjectAttribute attr, List<ObjectAttribute> children)
        {
            if (children.Count != 2)
                return;
            var left = children[0].GetLabel(PredefinedLabels.Double);
            if (left == null)
                return;
            var right = children[1].GetLabel(PredefinedLabels.Double);
            if (right == null)
                return;
            attr.AddLabel(PredefinedLabels.Point, left.Probability * right.Probability);
        }
    }
}
