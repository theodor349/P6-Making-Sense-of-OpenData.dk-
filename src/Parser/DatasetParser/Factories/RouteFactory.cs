using Shared.Models;
using Shared.Models.ObjectAttributes;
using Shared.Models.Output;
using Shared.Models.Output.Specializations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatasetParser.Factories
{
    public interface IRouteFactory : IDatasetOutputFactory { }

    public class RouteFactory : IRouteFactory
    {
        public Task<List<IntermediateOutput>> BuildDataset(DatasetObject dataset, int iteration)
        {
            var res = new List<IntermediateOutput>();

            foreach (var io in dataset.Objects)
            {
                res.Add(GenerateParkingSpot(io));
            }

            return Task.FromResult(res);
        }

        private IntermediateOutput GenerateParkingSpot(IntermediateObject io)
        {
            var res = new RouteSpecialization();
            res.GeoFeatures = GetLinestrig(io);
            return res;
        }

        private LineString GetLinestrig(IntermediateObject io)
        {
            var res = new LineString();
            var polygonAttribute = FindLinestringAttribute(io.Attributes);
            if(polygonAttribute != null)
                res.Coordinates = GetCoordinates(polygonAttribute);
            return res;
        }

        private List<Point> GetCoordinates(ListAttribute attribute)
        {
            var points = new List<Point>();
            foreach (var child in (List<ObjectAttribute>)attribute.Value)
            {
                points.Add(GetCoordinate(child));
            }
            return points;
        }

        private Point GetCoordinate(ObjectAttribute child)
        {
            var children = (List<ObjectAttribute>)child.Value;
            var point = new Point();
            point.Longitude = GetDouble(children[0]);
            point.Latitude = GetDouble(children[1]);
            return point;
        }

        private double GetDouble(ObjectAttribute objectAttribute)
        {
            return (double)objectAttribute.Value;
        }

        private ListAttribute? FindLinestringAttribute(List<ObjectAttribute> attributes)
        {
            ListAttribute? res = null;
            foreach (var attribute in attributes)
            {
                res = FindLinestringAttribute(attribute);
                if (res is not null)
                    return res;
            }
            return res;
        }

        private ListAttribute FindLinestringAttribute(ObjectAttribute attribute)
        {
            ListAttribute? res = null;
            if(attribute is ListAttribute list)
            {
                if (list.HasLabel(PredefinedLabels.LineString))
                    return list;

                var children = (List<ObjectAttribute>)list.Value;
                foreach (var child in children)
                {
                    res = FindLinestringAttribute(child);
                    if (res is not null)
                        return res;
                }
            }
            return res;
        }
    }
}
