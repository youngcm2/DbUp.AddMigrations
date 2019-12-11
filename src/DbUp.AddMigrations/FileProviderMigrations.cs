using System;
using System.IO;
using System.Text;
using Microsoft.Extensions.Logging;

namespace DbUp.AddMigrations
{
    class FileProviderMigrations : IProviderMigrations<FileProviderOptions>
    {
        private readonly ILogger<FileProviderMigrations> _logger;

        public FileProviderMigrations(ILogger<FileProviderMigrations> logger)
        {
            _logger = logger;
        }

        public int Add(FileProviderOptions options)
        {
            var currentDirectory = Directory.GetCurrentDirectory();

            var scriptsPath = Path.Combine(currentDirectory, options.ScriptsFolder);

            if (!Directory.Exists(scriptsPath))
            {
                _logger.LogDebug($"Creating directory: {scriptsPath}");
                try
                {
                    Directory.CreateDirectory(scriptsPath);
                }
                catch (Exception exception)
                {
                    _logger.LogCritical($"Could not create directory: {scriptsPath} - {exception.Message}");
                    return -1;
                }
            }

            var now = DateTimeOffset.Now;
            var timestamp = now.ToUniversalTime().ToString("yyyyMMddHHmmss");

            var fileName = $"{timestamp}_{options.Name}";

            if (!fileName.EndsWith(".sql"))
            {
                fileName += ".sql";
            }

            var header = new StringBuilder();

            
            header.Append("/*------------------------------------").AppendLine();
            header.AppendFormat("Migration Script: {0}", options.Name).AppendLine();
            header.AppendFormat("Created ({1}): {0}", now, TimeZoneInfo.Local.DisplayName).AppendLine();
            header.Append("------------------------------------*/").AppendLine().AppendLine();
            header.AppendFormat("PRINT 'Migration Script: {0}...';", options.Name).AppendLine().AppendLine();

            try
            {
                File.WriteAllText(fileName, header.ToString());
            }
            catch (Exception exception)
            {
                _logger.LogCritical($"Could not create file: {fileName} - {exception.Message}");
                return -1;
            }

            return 0;
        }
    }
}