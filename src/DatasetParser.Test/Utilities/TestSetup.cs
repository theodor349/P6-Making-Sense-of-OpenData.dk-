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

        internal RouteFactory RouteFactory()
        {
            return new RouteFactory(); 
        }

        internal GenericFactory GenericFactoryRoute()
        {
            var description = new SpecializationDescription();
            description.GeoFeatureType = GeoFeatureType.LineString;
            description.Properties.Add(new SpecializationPropertyDescription("Name", "Name"));
            description.Properties.Add(new SpecializationPropertyDescription("Description", "Description"));
            return new GenericFactory(description);
        }
    }
}
