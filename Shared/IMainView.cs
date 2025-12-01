using System;
using System.Collections.Generic;

namespace GameApp.Shared
{
    /// <summary>
    /// Контракт главного окна для взаимодействия с Presenter.
    /// </summary>
    public interface IMainView
    {
        /// <summary>
        /// Событие запроса загрузки данных при инициализации окна.
        /// </summary>
        event EventHandler LoadRequested;

        /// <summary>
        /// Событие запроса добавления игры.
        /// </summary>
        event EventHandler AddGameRequested;

        /// <summary>
        /// Событие запроса редактирования выбранной игры.
        /// </summary>
        event EventHandler<GameEventArgs> EditGameRequested;

        /// <summary>
        /// Событие запроса удаления выбранной игры.
        /// </summary>
        event EventHandler<GameEventArgs> DeleteGameRequested;

        /// <summary>
        /// Событие запроса фильтрации по жанру.
        /// </summary>
        event EventHandler<GenreFilterEventArgs> FilterRequested;

        /// <summary>
        /// Событие запроса топа игр.
        /// </summary>
        event EventHandler<TopGamesRequestEventArgs> TopGamesRequested;

        /// <summary>
        /// Событие экспорта данных в файл.
        /// </summary>
        event EventHandler<FilePathEventArgs> ExportRequested;

        /// <summary>
        /// Событие импорта данных из файла.
        /// </summary>
        event EventHandler<FilePathEventArgs> ImportRequested;

        /// <summary>
        /// Событие поиска игр по части названия.
        /// </summary>
        event EventHandler<string> SearchRequested;

        /// <summary>
        /// Отображает коллекцию игр.
        /// </summary>
        /// <param name="games">Список моделей для отображения.</param>
        void ShowGames(IReadOnlyCollection<GameViewModel> games);

        /// <summary>
        /// Отображает доступные жанры и выбирает текущий.
        /// </summary>
        /// <param name="genres">Коллекция жанров.</param>
        /// <param name="selectedGenre">Текущий жанр или null.</param>
        void ShowGenres(IReadOnlyCollection<string> genres, string? selectedGenre);

        /// <summary>
        /// Запрашивает у пользователя данные игры.
        /// </summary>
        /// <param name="source">Исходная игра или null для создания.</param>
        /// <returns>Заполненные данные или null, если отменено.</returns>
        GameViewModel? RequestGameData(GameViewModel? source);

        /// <summary>
        /// Показывает информационное сообщение.
        /// </summary>
        /// <param name="message">Текст сообщения.</param>
        void ShowInfo(string message);

        /// <summary>
        /// Показывает сообщение об ошибке.
        /// </summary>
        /// <param name="message">Текст ошибки.</param>
        void ShowError(string message);

        /// <summary>
        /// Запрашивает подтверждение действия.
        /// </summary>
        /// <param name="message">Сообщение.</param>
        /// <param name="caption">Заголовок.</param>
        /// <returns>True, если пользователь подтвердил.</returns>
        bool Confirm(string message, string caption);
    }
}
