using Microsoft.Extensions.DependencyInjection;
using Shared.ComponentInterfaces;

namespace DatasetDecider
{
    public static class ConfigureServices
    {
        public static void AddDatasetClassifier(this IServiceCollection services)
        {
            services.AddTransient<IDatasetClassifier, DatasetClassifier>();
        }
    }
}
