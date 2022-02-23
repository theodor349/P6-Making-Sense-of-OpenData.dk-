using Microsoft.Extensions.DependencyInjection;

namespace IntermediateGenerator
{
    public static class ConfigureServices 
    {
        public static void AddIntermediateGenerator(this IServiceCollection services)
        {
            services.AddTransient<IIntermediateGenerator, IntermediateGenerator>();
        }
    }
}
