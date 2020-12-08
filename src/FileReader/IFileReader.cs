using System.Threading.Tasks;

namespace Backuper.FileReader
{
    public interface IFileReader
    {
        Task<byte[]> ReadFileAsync(string filePath);
    }
}