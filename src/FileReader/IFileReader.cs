using System.Threading.Tasks;

namespace Slova.Backuper.FileReader
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