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
            polygon.AddLabel(PredefinedLabels.LineString, 1);
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

        private string name = "name";
        private string description = "description";

        public IntermediateObjectRoute()
        {

        }

        public IntermediateObjectRoute(string name, string description)
        {
            this.name = name;
            this.description = description;
        }

        public object Create(object request, ISpecimenContext context)
        {
            _context = context;
            if (request is Type type && type == typeof(IntermediateObject))
            {
                var attributes = _context.CreateMany<ListAttribute>(1).ToList().ConvertAll(x => (ObjectAttribute)x);
                var nameAttribute = new TextAttribute("name", name);
                nameAttribute.AddLabel("Name", 1);
                var descriptionAttribute = new TextAttribute("description", description);
                descriptionAttribute.AddLabel("Description", 1);
                attributes.Add(nameAttribute);
                attributes.Add(descriptionAttribute);
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
            var factory = setup.GenericFactoryRoute();

            var task = factory.BuildDataset(dataset, iteration);
            task.Wait();
            var res = task.Result.ConvertAll(x => (GenericSpecialization<LineString>)x);

            res.Count.Should().Be(objects.Count);
            EvaluateLinering(objects[0].Attributes[0], res[0].GeoFeatures);
        }

        private void EvaluateLinering(ObjectAttribute objectAttribute, LineString lineString)
        {
            var list = (List<ObjectAttribute>)objectAttribute.Value;
            for (int i = 0; i < list.Count; i++)
            {
                EvaluatePoint(list[i], lineString.Coordinates[i]);
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

        [DataRow("name0", "Description0")]
        [DataRow("name1", "Description1")]
        [TestMethod]
        public void Route_SingleIO_GetsData(string name, string description)
        {
            var fixture = new Fixture();
            fixture.Customizations.Add(new IntermediateObjectRoute(name, description));
            fixture.Customizations.Add(new ListAttributeRoute());
            fixture.Customizations.Add(
                new TypeRelay(
                    typeof(ObjectAttribute),
                    typeof(ListAttribute)));
            var objects = fixture.CreateMany<IntermediateObject>(1).ToList();
            var dataset = new DatasetObject("filename.geojson", "geojson", objects);
            var iteration = fixture.Create<int>();

            var setup = new TestSetup();
            var factory = setup.GenericFactoryRoute();

            var task = factory.BuildDataset(dataset, iteration);
            task.Wait();
            var res = task.Result.ConvertAll(x => (GenericSpecialization<LineString>)x);

            res.First().Properties.First(x => x.Name == "Name").Value.Should().BeEquivalentTo(name);
            res.First().Properties.First(x => x.Name == "Description").Value.Should().BeEquivalentTo(description);
        }
    }
}
