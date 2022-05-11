using LumenWorks.Framework.IO.Csv;
using Shared.Models;
using Shared.Models.ObjectAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Globalization;

namespace DatasetGenerator.ParseFile
{
    public interface ICsvParser : IFileParser
    {
    }

    internal class CsvParser : ICsvParser
    {
        private static Regex _multipolygonRegx = new Regex(@"\(\((([0-9]+.[0-9]+( )+[0-9]+.[0-9]+)(,( )*)*)*\)\)");
        private static Regex _polygonRegx = new Regex(@"\((([0-9]+.[0-9]+( )+[0-9]+.[0-9]+)(,( )*)*)*\)");
        private static Regex _pointRegx =  new Regex(@"[0-9]+.[0-9]+( )+[0-9]+.[0-9]+");
        private static Regex _doubleRegx = new Regex(@"[0-9]+(\.[0-9]+)|[0-9]+");


        public Task<DatasetObject> Parse(string stringFile, string extensionName, string fileName)
        {
            var dataset = new DatasetObject(extensionName, fileName);
            using (var csv = new CachedCsvReader(new StringReader(stringFile), true))
            {
                var headers = csv.GetFieldHeaders();
                while (csv.ReadNextRecord())
                {
                    var io = new IntermediateObject();
                    for (int i = 0; i < headers.Length; i++)
                        io.Attributes.Add(GetAttribute(headers[i], csv[i]));
                    dataset.Objects.Add(io);
                }
            }
            return Task.FromResult(dataset);
        }

        private ObjectAttribute GetAttribute(string name, string value)
        {
            if (value.Length == 0)
                return new NullAttribute(name);
            if (IsGeometricColumn(name))
                return CreateGeometricAttribute(name, value);
            long l;
            if (long.TryParse(value, out l))
                return new LongAttribute(name, l);
            double d;
            if (double.TryParse(value, out d))
                return new DoubleAttribute(name, d);
            DateTime dt;
            if (DateTime.TryParse(value, out dt))
                return new DateAttribute(name, dt);
            bool b;
            if (bool.TryParse(value, out b))
                return new BoolAttribute(name, b);

            return new TextAttribute(name, value);
        }

        private ObjectAttribute CreateGeometricAttribute(string name, string value)
        {
            if (value.Contains("Point", StringComparison.OrdinalIgnoreCase))
                return CreatePointAttribute(value, name);
            if (value.Contains("Multipolygon", StringComparison.OrdinalIgnoreCase))
                return CreateMultipolygonAttribute(value, name);
            if (value.Contains("POLYGON ", StringComparison.OrdinalIgnoreCase))
                return CreatePolygonAttribute(value, name);
            if (value.Contains("LineString", StringComparison.OrdinalIgnoreCase))
                return CreateLineString(value, name);
            throw new NotImplementedException("Unable to handle geometric type: " + value.Substring(0, Math.Min(value.Length, 16)));
        }

        private ObjectAttribute CreateLineString(string value, string name)
        {
            var points = new List<ObjectAttribute>();
            foreach (var pointMatch in _pointRegx.Matches(value).AsEnumerable())
            {
                points.Add(GeneratePoint(pointMatch.Value));
            }
            var polygon = new ListAttribute("LineString", points);
            if (name != null)
                polygon = new ListAttribute(name, new List<ObjectAttribute>() { polygon });
            return polygon;
        }

        private ObjectAttribute CreateMultipolygonAttribute(string value, string name)
        {
            var polygons = new List<ObjectAttribute>();
            var multipolygonString = _multipolygonRegx.Match(value).Value;
            foreach (var polygonMatch in _polygonRegx.Matches(multipolygonString).AsEnumerable())
            {
                var polygonString = polygonMatch.Value;
                ListAttribute polygon = CreatePolygonAttribute(polygonString);
                polygons.Add(polygon);
            }

            var multiPolygon = new ListAttribute("Multipolygon", polygons);
            var root = new ListAttribute(name, new List<ObjectAttribute>() { multiPolygon });
            return root;
        }

        private static ListAttribute CreatePolygonAttribute(string polygonString, string? name = null)
        {
            var points = new List<ObjectAttribute>();
            foreach (var pointMatch in _pointRegx.Matches(polygonString).AsEnumerable())
            {
                points.Add(GeneratePoint(pointMatch.Value));
            }
            var polygon = new ListAttribute("Polygon", points);
            if(name != null)
                polygon = new ListAttribute(name, new List<ObjectAttribute>() { polygon });
            return polygon;
        }

        private ObjectAttribute CreatePointAttribute(string value, string name)
        {
            var point = GeneratePoint(_pointRegx.Match(value).Value);
            var root = new ListAttribute(name, new List<ObjectAttribute>() { point });
            return root;
        }

        private static ListAttribute GeneratePoint(string pointString)
        {
            var doubleMatches = _doubleRegx.Matches(pointString);
            var doubles = new List<string>(doubleMatches.Count);
            for (int i = 0; i < doubleMatches.Count; i++)
            {
                var value = doubleMatches[i].Value;
                doubles.Add(value.Replace(',', '.'));
            }
            var latitude = new DoubleAttribute("Double", double.Parse(doubles[0], NumberStyles.Any, CultureInfo.InvariantCulture));
            var longitude = new DoubleAttribute("Double", double.Parse(doubles[1], NumberStyles.Any, CultureInfo.InvariantCulture));
            var point = new ListAttribute("Point", new List<ObjectAttribute>() { longitude, latitude });
            return point;
        }

        private static bool IsGeometricColumn(string name)
        {
            return name.Equals("wkb_geometry", StringComparison.OrdinalIgnoreCase) ||
                            name.Equals("Geometry_SPA", StringComparison.OrdinalIgnoreCase);
        }
    }
}
