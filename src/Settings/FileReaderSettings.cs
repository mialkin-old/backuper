using System.ComponentModel.DataAnnotations;

namespace YandexDiskFileUploader.Settings
{
    public class FileReaderSettings
    {
        public const string App = "App";
        
        /// <summary>
        /// Path to the directory with the file to upload.
        /// </summary>
        [Required(AllowEmptyStrings = false)]
        public string FileDirectory { get; set; }
        
        /// <summary>
        /// Name of the file to upload.
        /// </summary>
        public string FileName { get; set; }
    }
}