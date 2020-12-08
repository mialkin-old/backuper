using System;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using YandexDiskFileUploader.Interfaces;

namespace YandexDiskFileUploader.Implementations
{
    public class FileUploader : IFileUploader
    {
        private static readonly HttpClient Client = new HttpClient
        {
            BaseAddress = new Uri("https://cloud-api.yandex.net/v1/disk/")
        };

        public FileUploader(string oauthToken)
        {
            Client.DefaultRequestHeaders.Add("Accept", "application/json");
            Client.DefaultRequestHeaders.Add("Authorization", $"OAuth {oauthToken}");
        }

        public async Task<string> GetYandexDiskUploadLinkAsync(string yandexDiskDirectoryPath, string filename)
        {
            string uploadPath = Path.Combine("resources/upload?path=", yandexDiskDirectoryPath, filename);

            HttpResponseMessage responseMessage = await Client.GetAsync(uploadPath);
            responseMessage.EnsureSuccessStatusCode();

            string jsonString = await responseMessage.Content.ReadAsStringAsync();
            YandexDiskResponse? response = JsonSerializer.Deserialize<YandexDiskResponse>(jsonString);

            if (string.IsNullOrEmpty(response?.Link))
                throw new ArgumentNullException(nameof(response.Link));

            return response.Link;
        }

        public async Task UploadFileAsync(string uploadUri, byte[] fileBytes)
        {
            if(string.IsNullOrEmpty(uploadUri))
                throw new ArgumentNullException(uploadUri);
            
            if (fileBytes.Length == 0)
                throw new ArgumentException("Upload file size in bytes must be greater than 0.");
                    
            HttpResponseMessage uploadResponse =
                await Client.PutAsync(uploadUri, new ByteArrayContent(fileBytes, 0, fileBytes.Length));
            
            uploadResponse.EnsureSuccessStatusCode();
        }
    }
}