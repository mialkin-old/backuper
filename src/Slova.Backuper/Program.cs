using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Formatting.Json;
using Slova.Backuper.FileReader;
using Slova.Backuper.FileUploader;
using Slova.Backuper.Settings;

namespace Slova.Backuper
{
    class Program
    {
        static async Task Main(string[] args)
        {
            ConfigurationBuilder builder = new();
            BuildConfig(builder);

            IConfigurationRoot configurationRoot = builder.Build();

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configurationRoot)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .WriteTo.File(new JsonFormatter(), configurationRoot.GetValue<string>("LogFile"), rollingInterval: RollingInterval.Month)
                .CreateLogger();

            Log.Logger.Information("Application is starting.");

            IHost host = Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {
                    services
                        .AddSingleton(new HttpClient { BaseAddress = new Uri(configurationRoot.GetValue<string>("YandexDiskApi")) })
                        .AddTransient<IFileReader, FileReader.FileReader>()
                        .AddTransient<IFileUploader, FileUploader.FileUploader>()
                        .AddTransient<App>();

                    services.AddOptions<FileReaderSettings>().Bind(configurationRoot.GetSection(FileReaderSettings.FileReader))
                        .ValidateDataAnnotations();

                    services.AddOptions<FileUploaderSettings>().Bind(configurationRoot.GetSection(FileUploaderSettings.FileUploader))
                        .ValidateDataAnnotations();
                })
                .UseSerilog()
                .Build();

            App app = ActivatorUtilities.CreateInstance<App>(host.Services);
            await app.Run();

            Log.Logger.Information("Application is terminating.");
        }

        static void BuildConfig(IConfigurationBuilder builder)
        {
            builder.SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT") ?? "Production"}.json", optional: true,
                    reloadOnChange: true)
                .AddEnvironmentVariables("SLOVA_BACKUPER_");
        }
    }
}