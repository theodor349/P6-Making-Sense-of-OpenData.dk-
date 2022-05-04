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
            return Task.FromResult(res);

            foreach (var io in dataset.Objects)
            {
                res.Add(GenerateParkingSpot(io));
            }

            return Task.FromResult(res);
        }

        private IntermediateOutput GenerateParkingSpot(IntermediateObject io)
        {
            var res = new ParkingSpot();
            res.GeoFeatures = GetMultiPolygon(io);
            return res;
        }

        private MultiPolygon GetMultiPolygon(IntermediateObject io)
        {
            var res = new MultiPolygon();
            var polygons = new List<Polygon>();
            polygons.Add(GetPolygon(io));
            return res;
        }

        private Polygon GetPolygon(IntermediateObject io)
        {
            var res = new Polygon();
            var polygonAttribute = FindPolygonAttribute(io.Attributes);
            if(polygonAttribute != null)
                res.Coordinates = GetCoordinates(polygonAttribute);
            return res;
        }

        private List<Point> GetCoordinates(ListAttribute polygonAttribute)
        {
            throw new NotImplementedException();
        }

        private ListAttribute? FindPolygonAttribute(List<ObjectAttribute> attributes)
        {
            ListAttribute? res = null;
            foreach (var attribute in attributes)
            {
                res = FindPolygonAttribute(attribute);
                if (res is not null)
                    return res;
            }
            return res;
        }

        private ListAttribute FindPolygonAttribute(ObjectAttribute attributes)
        {
            throw new NotImplementedException();
        }
    }
}
