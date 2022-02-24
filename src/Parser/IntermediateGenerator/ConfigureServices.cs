using IntermediateGenerator.ParseFile;
using Microsoft.Extensions.DependencyInjection;
using Shared.ComponentInterfaces;

namespace IntermediateGenerator
{
    public static class ConfigureServices 
    {
        public static void AddIntermediateGenerator(this IServiceCollection services)
        {
            services.AddTransient<IIntermediateGenerator, IntermediateGenerator>();
            services.AddTransient<IParseJson, ParseJson>();
        }
    }
}
