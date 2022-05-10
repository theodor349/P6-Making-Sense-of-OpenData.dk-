using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Models.Output.Specializations
{
    public class GenericSpecialization<T> : GeodataOutput<T> where T : GeoFeature
    {
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
    }

    public class SpecializationPropertyDescription
    {
        public string Name { get; set; }
        public List<string> Targets { get; set; }

        public SpecializationPropertyDescription(string name, List<string> target)
        {
            Name = name;
            Targets = target;
        }
    }
}
