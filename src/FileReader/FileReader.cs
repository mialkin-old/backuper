using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using YandexDiskFileUploader.Settings;

namespace YandexDiskFileUploader.FileReader
{
    public class FileReader : IFileReader
    {
        private readonly FileReaderSettings _fileReaderSettings;

        public FileReader(IOptions<FileReaderSettings> fileReaderSettings)
        {
            _fileReaderSettings = fileReaderSettings.Value;
        }
        public async Task<byte[]> ReadFileAsync()
        {
            Print("Start backup.");
            Print("Start reading file from disk.");
            byte[] fileBytes = await File.ReadAllBytesAsync(Path.Combine(_fileReaderSettings.FileDirectory, _fileReaderSettings.FileName));
            
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