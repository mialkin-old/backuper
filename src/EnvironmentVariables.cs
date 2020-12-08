using System;

namespace YandexDiskFileUploader
{
    public class EnvironmentVariables
    {
        /// <summary>
        /// OAuth token from the <see href="https://oauth.yandex.com">application</see> registered on Yandex's OAuth server.
        /// </summary>
        public string OauthToken { get; } = Get("OAUTH_TOKEN");
        
        /// <summary>
        /// Path to the directory with the file to upload.
        /// </summary>
        public string SourceFolderPath { get; set; } = Get("SOURCE_FOLDER_PATH");

        /// <summary>
        /// Name of the file to upload.
        /// </summary>
        public string SourceFileName { get; set; } = Get("SOURCE_FILE_NAME");

        /// <summary>
        /// Path to upload directory on Yandex.Disk.
        /// </summary>
        public string UploadDirectoryPath { get; set; } = Get("YANDEX_DISK_FOLDER_PATH");

        private static string Get(string name)
        {
            string? variable = Environment.GetEnvironmentVariable(name);
            if (string.IsNullOrEmpty(variable))
                throw new ArgumentNullException($"Environment variable {name} is not set.");

            return variable;
        }
    }
}