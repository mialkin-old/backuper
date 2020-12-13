using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using YandexDiskFileUploader.Implementations;
using YandexDiskFileUploader.Interfaces;
using YandexDiskFileUploader.Options;
using YandexDiskFileUploader.Settings;

namespace YandexDiskFileUploader
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
                .ConfigureServices((context, serviceCollection) =>
                {
                    serviceCollection
                        .AddTransient<IFileReader, FileReader>()
                        .AddTransient<IFileUploader, FileUploader>()
                        .AddTransient<App>();

                    //IConfigurationSection section = context.Configuration.GetSection("OAuthToken");

                    serviceCollection.Configure<FileReaderSettings>(context.Configuration.GetSection(FileReaderSettings.App));
                    serviceCollection.Configure<FileUploaderSettings>(context.Configuration.GetSection("Slova"));
                });
    }
}