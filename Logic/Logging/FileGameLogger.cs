using System;
using System.Globalization;
using System.IO;
using Models;

namespace Logic.Logging
{
    /// <summary>
    /// Пишет события аудита в текстовый файл.
    /// </summary>
    public class FileGameLogger : IGameLogger
    {
        private readonly string _logFilePath;
        private readonly object _syncRoot = new();

        /// <summary>
        /// Инициализирует логгер с путём к файлу.
        /// </summary>
        /// <param name="logFilePath">Путь до файла лога.</param>
        public FileGameLogger(string logFilePath)
        {
            if (string.IsNullOrWhiteSpace(logFilePath))
            {
                throw new ArgumentException("Log file path must be provided.", nameof(logFilePath));
            }

            _logFilePath = logFilePath;
        }

        /// <inheritdoc/>
        public void LogInfo(string message)
        {
            WriteLine("INFO", message);
        }

        /// <inheritdoc/>
        public void LogError(string message, string exceptionMessage)
        {
            WriteLine("ERROR", $"{message} | Details: {exceptionMessage}");
        }

        /// <inheritdoc/>
        public void LogGameSnapshot(Game game)
        {
            WriteLine(
                "SNAPSHOT",
                $"Id={game.Id}, Name={game.Name}, Genre={game.Genre}, Rating={game.Rating.ToString("0.0", CultureInfo.InvariantCulture)}");
        }

        private void WriteLine(string level, string message)
        {
            var timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
            var line = $"{timestamp} [{level}] {message}";

            lock (_syncRoot)
            {
                var directory = Path.GetDirectoryName(_logFilePath);
                if (!string.IsNullOrWhiteSpace(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                File.AppendAllText(_logFilePath, line + Environment.NewLine);
            }
        }
    }
}
