using DatasetGenerator.ParseFile;
using Microsoft.Extensions.DependencyInjection;
using Shared.ComponentInterfaces;

namespace DatasetGenerator
{
    public static class ConfigureServices 
    {
        public static void AddIntermediateGenerator(this IServiceCollection services)
        {
            services.AddTransient<IDatasetGenerator, DatasetGenerator>();
            services.AddTransient<IJsonParser, JsonParser>();
            services.AddTransient<ICsvParser, CsvParser>();
            services.AddTransient<IDatasetObjectSplitter, IntermediateDatasetSplitter>();
        }
    }
}
