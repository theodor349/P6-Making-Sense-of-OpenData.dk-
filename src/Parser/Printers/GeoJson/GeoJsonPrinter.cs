using Newtonsoft.Json.Linq;
using Shared.ComponentInterfaces;
using Shared.Models.ObjectAttributes;
using Shared.Models.Output;
using Shared.Models.Output.Specializations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Printers.GeoJson
{
    public interface IGeoJsonPrinter : IPrinter { }

    public class GeoJsonPrinter : IGeoJsonPrinter
    {
        private readonly IFilePrinter _filePrinter;

        public GeoJsonPrinter(IFilePrinter filePrinter)
        {
            _filePrinter = filePrinter;
        }

        public async Task Print(OutputDataset dataset, int iteration)
        {
            var features = GenerateFeatures(dataset);

            var root = new JObject();
            root.Add(new JProperty("type", "FeatureCollection"));
            root.Add(new JProperty("features", features));

            await _filePrinter.Print(dataset, root, iteration);
        }

        private JArray GenerateFeatures(OutputDataset dataset)
        {
            var features = new JArray();
            foreach (var io in dataset.Objects)
            {
                features.Add(GenerateFeature(io));
            }
            return features;
        }

        private JObject GenerateFeature(IntermediateOutput io)
        {
            var geometry = GenerateGeometry(io);
            var properties = GenerateProperties(io);

            var root = new JObject();
            root.Add(new JProperty("type", "Feature"));
            root.Add(new JProperty("geometry", geometry));
            root.Add(new JProperty("properties", properties));
            return root;
        }

        private JObject GenerateProperties(IntermediateOutput io)
        {
            var root = new JObject();
            
            var geoProperties = typeof(GeodataOutput<GeoFeature>).GetProperties().ToLookup(y => y.Name);
            var properties = io.Properties;

            foreach (var p in properties)
            {
                if (p.Value is TextAttribute t)
                    root.Add(new JProperty(p.Name, (string)t.Value));
                else
                    root.Add(new JProperty(p.Name, p.Value.ToString()));
            }

            return root;
        }

        private JObject GenerateGeometry(IntermediateOutput io)
        {
            if(io is GeodataOutput<MultiPolygon>)
            {
                return GenerateMultiPolygon(((GeodataOutput<MultiPolygon>)io).GeoFeatures);
            }
            else if (io is GeodataOutput<LineString>)
            {
                return GenerateLineString(((GeodataOutput<LineString>)io).GeoFeatures);
            }
            return new JObject();
        }

        private JObject GenerateLineString(LineString geoFeatures)
        {
            var root = new JObject();
            var lineString = GenerateLinearRing(geoFeatures.Coordinates);

            root.Add(new JProperty("type", "LineString"));
            root.Add("coordinates", lineString);
            return root;
        }

        private JObject GenerateMultiPolygon(MultiPolygon geoFeatures)
        {
            var polygons = new JArray();
            foreach (var polygon in geoFeatures.Polygons)
            {
                polygons.Add(GeneratePolygon(polygon));
            }

            var root = new JObject();
            root.Add(new JProperty("type", "MultiPolygon"));
            root.Add("coordinates", polygons);
            return root;
        }

        private JArray GeneratePolygon(Polygon polygon)
        {
            var root = new JArray();
            root.Add(GenerateLinearRing(polygon.Coordinates));
            return root;
        }

        private JArray GenerateLinearRing(List<Point> coordinates)
        {
            var root = new JArray();
            foreach (var coord in coordinates)
            {
                root.Add(GenerateCoordinate(coord));
            }
            return root;
        }

        private JArray GenerateCoordinate(Point coord)
        {
            var root = new JValue[2]
            {
                GenerateDoubleValue(coord.Longitude),
                GenerateDoubleValue(coord.Latitude),
            };
            return new JArray(root);
        }

        private JValue GenerateDoubleValue(double value)
        {
            return new JValue(value);
        }
    }
}
