using System;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Slova.Backuper.Settings;

namespace Slova.Backuper.FileUploader
{
    public class FileUploader : IFileUploader
    {
        private readonly FileUploaderSettings _fileUploaderSettings;
        private readonly HttpClient _client;

        public FileUploader(HttpClient client, IOptions<FileUploaderSettings> options)
        {
            _client = client;
            _fileUploaderSettings = options.Value;

            _client.DefaultRequestHeaders.Add("Accept", "application/json");
            _client.DefaultRequestHeaders.Add("Authorization", $"OAuth {_fileUploaderSettings.OAuthToken}");
        }

        public async Task<string> GetUploadLinkAsync()
        {
            string uploadPath = "resources/upload?path=" + Path.Combine(_fileUploaderSettings.UploadDirectory,
                $"{DateTime.Now:yyyy-MM-dd HH-mm-ss} {_fileUploaderSettings.FileName}");

            HttpResponseMessage responseMessage = await _client.GetAsync(uploadPath);
            responseMessage.EnsureSuccessStatusCode();

            string jsonString = await responseMessage.Content.ReadAsStringAsync();
            Response? response = JsonSerializer.Deserialize<Response>(jsonString);

            if (string.IsNullOrEmpty(response?.Link))
                throw new ArgumentNullException(nameof(response.Link));

            return response.Link;
        }

        public async Task UploadFileAsync(string uploadLink, byte[] fileBytes)
        {
            if (string.IsNullOrEmpty(uploadLink))
                throw new ArgumentNullException(uploadLink);

            if (fileBytes.Length == 0)
                throw new ArgumentException("Upload file size in bytes must be greater than 0.");

            HttpResponseMessage uploadResponse =
                await _client.PutAsync(uploadLink, new ByteArrayContent(fileBytes, 0, fileBytes.Length));

            uploadResponse.EnsureSuccessStatusCode();
        }
    }
}