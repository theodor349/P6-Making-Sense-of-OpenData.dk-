using AutoFixture;
using AutoFixture.Kernel;
using DatasetParser.Test.Utilities;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using Shared.Models.Output;
using Shared.Models.Output.Specializations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Printers.Tests
{
    class MultiPolygonSpecimen : ISpecimenBuilder
    {
        public object Create(object request, ISpecimenContext context)
        {
            if (request is Type type && type == typeof(Polygon))
            {
                return new Polygon()
                {
                    Coordinates = new List<Point>()
                    {
                        context.Create<Point>(),
                        context.Create<Point>(),
                        context.Create<Point>(),
                        context.Create<Point>(),
                    }
                };
            }
            return new NoSpecimen();
        }
    }

    class LineStringSpecimen : ISpecimenBuilder
    {
        public object Create(object request, ISpecimenContext context)
        {
            if (request is Type type && type == typeof(LineString))
            {
                return new LineString()
                {
                    Coordinates = new List<Point>()
                    {
                        context.Create<Point>(),
                        context.Create<Point>(),
                        context.Create<Point>(),
                        context.Create<Point>(),
                    }
                };
            }
            return new NoSpecimen();
        }
    }

    class SpecializationPropertySpecimen : ISpecimenBuilder
    {
        public object Create(object request, ISpecimenContext context)
        {
            if (request is Type type && type == typeof(SpecializationProperty))
            {
                return new SpecializationProperty("name_" + context.Create<string>(), "value_" + context.Create<string>());
            }
            return new NoSpecimen();
        }
    }

    [TestClass]
    public class GeoJsonTests
    {
        [TestMethod]
        public void Print_ParkingSpot_CorrectParkingData()
        {
            // Arange 
            var fixture = new Fixture();
            fixture.Customizations.Add(new MultiPolygonSpecimen());
            fixture.Customizations.Add(new SpecializationPropertySpecimen());
            fixture.Customizations.Add(
            new TypeRelay(
                typeof(IntermediateOutput),
                typeof(GenericSpecialization<MultiPolygon>)));

            var dataset = fixture.Create<OutputDataset>();
            var setup = new TestSetup();
            JObject? res = null;
            setup.OnPrintToFile(x => res = x);

            // Act
            var printer = setup.GeoJsonPrinter();
            printer.Print(dataset, 0);

            string s = res.ToString();

            // Assert
            res.Should().NotBeNull();
            res.Children().Should().ContainEquivalentOf(new JProperty("type", "FeatureCollection"));
            var children = GetIOs(res);
            for (int i = 0; i < dataset.Objects.Count; i++)
            {
                VerifyIOData(dataset.Objects[i], children[i]);
            }
        }

        private void VerifyIOData(IntermediateOutput intermediateOutput, JToken jToken)
        {
            var geoProperties = typeof(GeodataOutput<GeoFeature>).GetProperties().ToLookup(y => y.Name);
            var expected = intermediateOutput.Properties;

            var properties = jToken.Children().ToList()[2].ToList()[0].ToList().ConvertAll(x => (JProperty)x);

            properties.Count.Should().Be(expected.Count());
            foreach (var e in expected)
            {
                var v = e.Value.ToString();
                bool found = false;
                foreach (var p in properties)
                {
                    if (p.Name == e.Name)
                    {
                        p.Value.ToString().Should().Be(v);
                        found = true;
                    }
                }
                found.Should().BeTrue();
            }
        }

        [TestMethod]
        public void Print_ParkingSpot_CorrectMultiPolygon()
        {
            // Arange 
            var fixture = new Fixture();
            fixture.Customizations.Add(new MultiPolygonSpecimen());
            fixture.Customizations.Add(new SpecializationPropertySpecimen());
            fixture.Customizations.Add(
            new TypeRelay(
                typeof(IntermediateOutput),
                typeof(GenericSpecialization<MultiPolygon>)));

            var dataset = fixture.Create<OutputDataset>();
            var setup = new TestSetup();
            JObject? res = null;
            setup.OnPrintToFile(x => res = x);

            // Act
            var printer = setup.GeoJsonPrinter();
            printer.Print(dataset, 0);

            // Assert
            res.Should().NotBeNull();
            res.Children().Should().ContainEquivalentOf(new JProperty("type", "FeatureCollection"));
            var children = GetIOs(res);
            for (int i = 0; i < dataset.Objects.Count; i++)
            {
                VerifyIOMultiPolygon(dataset.Objects[i], children[i]);
            }
        }

        private List<JToken> GetIOs(JObject res)
        {
            return res.GetValue("features").Children().ToList();
        }

        private void VerifyIOMultiPolygon(IntermediateOutput intermediateOutput, JToken jToken)
        {
            var io = (GeodataOutput<MultiPolygon>)intermediateOutput;

            var geometry = jToken.Children().ToList()[1].ToList()[0];
            var multipolygonRoot = geometry.Children().ToList();
            var multipolygon = multipolygonRoot.Children().ToList()[1];
            var polygons = multipolygon.Children().ToList();
            for (int i = 0; i < io.GeoFeatures.Polygons.Count; i++)
            {
                VerifyPolygon(io.GeoFeatures.Polygons[i], polygons[i]);
            }
        }

        private void VerifyPolygon(Polygon expected, JToken polygon)
        {
            VerifyLineRing(expected.Coordinates, polygon[0]);
        }

        private void VerifyLineRing(List<Point> coordinates, JToken? lineRing)
        {
            for (int i = 0; i < coordinates.Count; i++)
            {
                VerifyPoint(coordinates[i], lineRing[i]);
            }
        }

        private void VerifyPoint(Point expected, JToken? point)
        {
            var coords = point.Children().ToList();
            var v0 = ((double)coords[0]);
            var v1 = ((double)coords[1]);
            v0.Should().Be(expected.Longitude);
            v1.Should().Be(expected.Latitude);
        }

        [TestMethod]
        public void Print_ParkingSpot_CorrectLineString()
        {
            // Arange 
            var fixture = new Fixture();
            fixture.Customizations.Add(new LineStringSpecimen());
            fixture.Customizations.Add(new SpecializationPropertySpecimen());
            fixture.Customizations.Add(
            new TypeRelay(
                typeof(IntermediateOutput),
                typeof(GenericSpecialization<LineString>)));

            var dataset = fixture.Create<OutputDataset>();
            var setup = new TestSetup();
            JObject? res = null;
            setup.OnPrintToFile(x => res = x);

            // Act
            var printer = setup.GeoJsonPrinter();
            printer.Print(dataset, 0);

            // Assert
            res.Should().NotBeNull();
            res.Children().Should().ContainEquivalentOf(new JProperty("type", "FeatureCollection"));
            var children = GetIOs(res);
            for (int i = 0; i < dataset.Objects.Count; i++)
            {
                var linestringObject = (GeodataOutput<LineString>)dataset.Objects[i];
                VerifyLineString(linestringObject.GeoFeatures.Coordinates, children[i]);
            }
        }

        private void VerifyLineString(List<Point> coordinates, JToken jToken)
        {

            var geometry = jToken.Children().ToList()[1].ToList()[0];
            var multipolygonRoot = geometry.Children().ToList();
            var multipolygon = multipolygonRoot.Children().ToList()[1];
            var lineString = multipolygon;
            VerifyLineRing(coordinates, lineString);
        }
    }
}