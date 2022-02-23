using Microsoft.Extensions.DependencyInjection;

namespace LabelRecognizer
{
    public static class ConfigureServices
    {
        public static void AddLabelRecognizer(this IServiceCollection services)
        {
            services.AddTransient<ILabelGenerator, LabelGenerator>();
        }
    }
}
