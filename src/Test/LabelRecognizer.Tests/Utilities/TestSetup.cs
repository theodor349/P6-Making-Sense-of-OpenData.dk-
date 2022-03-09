using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LabelRecognizer;
using LabelRecognizer.Helpers;
using NSubstitute;

namespace LabelRecognizer.Tests.Utilities
{
    internal class TestSetup
    {
        public TestSetup()
        {

        }
        public LabelGenerator LabelGenerator()
        {
            ITypeLabeler typeLabeler = new TypeLabeler();
            return new LabelGenerator(typeLabeler);
        }
    }
}
