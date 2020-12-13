using System.Threading.Tasks;

namespace YandexDiskFileUploader.FileReader
{
    public interface IFileReader
    {
        /// <summary>
        /// Reads file from specified path.
        /// </summary>
        /// <returns>File bytes.</returns>
        Task<byte[]> ReadFileAsync();
    }
}