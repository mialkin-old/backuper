using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace DictionaryBackup
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var environmentVariables = new EnvironmentVariables();
            var messagePrinter = new MessagePrinter();

            await new Backuper(environmentVariables, messagePrinter).Backup();
        }
    }
}