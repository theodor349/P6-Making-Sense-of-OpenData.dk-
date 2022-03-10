using LabelRecognizer.Helpers;
using Microsoft.Extensions.DependencyInjection;
using Shared.ComponentInterfaces;

namespace LabelRecognizer
{
    public static class ConfigureServices
    {
        public static void AddLabelRecognizer(this IServiceCollection services)
        {
            services.AddTransient<ILabelGenerator, LabelGenerator>();
            services.AddTransient<ITypeLabeler, TypeLabeler>();
            services.AddTransient<IGeoLabeler, GeoLabeler>();
        }
    }
}
