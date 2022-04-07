using LumenWorks.Framework.IO.Csv;
using Shared.Models;
using Shared.Models.ObjectAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;


namespace DatasetGenerator.ParseFile
{
    public interface ICsvParser : IFileParser
    {
    }

    internal class CsvParser : ICsvParser
    {
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
                return CreatePointAttribute(name, value);
            return null;
        }

        private ObjectAttribute CreatePointAttribute(string name, string value)
        {
            var pointsRegx = new Regex("[0-9]+.[0-9]+( )+[0-9]+.[0-9]+");
            var points = pointsRegx.Match(value).Value.Split(' ');
            var longitude = new DoubleAttribute("Double", double.Parse(points[0]));
            var latitude = new DoubleAttribute("Double", double.Parse(points[1]));
            var point = new ListAttribute("Point", new List<ObjectAttribute>() { longitude, latitude });
            var root = new ListAttribute(name, new List<ObjectAttribute>() { point });
            return root;
        }

        private static bool IsGeometricColumn(string name)
        {
            return name.Equals("wkb_geometry", StringComparison.OrdinalIgnoreCase) ||
                            name.Equals("", StringComparison.OrdinalIgnoreCase);
        }
    }
}
