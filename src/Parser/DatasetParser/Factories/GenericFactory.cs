using DatasetParser.Helper;
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
    public interface IGenericFactory : IDatasetOutputFactory { }
    public class GenericFactory : IGenericFactory
    {
        public SpecializationDescription Description { get; set; }
        private List<string> geoFeatureTargets;

        public GenericFactory(SpecializationDescription description)
        {
            Description = description;
            geoFeatureTargets = Enum.GetNames(typeof(GeoFeatureType)).ToList();
        }

        public Task<List<IntermediateOutput>> BuildDataset(DatasetObject dataset, int iteration)
        {
            var res = new List<IntermediateOutput>();

            foreach (var io in dataset.Objects)
            {
                res.Add(GenerateModel(io, Description));
            }

            return Task.FromResult(res);
        }

        private IntermediateOutput GenerateModel(IntermediateObject io, SpecializationDescription description)
        {
            var properties = new List<SpecializationProperty>();
            ObjectAttribute geoFeatureObject = null;

            var labels = description.GetLabels();
            var finds = LabelFinder.FindLabels(io, labels);
            foreach (var find in finds.Result)
            {
                if (geoFeatureTargets.Contains(find.Key))
                    geoFeatureObject = finds.BestFit(find.Key);
                else 
                    properties.Add(new SpecializationProperty(find.Key, finds.BestFit(find.Key).Value));
            }

            return GenerateSpecialization(description, geoFeatureObject, properties);
        }

        private IntermediateOutput GenerateSpecialization(SpecializationDescription description, ObjectAttribute? geoFeatureObject, List<SpecializationProperty> properties)
        {
            switch (description.GeoFeatureType)
            {
                case GeoFeatureType.Point:
                    //return new GenericSpecialization<Point>(GetPoint(), properties);
                    throw new NotImplementedException();
                case GeoFeatureType.MultiPoint:
                    throw new NotImplementedException();
                case GeoFeatureType.LineString:
                    return new GenericSpecialization<LineString>(GetLineString(geoFeatureObject), properties);
                case GeoFeatureType.MultiLineString:
                    throw new NotImplementedException();
                case GeoFeatureType.Polygon:
                    throw new NotImplementedException();
                case GeoFeatureType.MultiPolygon:
                    throw new NotImplementedException();
                default:
                    throw new NotImplementedException();
            }
        }

        private LineString GetLineString(ObjectAttribute? polygonAttribute)
        {
            var res = new LineString();
            if (polygonAttribute != null)
                res.Coordinates = GetCoordinates((ListAttribute)polygonAttribute);
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
    }
}
