using System.Threading.Tasks;

namespace YandexDiskFileUploader.FileReader
{
    public interface IFileReader
    {
        Task<byte[]> ReadFileAsync(string filePath);
    }
}