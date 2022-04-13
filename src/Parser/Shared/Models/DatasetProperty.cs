using System;
namespace Shared.Models
{
	public struct DatasetProperty
	{
		public string name;
		public string value;

        public DatasetProperty(string name, string value)
        {
            this.name = name;
            this.value = value;
        }
    }
}

