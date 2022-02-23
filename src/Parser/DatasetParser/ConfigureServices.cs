﻿using Microsoft.Extensions.DependencyInjection;
using Shared.ComponentInterfaces;

namespace DatasetParser
{
    public static class ConfigureServices
    {
        public static void AddDatasetParser(this IServiceCollection services)
        {
            services.AddTransient<IDatasetParser, DatasetParser>();
        }
    }
}
