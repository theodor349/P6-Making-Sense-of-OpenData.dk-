using Microsoft.Extensions.DependencyInjection;

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
