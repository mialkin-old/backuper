using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using YandexDiskFileUploader.FileReader;

namespace YandexDiskFileUploader
{
    class Program
    {
        public string OperationId { get; } = Guid.NewGuid().ToString()[^4..];
        static async Task Main(string[] args)
        {
            using IHost host = CreateHostBuilder(args).Build();
            
            var env = new EnvironmentVariables();
            var backuper = new Backuper(env.OauthToken);
            
            IFileReader fl = new FileReader.FileReader();
            
            byte[] fileBytes = await fl.ReadFileAsync(Path.Combine(env.SourceFolderPath, env.SourceFileName));

            Print("Start getting upload link.");
            string uploadLink = await backuper.GetYandexDiskUploadLink(env.YandexDiskFolderPath, 
                $"{DateTime.Now:yyyy-MM-dd HH-mm-ss} {env.SourceFileName}");
            Print("Upload link has been received.");

            Print("Start uploading file.");
            await backuper.UploadFile(uploadLink, fileBytes);
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