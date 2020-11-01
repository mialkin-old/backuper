using System.Threading.Tasks;

namespace YandexDiskSingleFileBackup
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