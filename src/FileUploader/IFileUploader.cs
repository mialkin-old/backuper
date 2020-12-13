using System.Threading.Tasks;

namespace Slova.Backuper.FileUploader
{
    public interface IFileUploader
    {
        /// <summary>
        /// Requests upload link from Yandex.Disk.
        /// </summary>
        /// <returns>Upload link</returns>
        public Task<string> GetUploadLinkAsync();

        /// <summary>
        /// Uploads file to Yandex.Disk.
        /// </summary>
        /// <param name="uploadLink">Upload link.</param>
        /// <param name="fileBytes">File to upload.</param>
        Task UploadFileAsync(string uploadLink, byte[] fileBytes);
    }
}