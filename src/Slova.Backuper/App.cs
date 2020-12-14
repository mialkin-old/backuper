using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Slova.Backuper.FileReader;
using Slova.Backuper.FileUploader;

namespace Slova.Backuper
{
    public class App
    {
        private readonly IFileReader _fileReader;
        private readonly IFileUploader _fileUploader;
        private readonly ILogger<App> _logger;

        public App(IFileReader fileReader, IFileUploader fileUploader, ILogger<App> logger)
        {
            _fileReader = fileReader;
            _fileUploader = fileUploader;
            _logger = logger;
        }

        public async Task Run()
        {
            _logger.LogInformation("Start backup. 🚀");

            byte[] fileBytes = await _fileReader.ReadFileAsync();
            string uploadLink = await _fileUploader.GetUploadLinkAsync();
            await _fileUploader.UploadFileAsync(uploadLink, fileBytes);
            
            _logger.LogInformation("Backup is finished. 🏁");
        }
    }
}