using System.Threading.Tasks;

namespace YandexDiskFileUploader.Interfaces
{
    public interface IFileUploader
    {
        /// <summary>
        /// Requests upload link from Yandex.Disk.
        /// </summary>
        /// <param name="filename">Name which uploaded file will have on Yandex.Disk.</param>
        /// <returns>Upload link</returns>
        public Task<string> GetUploadLinkAsync(string filename);

        /// <summary>
        /// Uploads file to Yandex.Disk.
        /// </summary>
        /// <param name="uploadLink">Upload link.</param>
        /// <param name="fileBytes">File to upload.</param>
        Task UploadFileAsync(string uploadLink, byte[] fileBytes);
    }
}