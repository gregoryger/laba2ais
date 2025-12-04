using System;
using Models;

namespace Logic
{
    /// <summary>
    /// Аргументы событий изменения одной игры.
    /// </summary>
    public class GameChangedEventArgs : EventArgs
    {
        /// <summary>
        /// Создает аргументы события.
        /// </summary>
        /// <param name="game">Затронутая игра.</param>
        public GameChangedEventArgs(Game game)
        {
            Game = game ?? throw new ArgumentNullException(nameof(game));
        }

        /// <summary>
        /// Игра, к которой относится событие.
        /// </summary>
        public Game Game { get; }
    }

    /// <summary>
    /// Аргументы события массового импорта.
    /// </summary>
    public class GamesImportedEventArgs : EventArgs
    {
        /// <summary>
        /// Создает аргументы импорта.
        /// </summary>
        /// <param name="addedCount">Количество добавленных записей.</param>
        public GamesImportedEventArgs(int addedCount)
        {
            AddedCount = addedCount;
        }

        /// <summary>
        /// Количество успешно добавленных игр.
        /// </summary>
        public int AddedCount { get; }
    }
}
