using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Formatting.Json;

namespace Slova.Backuper
{
    class Program
    {
        static async Task Main(string[] args)
        {
            ConfigurationBuilder builder = new();
            BuildConfig(builder);

            IConfigurationRoot configurationRoot = builder.Build();

            Log.Logger = SetupLogger(configurationRoot);

            SetupUnhandledExceptionsHandler(Log.Logger);

            Log.Logger.Information("Application is starting.");

            IHost host = Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) => { services.AddCustomServices(configurationRoot); })
                .UseSerilog()
                .Build();

            IApp app = ActivatorUtilities.CreateInstance<App>(host.Services);
            await app.Run();

            Log.Logger.Information("Application is stopping.");
        }

        private static void SetupUnhandledExceptionsHandler(ILogger logger)
        {
            AppDomain.CurrentDomain.UnhandledException += (sender, eventArgs) =>
            {
                logger.Error("Application is terminating because of the unhandled exception: {exception}", eventArgs.ExceptionObject);
                Environment.Exit(1);
            };
        }

        private static ILogger SetupLogger(IConfigurationRoot configurationRoot)
        {
            return new LoggerConfiguration()
                .ReadFrom.Configuration(configurationRoot)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .WriteTo.File(new JsonFormatter(), configurationRoot.GetValue<string>("LogFile"), rollingInterval: RollingInterval.Month)
                .CreateLogger();
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