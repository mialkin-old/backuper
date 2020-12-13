using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using YandexDiskFileUploader.Interfaces;
using YandexDiskFileUploader.Options;
using YandexDiskFileUploader.Settings;

namespace YandexDiskFileUploader
{
    public class App
    {
        private readonly IFileReader _fileReader;
        private readonly IFileUploader _fileUploader;
        private readonly FileReaderSettings _fileReaderSettings;

        public App(IFileReader fileReader, IFileUploader fileUploader, IOptions<FileReaderSettings> options)
        {
            _fileReader = fileReader;
            _fileUploader = fileUploader;
            _fileReaderSettings = options.Value;
        }

        public async Task Run()
        {
            byte[] fileBytes = await _fileReader.ReadFileAsync(Path.Combine(_fileReaderSettings.FileDirectory, _fileReaderSettings.FileName));
            
            Print("Start getting upload link.");
            string uploadLink = await _fileUploader.GetUploadLinkAsync($"{DateTime.Now:yyyy-MM-dd HH-mm-ss} {_fileReaderSettings.FileName}");
            Print("Upload link has been received.");
            
            Print("Start uploading file.");
            await _fileUploader.UploadFileAsync(uploadLink, fileBytes);
            Print("File has been uploaded.");
            Print("Backup is finished.");
        }
        
        private static void Print(string message)
        {
            Console.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff} — {message}");
        }
    }
}