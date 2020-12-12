using System.Threading.Tasks;

namespace YandexDiskFileUploader.Interfaces
{
    public interface IFileReader
    {
        /// <summary>
        /// Reads file from specified path.
        /// </summary>
        /// <param name="filePath">File path.</param>
        /// <returns>File bytes.</returns>
        Task<byte[]> ReadFileAsync(string filePath);
    }
}