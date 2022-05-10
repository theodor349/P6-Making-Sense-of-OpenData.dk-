using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DatasetParser;
using DatasetParser.Factories;
using Shared.Models.Output.Specializations;

namespace DatasetParser.Test.Utilities
{
    internal class TestSetup
    {
        public TestSetup()
        {

        }

        internal GenericFactory GenericFactoryRoute()
        {
            var description = new SpecializationDescription();
            description.GeoFeatureType = GeoFeatureType.LineString;
            description.Properties.Add(new SpecializationPropertyDescription("Name", new List<string>() { "Name", "Navn" }));
            description.Properties.Add(new SpecializationPropertyDescription("Description", new List<string>() { "Description", "Beskrivelse" }));
            return new GenericFactory(description);
        }

        internal GenericFactory GenericFactoryParking()
        {
            var description = new SpecializationDescription();
            description.GeoFeatureType = GeoFeatureType.Polygon;
            description.Properties.Add(new SpecializationPropertyDescription("Name", new List<string>() { "Name", "Navn" }));
            description.Properties.Add(new SpecializationPropertyDescription("Description", new List<string>() { "Description", "Beskrivelse" }));
            return new GenericFactory(description);
        }
    }
}
