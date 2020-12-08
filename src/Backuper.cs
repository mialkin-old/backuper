using System;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace YandexDiskFileUploader
{
    public class Backuper
    {
        private static readonly HttpClient Client = new HttpClient
        {
            BaseAddress = new Uri("https://cloud-api.yandex.net/v1/disk/")
        };

        public Backuper(string oauthToken)
        {
            Client.DefaultRequestHeaders.Add("Accept", "application/json");
            Client.DefaultRequestHeaders.Add("Authorization", $"OAuth {oauthToken}");
        }

        /// <summary>
        /// Запрашивает у Яндекс.Диска ссылку, по которой можно будет выгрузить файл.
        /// </summary>
        /// <param name="directoryPath">Полный путь до директории на Яндекс.Диске, в которую нужно загрузить файл.</param>
        /// <param name="filename">Название, которое будет у загруженного на Яндекс.Диск файла.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task<string> GetYandexDiskUploadLink(string directoryPath, string filename)
        {
            string uploadPath = Path.Combine("resources/upload?path=", directoryPath, filename);

            HttpResponseMessage responseMessage = await Client.GetAsync(uploadPath);
            responseMessage.EnsureSuccessStatusCode();

            string jsonString = await responseMessage.Content.ReadAsStringAsync();
            YandexDiskResponse? response = JsonSerializer.Deserialize<YandexDiskResponse>(jsonString);

            if (string.IsNullOrEmpty(response?.Link))
                throw new ArgumentNullException(nameof(response.Link));

            return response.Link;
        }

        /// <summary>
        /// Выгружает файл на Яндекс.Диск.
        /// </summary>
        /// <param name="uploadLink">Ссылка по которой нужно выгружать файл.</param>
        /// <param name="fileBytes">Файл для выгрузки.</param>
        /// <returns></returns>
        public async Task UploadFile(string uploadLink, byte[] fileBytes)
        {
            if(string.IsNullOrEmpty(uploadLink))
                throw new ArgumentNullException(uploadLink);
            
            if (fileBytes.Length == 0)
                throw new ArgumentException("Размер выгружаемого файла должен быть больше 0 байт.");
                    
            HttpResponseMessage uploadResponse =
                await Client.PutAsync(uploadLink, new ByteArrayContent(fileBytes, 0, fileBytes.Length));
            uploadResponse.EnsureSuccessStatusCode();
        }
    }
}