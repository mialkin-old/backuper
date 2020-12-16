using System.Threading.Tasks;

namespace Slova.Backuper
{
    /// <summary>
    /// Application.
    /// </summary>
    public interface IApp
    {
        /// <summary>
        /// Runs application.
        /// </summary>
        Task Run();
    }
}