using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Slova.Backuper.FileReader;
using Slova.Backuper.FileUploader;
using Slova.Backuper.Settings;

namespace Slova.Backuper
{
    class Program
    {
        static async Task Main(string[] args)
        {
            using IHost host = CreateHostBuilder(args).Build();

            App app = host.Services.GetRequiredService<App>();
            await app.Run();

            await host.RunAsync();
        }

        static IHostBuilder CreateHostBuilder(string[] args) =>
            Host
                .CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((context, builder) =>
                {
                    builder.AddEnvironmentVariables(prefix: "SLOVA_BACKUPER_");
                })
                .ConfigureServices((context, serviceCollection) =>
                {
                    serviceCollection
                        .AddSingleton(new HttpClient { BaseAddress = new Uri("https://cloud-api.yandex.net/v1/disk/") })
                        .AddTransient<IFileReader, FileReader.FileReader>()
                        .AddTransient<IFileUploader, FileUploader.FileUploader>()
                        .AddTransient<App>();

                    serviceCollection.AddOptions<FileReaderSettings>().Bind(context.Configuration.GetSection(FileReaderSettings.FileReader))
                        .ValidateDataAnnotations();

                    serviceCollection.AddOptions<FileUploaderSettings>().Bind(context.Configuration.GetSection(FileUploaderSettings.FileUploader))
                        .ValidateDataAnnotations();
                });
    }
}