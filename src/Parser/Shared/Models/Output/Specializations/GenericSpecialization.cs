using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Models.Output.Specializations
{
    public class GenericSpecialization<T> : GeodataOutput<T> where T : GeoFeature
    {
        public List<SpecializationProperty> Properties { get; set; } = new();

        public GenericSpecialization(T geoFeature, List<SpecializationProperty> properties)
        {
            GeoFeatures = geoFeature;
            Properties = properties;
        }
    }

    public class SpecializationProperty
    {
        public string Name { get; set; }
        public object Value { get; set; }

        public SpecializationProperty(string name, object value)
        {
            Name = name;
            Value = value;
        }
    }

    public enum GeoFeatureType { Point, MultiPoint, LineString, MultiLineString, Polygon, MultiPolygon }
    public class SpecializationDescription
    {
        public GeoFeatureType GeoFeatureType { get; set; }
        public List<SpecializationPropertyDescription> Properties { get; set; } = new List<SpecializationPropertyDescription>();

        public List<string> GetLabels()
        {
            var res = Properties.ConvertAll(x => x.Target);
            res.Add(GeoFeatureType.ToString());
            return res;
        }
    }

    public class SpecializationPropertyDescription
    {
        public string Name { get; set; }
        public string Target { get; set; }

        public SpecializationPropertyDescription(string name, string target)
        {
            Name = name;
            Target = target;
        }
    }
}
