using System;

namespace DictionaryBackup
{
    class Program
    {
        static void Main(string[] args)
        {
            string? oAuthToken = Environment.GetEnvironmentVariable("DICT_BACKUP_OAUTH_TOKEN");
            if(string.IsNullOrEmpty(oAuthToken))
                throw new ArgumentNullException(nameof(oAuthToken));

            PrintMessage("Starting backup.");
            Backup(oAuthToken);

            Console.WriteLine($"{oAuthToken}");
        }

        static void Backup(string token)
        {
        }

        static void PrintMessage(string message)
        {
            Console.WriteLine($"{DateTime.Now:yyyy-dd-M--HH-mm-ss} — {message}");
        }
    }
}