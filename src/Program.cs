using System;
using System.IO;
using System.Threading.Tasks;

namespace Backuper
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var env = new EnvironmentVariables();
            var backuper = new Backuper(env.OauthToken);
            
            Print("Reading file from disk.");
            string filePath = Path.Combine(env.SourceFolderPath, env.SourceFileName);
            byte[]? fileBytes = await File.ReadAllBytesAsync(filePath);
            Print($"File size is {fileBytes.Length / 1024} KB.");

            Print("Getting upload link.");
            string uploadLink = await backuper.GetYandexDiskUploadLink(env.YandexDiskFolderPath, 
                $"{DateTime.Now:yyyy-MM-dd HH-mm-ss} {env.SourceFileName}");
            
            Print("Uploading file.");
            await backuper.UploadFile(uploadLink, fileBytes);
            Print("Finished.");
        }

        private static void Print(string message)
        {
            Console.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff} — {message}");
        }
    }
}