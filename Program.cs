using System;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace DictionaryBackup
{
    class Program
    {
        private static string? _oAuthToken;

        private static readonly HttpClient Client = new HttpClient
        {
            BaseAddress = new Uri("https://cloud-api.yandex.net/v1/disk/")
        };

        static void Main(string[] args)
        {
            _oAuthToken = Environment.GetEnvironmentVariable("DICT_BACKUP_OAUTH_TOKEN");
            if (string.IsNullOrEmpty(_oAuthToken))
                throw new ArgumentNullException(nameof(_oAuthToken));

            PrintMessage("Backup started.");
            Backup().GetAwaiter().GetResult();
        }

        static async Task Backup()
        {
            string todayDate = DateTime.Now.ToString("yyyy-MM-dd");

            Client.DefaultRequestHeaders.Clear();
            Client.DefaultRequestHeaders.Add("Accept", "application/json");
            Client.DefaultRequestHeaders.Add("Authorization", $"OAuth {_oAuthToken}");

            HttpResponseMessage response = await Client
                .GetAsync($"resources/upload?path=Backups%2Fdict.mialkin.ru%2F{todayDate}-dictionary.db")
                .ConfigureAwait(false);

            response.EnsureSuccessStatusCode();

            string jsonString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            YandexDiskResponse? yandexDiskResponse = JsonSerializer.Deserialize<YandexDiskResponse>(jsonString);

            if (string.IsNullOrEmpty(yandexDiskResponse?.Link))
                throw new ArgumentNullException(nameof(yandexDiskResponse.Link));

            byte[]? byteArray = await File.ReadAllBytesAsync("dictionary.db").ConfigureAwait(false);

            HttpResponseMessage uploadResponse = await Client.PutAsync(yandexDiskResponse.Link, new ByteArrayContent(byteArray, 0, byteArray.Length))
                .ConfigureAwait(false);
            uploadResponse.EnsureSuccessStatusCode();

            PrintMessage("Backup finished.");
        }

        static void PrintMessage(string message)
        {
            Console.WriteLine($"{DateTime.Now:yyyy-MM-dd--HH-mm-ss} — {message}");
        }
    }
}