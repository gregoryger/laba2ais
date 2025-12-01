using System;
using System.Globalization;
using System.IO;
using Models;

namespace Logic.Logging
{
    /// <summary>
    /// Логгер, записывающий данные в файл.
    /// </summary>
    public class FileGameLogger : IGameLogger
    {
        private readonly string _logFilePath;
        private readonly object _syncRoot = new();

        /// <summary>
        /// Создает логгер с указанием пути к файлу.
        /// </summary>
        /// <param name="logFilePath">Путь к файлу логов.</param>
        public FileGameLogger(string logFilePath)
        {
            if (string.IsNullOrWhiteSpace(logFilePath))
            {
                throw new ArgumentException("Путь к файлу логов обязателен.", nameof(logFilePath));
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
            WriteLine("ERROR", $"{message} | {exceptionMessage}");
        }

        /// <inheritdoc/>
        public void LogGameSnapshot(Game game)
        {
            if (game == null)
            {
                return;
            }

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
