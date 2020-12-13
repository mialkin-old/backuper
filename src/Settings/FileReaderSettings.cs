using System.ComponentModel.DataAnnotations;

namespace Slova.Backuper.Settings
{
    public class FileReaderSettings
    {
        public const string FileReader = "FileReader";

        /// <summary>
        /// Path to the directory with the file to upload.
        /// </summary>
        [Required(AllowEmptyStrings = false)]
        public string FileDirectory { get; set; }

        /// <summary>
        /// Name of the file.
        /// </summary>
        [Required(AllowEmptyStrings = false)]
        public string FileName { get; set; }
    }
}