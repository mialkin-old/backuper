using System;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace YandexDiskSingleFileBackup
{
    public class Backuper
    {
        private readonly EnvironmentVariables _env;
        private readonly MessagePrinter _printer;

        private static readonly HttpClient Client = new HttpClient
        {
            BaseAddress = new Uri("https://cloud-api.yandex.net/v1/disk/"),
        };

        public Backuper(EnvironmentVariables env, MessagePrinter printer)
        {
            _env = env;
            _printer = printer;

            Client.DefaultRequestHeaders.Add("Accept", "application/json");
            Client.DefaultRequestHeaders.Add("Authorization", $"OAuth {_env.OauthToken}");
        }

        public async Task Backup()
        {
            _printer.Print("Starting backup.");
            _printer.Print("Getting upload link.");
            string yandexDiskLinkForUpload = await GetYandexDiskLinkForUpload();
            _printer.Print("Uploading file.");
            await Upload(yandexDiskLinkForUpload);
            _printer.Print("Backup is finished.");
        }

        /// <summary>
        /// Запрашивает у Яндекс.Диска ссылку, по которой можно будет выгрузить файл.
        /// </summary>
        private async Task<string> GetYandexDiskLinkForUpload()
        {
            string uploadPath = Path.Combine("resources/upload?path=", _env.YandexDiskFolderPath,
                $"{DateTime.Now:yyyy-MM-dd HH-mm-ss} {_env.SourceFileName}");

            HttpResponseMessage response = await Client.GetAsync(uploadPath);
            response.EnsureSuccessStatusCode();

            string jsonString = await response.Content.ReadAsStringAsync();
            YandexDiskResponse? yandexDiskResponse = JsonSerializer.Deserialize<YandexDiskResponse>(jsonString);

            if (string.IsNullOrEmpty(yandexDiskResponse?.Link))
                throw new ArgumentNullException(nameof(yandexDiskResponse.Link));

            return yandexDiskResponse.Link;
        }

        /// <summary>
        /// Выгружает файл на Яндекс.Диск.
        /// </summary>
        private async Task Upload(string yandexDiskLinkForUpload)
        {
            string filePath = Path.Combine(_env.SourceFolderPath, _env.SourceFileName);
            byte[]? sourceFileBytes = await File.ReadAllBytesAsync(filePath);

            HttpResponseMessage uploadResponse =
                await Client.PutAsync(yandexDiskLinkForUpload, new ByteArrayContent(sourceFileBytes, 0, sourceFileBytes.Length));
            uploadResponse.EnsureSuccessStatusCode();
        }
    }
}