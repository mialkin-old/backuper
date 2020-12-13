using System;
using System.Threading.Tasks;
using Slova.Backuper.FileReader;
using Slova.Backuper.FileUploader;

namespace Slova.Backuper
{
    public class App
    {
        private readonly IFileReader _fileReader;
        private readonly IFileUploader _fileUploader;
        
        public App(IFileReader fileReader, IFileUploader fileUploader)
        {
            _fileReader = fileReader;
            _fileUploader = fileUploader;
        }

        public async Task Run()
        {
            byte[] fileBytes = await _fileReader.ReadFileAsync();
            
            Print("Start getting upload link.");
            string uploadLink = await _fileUploader.GetUploadLinkAsync();
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