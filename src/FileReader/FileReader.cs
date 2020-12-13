using System;
using System.IO;
using System.Threading.Tasks;
using YandexDiskFileUploader.Interfaces;

namespace YandexDiskFileUploader.Implementations
{
    public class FileReader : IFileReader
    {
        public async Task<byte[]> ReadFileAsync(string filePath)
        {
            Print("Start backup.");
            Print("Start reading file from disk.");
            byte[] fileBytes = await File.ReadAllBytesAsync(filePath);
            
            if (fileBytes.Length == 0)
                throw new Exception("File size is null.");
            
            Print($"File has been read. File size: {fileBytes.Length / 1024} KB.");

            return fileBytes;
        }

        private static void Print(string message)
        {
            Console.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff} — {message}");
        }
    }
}