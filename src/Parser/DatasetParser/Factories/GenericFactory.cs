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
            var threads = new List<Task<IntermediateOutput>>();

            for (int i = 0; i < dataset.Objects.Count; i++)
            {
                IntermediateObject? io = dataset.Objects[i];
                var thread = GenerateModelAsync(io, Description, dataset.Crs, i);
                threads.Add(thread);
            }

            Task.WaitAll(threads.ToArray());
            foreach (var thread in threads)
            {
                res.Add(thread.Result);
            }

            return Task.FromResult(res);
        }

        private Task<IntermediateOutput> GenerateModelAsync(IntermediateObject io, SpecializationDescription description, CoordinateReferenceSystem crs, int i)
        {
            return Task.Run(() => GenerateModel(io, description, crs, i));
        }

        private IntermediateOutput GenerateModel(IntermediateObject io, SpecializationDescription description, CoordinateReferenceSystem crs, int i)
        {
            var properties = new List<SpecializationProperty>();
            ObjectAttribute geoFeatureObject = null;

            var temp = new SpecializationPropertyDescription[description.Properties.Count()];
            description.Properties.CopyTo(temp);
            var labels = temp.ToList();
            labels.Add(new SpecializationPropertyDescription(description.GeoFeatureType.ToString(), new List<string>() { description.GeoFeatureType.ToString() }));
            var finds = LabelFinder.FindLabels(io, labels);
            foreach (var find in finds.Result)
            {
                if (geoFeatureTargets.Contains(find.Key))
                    geoFeatureObject = finds.BestFit(find.Key);
                else
                {
                    var fit = finds.BestFit(find.Key);
                    if(fit != null)
                        properties.Add(new SpecializationProperty(find.Key, fit.Value));
                }
            }

            var model = GenerateSpecialization(description, geoFeatureObject, properties, crs);
            model.Properties.Add(new SpecializationProperty("Index", i));
            return model;
        }

        private IntermediateOutput GenerateSpecialization(SpecializationDescription description, ObjectAttribute? geoFeatureObject, List<SpecializationProperty> properties, CoordinateReferenceSystem crs)
        {
            switch (description.GeoFeatureType)
            {
                case GeoFeatureType.Point:
                    //return new GenericSpecialization<Point>(GetPoint(), properties);
                    throw new NotImplementedException();
                case GeoFeatureType.MultiPoint:
                    throw new NotImplementedException();
                case GeoFeatureType.LineString:
                    return new GenericSpecialization<LineString>(GetLineString(geoFeatureObject, crs), properties);
                case GeoFeatureType.MultiLineString:
                    throw new NotImplementedException();
                case GeoFeatureType.Polygon:
                    throw new NotImplementedException();
                case GeoFeatureType.MultiPolygon:
                    return new GenericSpecialization<MultiPolygon>(GetMultiPolygon(geoFeatureObject, crs), properties);
                default:
                    throw new NotImplementedException();
            }
        }

        private MultiPolygon GetMultiPolygon(ObjectAttribute? geoFeatureObject, CoordinateReferenceSystem crs)
        {
            var multiPolygon = new MultiPolygon();
            var polygons = new List<Polygon>();
            foreach (var p in (List<ObjectAttribute>)geoFeatureObject.Value)
            {
                polygons.Add(GetPolygon(p, crs));
            }
            multiPolygon.Polygons = polygons;
            return multiPolygon;
        }

        private Polygon GetPolygon(ObjectAttribute polygonAttribute, CoordinateReferenceSystem crs)
        {
            var res = new Polygon();
            if (polygonAttribute != null)
                res.Coordinates = GetCoordinates((ListAttribute)polygonAttribute, crs);
            return res;
        }

        private LineString GetLineString(ObjectAttribute? polygonAttribute, CoordinateReferenceSystem crs)
        {
            var res = new LineString();
            if (polygonAttribute != null)
                res.Coordinates = GetCoordinates((ListAttribute)polygonAttribute, crs);
            return res;
        }

        private List<Point> GetCoordinates(ListAttribute attribute, CoordinateReferenceSystem crs)
        {
            var points = new List<Point>();
            foreach (var child in (List<ObjectAttribute>)attribute.Value)
            {
                points.Add(GetCoordinate(child, crs));
            }
            return points;
        }

        private Point GetCoordinate(ObjectAttribute child, CoordinateReferenceSystem crs)
        {
            var coord = new GenericCoordinate(child, crs);
            return new Point(coord.Longitude, coord.Latitude);
        }

        private double GetDouble(ObjectAttribute objectAttribute)
        {
            return (double)objectAttribute.Value;
        }
    }
}
