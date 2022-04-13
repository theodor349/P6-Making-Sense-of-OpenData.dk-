using Microsoft.Extensions.DependencyInjection;
using PostProcessing.Helpers;
using Shared.ComponentInterfaces;

namespace PostProcessing
{
    public static class ConfigureServices
    {
        public static void AddPostProcessing(this IServiceCollection services)
        {
            services.AddTransient<IPostProcessor, PostProcessor>();
            services.AddTransient<IGeoMetaData, GeoMetaData>();
        }
    }
}
