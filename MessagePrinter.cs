using System;

namespace YandexDiskSingleFileBackup
{
    public class MessagePrinter
    {
        public void Print(string message)
        {
            Console.WriteLine($"{DateTime.Now:yyyy-MM-dd--HH-mm-ss} — {message}");
        }
    }
}