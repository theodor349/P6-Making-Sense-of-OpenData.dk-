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
            AddGeographicStructure(attr, children);
        }

        private void AddGeographicStructure(ObjectAttribute attr, List<ObjectAttribute> children)
        {
            int numCoordinates = GetCordinateCount(children);
            if (children.Count == 0 || numCoordinates != children.Count)
                return;

            bool sameStartAndEnd = IsSameCoordinate(children.First(), children.Last());
            if(IsPolygone(numCoordinates, sameStartAndEnd))
                attr.AddLabel(ObjectLabel.Polygon, 1);
        }

        private bool IsPolygone(int numCoordinates, bool sameStartAndEnd)
        {
            return numCoordinates > 3 && sameStartAndEnd;
        }

        private bool IsSameCoordinate(ObjectAttribute first, ObjectAttribute last)
        {
            if (!first.HasLabel(ObjectLabel.Coordinate) || !last.HasLabel(ObjectLabel.Coordinate))
                return false;

            var lat1 = GetLatitue((ListAttribute)first);
            var lat2 = GetLatitue((ListAttribute)last);
            if (lat1 != lat2)
                return false;

            var long1 = GetLongitude((ListAttribute)first);
            var long2 = GetLongitude((ListAttribute)first);
            return long1 == long2;
        }

        private double GetLongitude(ListAttribute obj)
        {
            var attr = (DoubleAttribute)((List<ObjectAttribute>)obj.Value).Last();
            return (double)attr.Value;
        }

        private double GetLatitue(ListAttribute obj)
        {
            var attr = (DoubleAttribute)((List<ObjectAttribute>)obj.Value).First();
            return (double)attr.Value;
        }

        private static int GetCordinateCount(List<ObjectAttribute> children)
        {
            int coordinates = 0;
            foreach (var child in children)
            {
                if (child.HasLabel(ObjectLabel.Coordinate))
                    coordinates++;
            }

            return coordinates;
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
