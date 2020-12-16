using System;
using System.Net.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Slova.Backuper.FileReader;
using Slova.Backuper.FileUploader;
using Slova.Backuper.Settings;

namespace Slova.Backuper
{
    public static class ServicesConfiguration
    {
        public static void AddCustomServices(this IServiceCollection services, IConfigurationRoot configurationRoot)
        {
            services
                .AddSingleton(new HttpClient { BaseAddress = new Uri(configurationRoot.GetValue<string>("YandexDiskApi")) })
                .AddTransient<IFileReader, FileReader.FileReader>()
                .AddTransient<IFileUploader, FileUploader.FileUploader>()
                .AddTransient<IApp, App>();

            services.AddOptions<FileReaderSettings>().Bind(configurationRoot.GetSection(FileReaderSettings.FileReader))
                .ValidateDataAnnotations();

            services.AddOptions<FileUploaderSettings>().Bind(configurationRoot.GetSection(FileUploaderSettings.FileUploader))
                .ValidateDataAnnotations();
        }
    }
}