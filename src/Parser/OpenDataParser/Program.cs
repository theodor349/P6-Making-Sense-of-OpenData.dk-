using DatasetDecider;
using DatasetParser;
using DatasetGenerator;
using LabelRecognizer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenDataParser;
using Serilog;
using PostProcessing;
using Printers;

var builder = new ConfigurationBuilder();

Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(builder.Build())
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .CreateLogger();

Log.Logger.Information("Application Starting");

var host = Host.CreateDefaultBuilder()
    .ConfigureServices((context, services) =>
    {
        services.AddTransient<IDataParser, DataParser>();
        services.AddIntermediateGenerator();
        services.AddDatasetClassifier();
        services.AddDatasetParser();
        services.AddLabelRecognizer();
        services.AddPostProcessing();
        services.AddPrinters();
    })
    .UseSerilog()
    .Build();

var svc = ActivatorUtilities.CreateInstance<DataParser>(host.Services);
await svc.Run();
