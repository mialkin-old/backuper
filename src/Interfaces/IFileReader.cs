using System.Threading.Tasks;

namespace YandexDiskFileUploader.Interfaces
{
    public interface IFileReader
    {
        Task<byte[]> ReadFileAsync(string filePath);
    }
}