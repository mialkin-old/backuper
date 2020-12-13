using System.ComponentModel.DataAnnotations;

namespace Slova.Backuper.Settings
{
    public class FileUploaderSettings
    {
        public const string FileUploader = "FileUploader";

        [Required(AllowEmptyStrings = false)]
        public string OAuthToken { get; set; }
        
        /// <summary>
        /// Path to upload directory on Yandex.Disk.
        /// </summary>
        [Required(AllowEmptyStrings = false)]
        public string UploadDirectory { get; set; }
        
        /// <summary>
        /// Name of the file to upload.
        /// </summary>
        [Required(AllowEmptyStrings = false)]
        public string FileName { get; set; }
    }
}