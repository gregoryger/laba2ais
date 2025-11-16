using System.Collections.Generic;
using Models;

namespace Logic
{
    /// <summary>
    /// Абстракция для сценариев работы с сущностью <see cref="Game"/>.
    /// </summary>
    public interface IGameLogic
    {
        /// <summary>
        /// Добавляет игру после валидации.
        /// </summary>
        /// <param name="game">Экземпляр игры для добавления.</param>
        /// <returns>Сохранённая игра с присвоенным идентификатором.</returns>
        Game AddGame(Game game);

        /// <summary>
        /// Удаляет игру по идентификатору, если она найдена.
        /// </summary>
        /// <param name="id">Идентификатор игры.</param>
        /// <returns>True, если запись удалена, иначе false.</returns>
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
        /// <returns>Игра или null, если не найдена.</returns>
        Game? GetGameById(int id);

        /// <summary>
        /// Обновляет информацию об игре.
        /// </summary>
        /// <param name="game">Игра с обновлёнными полями.</param>
        /// <returns>True, если обновление применено, иначе false.</returns>
        bool UpdateGame(Game game);

        /// <summary>
        /// Возвращает игры указанного жанра.
        /// </summary>
        /// <param name="genre">Жанр для поиска.</param>
        /// <returns>Отфильтрованный список игр.</returns>
        List<Game> FilterByGenre(string genre);

        /// <summary>
        /// Возвращает наиболее рейтинговые игры с ограничением по количеству.
        /// </summary>
        /// <param name="count">Максимальное количество записей.</param>
        /// <returns>Список игр с наивысшим рейтингом.</returns>
        List<Game> GetTopRatedGames(int count);

        /// <summary>
        /// Сохраняет текущий каталог в JSON-файл.
        /// </summary>
        /// <param name="filePath">Путь к итоговому файлу.</param>
        void ExportToJson(string filePath);
    }
}
