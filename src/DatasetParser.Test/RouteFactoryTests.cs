using AutoFixture;
using AutoFixture.Kernel;
using DatasetParser.Test.Utilities;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shared.Models;
using Shared.Models.ObjectAttributes;
using Shared.Models.Output;
using Shared.Models.Output.Specializations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatasetParser.Test
{
    class ListAttributeRoute : ISpecimenBuilder
    {
        ISpecimenContext _context;

        public object Create(object request, ISpecimenContext context)
        {
            _context = context;
            if(request is Type type && type == typeof(ListAttribute))
            {
                return GeneratePolygon();
            }
            return new NoSpecimen();
        }

        private ListAttribute GeneratePolygon()
        {
            var points = GeneratePoints(4);
            var polygon = new ListAttribute("", points);
            polygon.AddLabel(PredefinedLabels.List, 1);
            polygon.AddLabel(PredefinedLabels.Polygon, 1);
            return polygon;
        }

        private List<ObjectAttribute> GeneratePoints(int n)
        {
            var points = new List<ObjectAttribute>();
            for (int i = 0; i < n; i++)
            {
                points.Add(GeneratePoint());
            }
            return points;
        }

        private ListAttribute GeneratePoint()
        {
            var values = new List<ObjectAttribute>();
            values.Add(GenerateDouble());
            values.Add(GenerateDouble());
            var point = new ListAttribute("", values);
            point.AddLabel(PredefinedLabels.List, 1);
            point.AddLabel(PredefinedLabels.Point, 1);
            return point;
        }

        private DoubleAttribute GenerateDouble()
        {
            var res = new DoubleAttribute("", _context.Create<double>());
            res.AddLabel(PredefinedLabels.Double, 1);
            return res;
        }
    }

    class IntermediateObjectRoute : ISpecimenBuilder
    {
        ISpecimenContext _context;

        public object Create(object request, ISpecimenContext context)
        {
            _context = context;
            if (request is Type type && type == typeof(IntermediateObject))
            {
                var attributes = _context.CreateMany<ListAttribute>(1).ToList().ConvertAll(x => (ObjectAttribute)x);
                return new IntermediateObject(attributes);
            }
            return new NoSpecimen();
        }
    }

    [TestClass]
    public class RouteFactoryTests
    {
        [TestMethod]
        public void Route_SingleIO_GetsPolygon()
        {
            var fixture = new Fixture();
            fixture.Customizations.Add(new IntermediateObjectRoute());
            fixture.Customizations.Add(new ListAttributeRoute());
            fixture.Customizations.Add(
                new TypeRelay(
                    typeof(ObjectAttribute),
                    typeof(ListAttribute)));
            var objects = fixture.CreateMany<IntermediateObject>(1).ToList();
            var dataset = new DatasetObject("filename.geojson", "geojson", objects);
            var iteration = fixture.Create<int>();

            var setup = new TestSetup();
            var factory = setup.RouteFactory();

            var task = factory.BuildDataset(dataset, iteration);
            task.Wait();
            var res = task.Result.ConvertAll(x => (ParkingSpot)x);

            res.Count.Should().Be(objects.Count);
            res[0].GeoFeatures.Polygons.Count.Should().Be(objects[0].Attributes.Count);
            EvaluateLinering(objects[0].Attributes[0], res[0].GeoFeatures.Polygons[0]);
        }

        private void EvaluateLinering(ObjectAttribute objectAttribute, Polygon polygon)
        {
            var list = (List<ObjectAttribute>)objectAttribute.Value;
            for (int i = 0; i < list.Count; i++)
            {
                EvaluatePoint(list[i], polygon.Coordinates[i]);
            }
        }

        private void EvaluatePoint(ObjectAttribute objectAttribute, Point point)
        {
            var list = (List<ObjectAttribute>)objectAttribute.Value;
            var longi = (double)list[0].Value;
            var lati = (double)list[1].Value;

            point.Longitude.Should().Be(longi);
            point.Latitude.Should().Be(lati);
        }
    }
}
