using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using YandexDiskFileUploader.Interfaces;
using YandexDiskFileUploader.Options;

namespace YandexDiskFileUploader
{
    public class BackupRunner
    {
        private readonly IFileReader _fileReader;
        private readonly IYandexDiskFileUploader _fileUploader;
        private readonly BackupRunnerOptions _options;

        public BackupRunner(IFileReader fileReader, IYandexDiskFileUploader fileUploader, IOptions<BackupRunnerOptions> options)
        {
            _fileReader = fileReader;
            _fileUploader = fileUploader;
            _options = options.Value;
        }

        public async Task Run()
        {
            byte[] fileBytes = await _fileReader.ReadFileAsync(Path.Combine(_options.SourceFileDirectoryPath, _options.SourceFileName));
            
            Print("Start getting upload link.");
            string uploadLink = await _fileUploader.GetUploadLinkAsync(_options.UploadDirectoryPath,
                $"{DateTime.Now:yyyy-MM-dd HH-mm-ss} {_options.SourceFileName}");
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