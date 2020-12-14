using System;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Slova.Backuper.Settings;

namespace Slova.Backuper.FileUploader
{
    public class FileUploader : IFileUploader
    {
        private readonly FileUploaderSettings _fileUploaderSettings;
        private readonly HttpClient _client;
        private readonly ILogger<FileUploader> _logger;

        public FileUploader(HttpClient client, IOptions<FileUploaderSettings> options, ILogger<FileUploader> logger)
        {
            _client = client;
            _fileUploaderSettings = options.Value;
            _logger = logger;

            _client.DefaultRequestHeaders.Add("Accept", "application/json");
            _client.DefaultRequestHeaders.Add("Authorization", $"OAuth {_fileUploaderSettings.OAuthToken}");
        }

        public async Task<string> GetUploadLinkAsync()
        {
            _logger.LogInformation("Start getting upload link.");
            string uploadPath = "resources/upload?path=" + Path.Combine(_fileUploaderSettings.UploadDirectory,
                $"{DateTime.Now:yyyy-MM-dd HH-mm-ss} {_fileUploaderSettings.FileName}");

            _logger.LogInformation("Upload link path: {0}", uploadPath);
            HttpResponseMessage responseMessage = await _client.GetAsync(uploadPath);
            _logger.LogInformation("Received Yandex.Disk server response: {0}", responseMessage);
            responseMessage.EnsureSuccessStatusCode();
            _logger.LogInformation("Start deserializing response.");
            string jsonString = await responseMessage.Content.ReadAsStringAsync();
            Response? response = JsonSerializer.Deserialize<Response>(jsonString);
            _logger.LogInformation("Response successfully deserialized: {0}", jsonString);

            if (string.IsNullOrEmpty(response?.Link))
                throw new ArgumentNullException(nameof(response.Link));

            _logger.LogInformation("Upload link has been successfully received: {0}", response.Link);
            return response.Link;
        }

        public async Task UploadFileAsync(string uploadLink, byte[] fileBytes)
        {
            _logger.LogInformation("Prepare uploading file with upload link: {0}.", uploadLink);

            if (string.IsNullOrEmpty(uploadLink))
                throw new ArgumentNullException(uploadLink);

            _logger.LogInformation($"Upload file size is {fileBytes.Length} bytes.");

            if (fileBytes.Length == 0)
                throw new ArgumentException("Upload file size in bytes must be greater than 0.");

            _logger.LogInformation("Start uploading file.");
            HttpResponseMessage uploadResponse =
                await _client.PutAsync(uploadLink, new ByteArrayContent(fileBytes, 0, fileBytes.Length));
            _logger.LogInformation("Finish uploading file.");
            uploadResponse.EnsureSuccessStatusCode();

            _logger.LogInformation("File has been successfully uploaded.");
        }
    }
}