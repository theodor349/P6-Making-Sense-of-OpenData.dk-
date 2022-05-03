using Newtonsoft.Json.Linq;
using Shared.ComponentInterfaces;
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
            var properties = io.GetType().GetProperties().Where(x => !geoProperties.Contains(x.Name)).ToArray();

            foreach (var p in properties)
            {
                root.Add(new JProperty(p.Name, p.GetValue(io)));
            }

            return root;
        }

        private JObject GenerateGeometry(IntermediateOutput io)
        {
            if(io is ParkingSpot)
            {
                return GenerateMultiPolygon(((GeodataOutput<MultiPolygon>)io).GeoFeatures);
            }
            return new JObject();
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

        internal static JProperty ReturnPolygonProperty()
        {
            JArray coords = new JArray();
            coords.Add(new JArray(1.1, 1.2));
            coords.Add(new JArray(1.3, 1.4));
            coords.Add(new JArray(1.5, 1.6));
            coords.Add(new JArray(1.1, 1.2));
            JArray JArr = new JArray();
            JArr.Add(coords);
            JProperty JProp = new JProperty("coordinates", JArr);
            return JProp;
        }
    }
}
