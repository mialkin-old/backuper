using System.ComponentModel.DataAnnotations;

namespace YandexDiskFileUploader.Utils
{
    public class YaDiskOptions
    {
        /// <summary>
        /// OAuth token from the application registered on Yandex's OAuth server. <see href="https://oauth.yandex.com"/>
        /// </summary>
        public string YandexAppOauthToken { get; set; }

        /// <summary>
        /// Path to the directory with the file to upload.
        /// </summary>
        [Required(AllowEmptyStrings = false)]
        public string SourceFileDirectoryPath { get; set; }

        /// <summary>
        /// Name of the file to upload.
        /// </summary>
        public string SourceFileName { get; set; }

        /// <summary>
        /// Path to upload directory on Yandex.Disk.
        /// </summary>
        public string UploadDirectoryPath { get; set; }
    }
}