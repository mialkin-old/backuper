using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using YandexDiskFileUploader.Implementations;
using YandexDiskFileUploader.Interfaces;

namespace YandexDiskFileUploader
{
    class Program
    {
        public string OperationId { get; } = Guid.NewGuid().ToString()[^4..];

        static async Task Main(string[] args)
        {
            using IHost host = CreateHostBuilder(args).Build();

            EnvironmentVariables env = new();

            IFileReader fileReader = new FileReader();

            byte[] fileBytes = await fileReader.ReadFileAsync(Path.Combine(env.SourceFolderPath, env.SourceFileName));

            IYandexDiskFileUploader yandexDiskFileUploader = new DefaultFileUploader(env.OauthToken);

            Print("Start getting upload link.");
            string uploadLink = await yandexDiskFileUploader.GetUploadLinkAsync(env.UploadDirectoryPath,
                $"{DateTime.Now:yyyy-MM-dd HH-mm-ss} {env.SourceFileName}");
            Print("Upload link has been received.");

            Print("Start uploading file.");
            await yandexDiskFileUploader.UploadFileAsync(uploadLink, fileBytes);
            Print("File has been uploaded.");
            Print("Backup is finished.");

            await host.RunAsync();
        }

        static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args);

        private static void Print(string message)
        {
            Console.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff} — {message}");
        }
    }
}