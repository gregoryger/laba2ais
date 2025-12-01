using System.Collections.Generic;
using Models;

namespace Logic
{
    /// <summary>
    /// Контракт бизнес-логики для работы с играми.
    /// </summary>
    public interface IGameLogic
    {
        /// <summary>
        /// Добавляет новую игру.
        /// </summary>
        /// <param name="game">Данные игры.</param>
        /// <returns>Сохраненная игра.</returns>
        Game AddGame(Game game);

        /// <summary>
        /// Удаляет игру по идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор игры.</param>
        /// <returns>True, если игра удалена.</returns>
        bool DeleteGame(int id);

        /// <summary>
        /// Возвращает все игры.
        /// </summary>
        /// <returns>Список игр.</returns>
        List<Game> GetAllGames();

        /// <summary>
        /// Возвращает игру по идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор игры.</param>
        /// <returns>Экземпляр игры или null.</returns>
        Game? GetGameById(int id);

        /// <summary>
        /// Обновляет данные игры.
        /// </summary>
        /// <param name="game">Игра с измененными полями.</param>
        /// <returns>True, если обновление выполнено.</returns>
        bool UpdateGame(Game game);

        /// <summary>
        /// Возвращает игры указанного жанра.
        /// </summary>
        /// <param name="genre">Жанр (пусто = все).</param>
        /// <returns>Список игр.</returns>
        List<Game> FilterByGenre(string genre);

        /// <summary>
        /// Возвращает топ игр по рейтингу.
        /// </summary>
        /// <param name="count">Количество записей.</param>
        /// <returns>Список игр.</returns>
        List<Game> GetTopRatedGames(int count);

        /// <summary>
        /// Экспортирует данные в JSON.
        /// </summary>
        /// <param name="filePath">Путь к файлу.</param>
        void ExportToJson(string filePath);

        /// <summary>
        /// Импортирует данные из JSON.
        /// </summary>
        /// <param name="filePath">Путь к файлу.</param>
        /// <returns>Количество добавленных записей.</returns>
        int ImportFromJson(string filePath);
    }
}
