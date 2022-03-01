﻿using Microsoft.Extensions.DependencyInjection;
using Shared.ComponentInterfaces;

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