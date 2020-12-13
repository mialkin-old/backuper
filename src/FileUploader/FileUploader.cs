using System;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using YandexDiskFileUploader.Interfaces;
using YandexDiskFileUploader.Options;
using YandexDiskFileUploader.Utils;

namespace YandexDiskFileUploader.Implementations
{
    public class FileUploader : IFileUploader
    {
        private readonly FileUploaderSettings _fileUploaderSettings;

        private static readonly HttpClient Client = new()
        {
            BaseAddress = new Uri("https://cloud-api.yandex.net/v1/disk/")
        };

        public FileUploader(IOptions<FileUploaderSettings> options)
        {
            _fileUploaderSettings = options.Value;
            
            Client.DefaultRequestHeaders.Add("Accept", "application/json");
            Client.DefaultRequestHeaders.Add("Authorization", $"OAuth {_fileUploaderSettings.OAuthToken}");
        }

        public async Task<string> GetUploadLinkAsync(string filename)
        {
            string uploadPath = Path.Combine("resources/upload?path=", _fileUploaderSettings.UploadDirectory, filename);

            HttpResponseMessage responseMessage = await Client.GetAsync(uploadPath);
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
                await Client.PutAsync(uploadLink, new ByteArrayContent(fileBytes, 0, fileBytes.Length));

            uploadResponse.EnsureSuccessStatusCode();
        }
    }
}