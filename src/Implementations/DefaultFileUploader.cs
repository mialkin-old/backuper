using System;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using YandexDiskFileUploader.Interfaces;
using YandexDiskFileUploader.Utils;

namespace YandexDiskFileUploader.Implementations
{
    public class DefaultFileUploader : IYandexDiskFileUploader
    {
        private static readonly HttpClient Client = new HttpClient
        {
            BaseAddress = new Uri("https://cloud-api.yandex.net/v1/disk/")
        };

        public DefaultFileUploader(IOptions<YaDiskOptions> options)
        {
            Client.DefaultRequestHeaders.Add("Accept", "application/json");
            Client.DefaultRequestHeaders.Add("Authorization", $"OAuth {options.Value.YandexAppOauthToken}");
        }

        public async Task<string> GetUploadLinkAsync(string uploadDirectoryPath, string filename)
        {
            string uploadPath = Path.Combine("resources/upload?path=", uploadDirectoryPath, filename);

            HttpResponseMessage responseMessage = await Client.GetAsync(uploadPath);
            responseMessage.EnsureSuccessStatusCode();

            string jsonString = await responseMessage.Content.ReadAsStringAsync();
            YandexDiskResponse? response = JsonSerializer.Deserialize<YandexDiskResponse>(jsonString);

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