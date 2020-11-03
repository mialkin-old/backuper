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
            
            Print("Start backup.");
            Print("Start reading file from disk.");
            string filePath = Path.Combine(env.SourceFolderPath, env.SourceFileName);
            byte[]? fileBytes = await File.ReadAllBytesAsync(filePath);
            Print($"File has been read. File size: {fileBytes.Length / 1024} KB.");

            Print("Start getting upload link.");
            string uploadLink = await backuper.GetYandexDiskUploadLink(env.YandexDiskFolderPath, 
                $"{DateTime.Now:yyyy-MM-dd HH-mm-ss} {env.SourceFileName}");
            Print("Upload link has been received.");

            Print("Start uploading file.");
            await backuper.UploadFile(uploadLink, fileBytes);
            Print("File has been uploaded.");
            Print("Backup is finished.");
        }

        private static void Print(string message)
        {
            Console.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff} — {message}");
        }
    }
}