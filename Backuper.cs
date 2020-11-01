using System;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace DictionaryBackup
{
    public class Backuper
    {
        private readonly EnvironmentVariables _env;
        private readonly MessagePrinter _printer;

        private static readonly HttpClient Client = new HttpClient
        {
            BaseAddress = new Uri("https://cloud-api.yandex.net/v1/disk/")
        };

        public Backuper(EnvironmentVariables env, MessagePrinter printer)
        {
            _env = env;
            _printer = printer;
        }

        public async Task Backup()
        {
            _printer.Print("Backup started.");

            string todayDate = DateTime.Now.ToString("yyyy-MM-dd");

            Client.DefaultRequestHeaders.Clear();
            Client.DefaultRequestHeaders.Add("Accept", "application/json");
            Client.DefaultRequestHeaders.Add("Authorization", $"OAuth {_env.OauthToken}");

            string uploadPath = GetUploadPath();

            HttpResponseMessage response = await Client.GetAsync(uploadPath).ConfigureAwait(false);

            response.EnsureSuccessStatusCode();

            string jsonString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            YandexDiskResponse? yandexDiskResponse = JsonSerializer.Deserialize<YandexDiskResponse>(jsonString);

            if (string.IsNullOrEmpty(yandexDiskResponse?.Link))
                throw new ArgumentNullException(nameof(yandexDiskResponse.Link));

            byte[]? byteArray = await File.ReadAllBytesAsync("dictionary.db").ConfigureAwait(false);

            HttpResponseMessage uploadResponse = await Client.PutAsync(yandexDiskResponse.Link, new ByteArrayContent(byteArray, 0, byteArray.Length))
                .ConfigureAwait(false);
            uploadResponse.EnsureSuccessStatusCode();

            _printer.Print("Backup finished.");
        }

        public string GetUploadPath()
        {
            return Path.Combine("resources/upload?path=", _env.DestinationFolderPath,
                $"{DateTime.Now:yyyy-MM-dd}-{_env.DestinationFileName}");
        }
    }
}