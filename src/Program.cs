using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using YandexDiskFileUploader.Implementations;
using YandexDiskFileUploader.Interfaces;
using YandexDiskFileUploader.Options;

namespace YandexDiskFileUploader
{
    class Program
    {
        static async Task Main(string[] args)
        {
            using IHost host = CreateHostBuilder(args).Build();

            BackupRunner backupRunner = host.Services.GetRequiredService<BackupRunner>();
            await backupRunner.Run();

            await host.RunAsync();
        }

        static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                        .AddJsonFile($"appsettings.Development.json", 
                            optional: true, reloadOnChange: true);

                })
                .ConfigureServices((context, serviceCollection) =>
                {
                    serviceCollection
                        .AddTransient<IFileReader, DefaultFileReader>()
                        .AddTransient<IYandexDiskFileUploader, DefaultFileUploader>()
                        .AddTransient<BackupRunner>();

                    IConfigurationSection section = context.Configuration.GetSection("BackupRunner");

                    serviceCollection.Configure<BackupRunnerOptions>(section);
                });
    }
}