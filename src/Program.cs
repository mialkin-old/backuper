using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using YandexDiskFileUploader.Implementations;
using YandexDiskFileUploader.Interfaces;
using YandexDiskFileUploader.Utils;

namespace YandexDiskFileUploader
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // TODO make YaDiskOptions properties notnull with attributes
            using IHost host = CreateHostBuilder(args).Build();
            // using IServiceScope serviceScope = host.Services.CreateScope();
            // IServiceProvider provider = serviceScope.ServiceProvider;
            // Backuper backuper = provider.GetRequiredService<Backuper>();

            Backuper backuper = host.Services.GetRequiredService<Backuper>();
            await backuper.StartBackup();

            // EnvironmentVariables env = new();
            //
            // IFileReader fileReader = new DefaultFileReader();
            //
            // byte[] fileBytes = await fileReader.ReadFileAsync(Path.Combine(env.SourceFileDirectoryPath, env.SourceFileName));
            //
            // IYandexDiskFileUploader yandexDiskFileUploader = new DefaultFileUploader(env.YandexAppOauthToken);
            //
            // Print("Start getting upload link.");
            // string uploadLink = await yandexDiskFileUploader.GetUploadLinkAsync(env.UploadDirectoryPath,
            //     $"{DateTime.Now:yyyy-MM-dd HH-mm-ss} {env.SourceFileName}");
            // Print("Upload link has been received.");
            //
            // Print("Start uploading file.");
            // await yandexDiskFileUploader.UploadFileAsync(uploadLink, fileBytes);
            // Print("File has been uploaded.");
            // Print("Backup is finished.");

            await host.RunAsync();
        }

        static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((context, serviceCollection) =>
                {
                    serviceCollection
                        .AddTransient<IFileReader, DefaultFileReader>()
                        .AddTransient<IYandexDiskFileUploader, DefaultFileUploader>()
                        .AddTransient<Backuper>();
                });

        private static void Print(string message)
        {
            Console.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff} — {message}");
        }
    }
}