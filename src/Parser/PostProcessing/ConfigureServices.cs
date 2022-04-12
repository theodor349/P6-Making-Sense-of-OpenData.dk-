using Microsoft.Extensions.DependencyInjection;
using Shared.ComponentInterfaces;

namespace PostProcessing
{
    public static class ConfigureServices
    {
        public static void AddPostProcessing(this IServiceCollection services)
        {
            services.AddTransient<IPostProcessor, PostProcessor>();
        }
    }
}
