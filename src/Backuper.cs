using System.Threading.Tasks;
using YandexDiskFileUploader.Interfaces;

namespace YandexDiskFileUploader
{
    public class Backuper
    {
        private readonly IFileReader _fileReader;
        private readonly IYandexDiskFileUploader _yandexDiskFileUploader;

        public Backuper(IFileReader fileReader, IYandexDiskFileUploader yandexDiskFileUploader)
        {
            _fileReader = fileReader;
            _yandexDiskFileUploader = yandexDiskFileUploader;
        }

        public async Task StartBackup()
        {
            
        }
    }
}