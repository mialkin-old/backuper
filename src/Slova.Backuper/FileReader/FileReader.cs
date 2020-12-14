using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Slova.Backuper.Settings;

namespace Slova.Backuper.FileReader
{
    public class FileReader : IFileReader
    {
        private readonly ILogger<FileReader> _logger;
        private readonly FileReaderSettings _fileReaderSettings;

        public FileReader(IOptions<FileReaderSettings> fileReaderSettings, ILogger<FileReader> logger)
        {
            _fileReaderSettings = fileReaderSettings.Value;
            _logger = logger;
        }

        public async Task<byte[]> ReadFileAsync()
        {
            _logger.LogInformation("Start reading file from disk");
            string path = Path.Combine(_fileReaderSettings.FileDirectory, _fileReaderSettings.FileName);
            _logger.LogInformation("File path is {0}", path);

            byte[] fileBytes = await File.ReadAllBytesAsync(path);
            _logger.LogInformation($"File has been read. File size is {fileBytes.Length / 1024} KB ({fileBytes.Length} bytes).");

            return fileBytes;
        }
    }
}