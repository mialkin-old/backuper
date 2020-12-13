namespace YandexDiskFileUploader.Options
{
    public class FileUploaderSettings
    {
        public const string FileUploader = "FileUploader";
        public string OAuthToken { get; set; }
        
        /// <summary>
        /// Path to upload directory on Yandex.Disk.
        /// </summary>
        public string UploadDirectory { get; set; }
    }
}