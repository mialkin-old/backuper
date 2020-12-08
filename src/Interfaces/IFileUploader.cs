using System.Threading.Tasks;

namespace YandexDiskFileUploader.Interfaces
{
    public interface IFileUploader
    {
        /// <summary>
        /// Запрашивает у Яндекс.Диска ссылку, по которой можно будет выгрузить файл.
        /// </summary>
        /// <param name="yandexDiskDirectoryPath">Полный путь до директории на Яндекс.Диске, в которую нужно загрузить файл.</param>
        /// <param name="filename">Название, которое будет у загруженного на Яндекс.Диск файла.</param>
        /// <returns></returns>
        public Task<string> GetYandexDiskUploadLinkAsync(string yandexDiskDirectoryPath, string filename);
        
        /// <summary>
        /// Uploads the file to Yandex.Disk.
        /// </summary>
        /// <param name="uploadUri">URI to upload file.</param>
        /// <param name="fileBytes">Файл для выгрузки.</param>
        /// <returns></returns>
        Task UploadFileAsync(string uploadUri, byte[] fileBytes);
    }
}