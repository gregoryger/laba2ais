using System;

namespace GameApp.Shared
{
    /// <summary>
    /// Аргументы с моделью игры.
    /// </summary>
    public class GameEventArgs : EventArgs
    {
        /// <summary>
        /// Создает набор аргументов с игрой.
        /// </summary>
        /// <param name="game">Выбранная игра.</param>
        public GameEventArgs(GameViewModel game)
        {
            Game = game ?? throw new ArgumentNullException(nameof(game));
        }

        /// <summary>
        /// Выбранная игра.
        /// </summary>
        public GameViewModel Game { get; }
    }

    /// <summary>
    /// Аргументы фильтрации по жанру.
    /// </summary>
    public class GenreFilterEventArgs : EventArgs
    {
        /// <summary>
        /// Создает аргументы фильтрации.
        /// </summary>
        /// <param name="genre">Жанр для фильтрации (пусто = все).</param>
        public GenreFilterEventArgs(string genre)
        {
            Genre = genre ?? string.Empty;
        }

        /// <summary>
        /// Жанр для фильтрации.
        /// </summary>
        public string Genre { get; }
    }

    /// <summary>
    /// Аргументы запроса топа игр.
    /// </summary>
    public class TopGamesRequestEventArgs : EventArgs
    {
        /// <summary>
        /// Создает аргументы запроса топа.
        /// </summary>
        /// <param name="count">Количество игр.</param>
        public TopGamesRequestEventArgs(int count)
        {
            Count = count;
        }

        /// <summary>
        /// Количество игр.
        /// </summary>
        public int Count { get; }
    }

    /// <summary>
    /// Аргументы с путем к файлу.
    /// </summary>
    public class FilePathEventArgs : EventArgs
    {
        /// <summary>
        /// Создает аргументы с путем к файлу.
        /// </summary>
        /// <param name="path">Путь.</param>
        public FilePathEventArgs(string path)
        {
            Path = path ?? string.Empty;
        }

        /// <summary>
        /// Путь к файлу.
        /// </summary>
        public string Path { get; }
    }
}
