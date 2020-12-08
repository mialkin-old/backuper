using System;

namespace YandexDiskFileUploader
{
    public class EnvironmentVariables
    {
        /// <summary>
        /// OAuth-токен зарегистрированного на Яндекс.Диске <see href="https://oauth.yandex.ru"/>приложения</>.
        /// </summary>
        public string OauthToken { get; } = Get("OAUTH_TOKEN");

        /// <summary>
        /// Путь до папки с файлом, который нужно сохранить.
        /// </summary>
        public string SourceFolderPath { get; set; } = Get("SOURCE_FOLDER_PATH");

        /// <summary>
        /// Имя файла, который нужно сохранить.
        /// </summary>
        public string SourceFileName { get; set; } = Get("SOURCE_FILE_NAME");

        /// <summary>
        /// Путь до папки на Яндекс.Диске, в которую нужно сохранить резервную копию.
        /// </summary>
        public string YandexDiskFolderPath { get; set; } = Get("YANDEX_DISK_FOLDER_PATH");

        private static string Get(string name)
        {
            string? variable = Environment.GetEnvironmentVariable(name);
            if (string.IsNullOrEmpty(variable))
                throw new ArgumentNullException($"Не задана переменная окружения {name}.");

            return variable;
        }
    }
}