using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Slova.Backuper.FileReader;
using Slova.Backuper.FileUploader;
using Slova.Backuper.Settings;

namespace Slova.Backuper
{
    class Program
    {
        static async Task Main(string[] args)
        {
            ServiceCollection services = new();
            ConfigureServices(services);

            ServiceProvider serviceProvider = services.BuildServiceProvider();
            await serviceProvider.GetService<App>()!.Run();
        }
        
        private static void ConfigureServices(ServiceCollection services)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .AddEnvironmentVariables("SLOVA_BACKUPER_")
                .Build();

            services.AddLogging(x => x.AddConsole());

            services
                .AddSingleton(new HttpClient { BaseAddress = new Uri("https://cloud-api.yandex.net/v1/disk/") })
                .AddTransient<IFileReader, FileReader.FileReader>()
                .AddTransient<IFileUploader, FileUploader.FileUploader>()
                .AddTransient<App>();

            services.AddOptions<FileReaderSettings>().Bind(configuration.GetSection(FileReaderSettings.FileReader))
                .ValidateDataAnnotations();

            services.AddOptions<FileUploaderSettings>().Bind(configuration.GetSection(FileUploaderSettings.FileUploader))
                .ValidateDataAnnotations();
        }
    }
}