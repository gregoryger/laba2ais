using System;
using System.Collections.Generic;
using Logic;
using Models;

namespace ConsoleApp
{
    /// <summary>
    /// Вспомогательные методы консольного интерфейса.
    /// </summary>
    internal static class ConsoleMethods
    {
        /// <summary>
        /// Запускает интерактивное меню.
        /// </summary>
        /// <param name="logic">Инжектированная бизнес-логика игр.</param>
        public static void Run(IGameLogic logic)
        {
            if (logic == null)
            {
                throw new ArgumentNullException(nameof(logic));
            }

            var exit = false;
            while (!exit)
            {
                ShowMenu();
                var choice = ReadInt("\nВведите номер пункта (0-7): ", 0, 7);
                Console.Clear();

                switch (choice)
                {
                    case 1:
                        AddGame(logic);
                        break;
                    case 2:
                        ShowAllGames(logic);
                        break;
                    case 3:
                        UpdateGame(logic);
                        break;
                    case 4:
                        DeleteGame(logic);
                        break;
                    case 5:
                        FilterByGenre(logic);
                        break;
                    case 6:
                        ShowTopRated(logic);
                        break;
                    case 7:
                        ExportGames(logic);
                        break;
                    case 0:
                        exit = true;
                        break;
                }
            }
        }

        private static void ShowMenu()
        {
            Console.WriteLine("=== Каталог любимых игр ===");
            Console.WriteLine("1. Добавить игру");
            Console.WriteLine("2. Показать все игры");
            Console.WriteLine("3. Изменить игру");
            Console.WriteLine("4. Удалить игру");
            Console.WriteLine("5. Фильтр по жанру");
            Console.WriteLine("6. ТОП N по рейтингу");
            Console.WriteLine("7. Экспорт в JSON");
            Console.WriteLine("0. Выход");
        }

        private static int ReadInt(string prompt, int min, int max)
        {
            while (true)
            {
                Console.Write(prompt);
                var raw = Console.ReadLine();
                if (int.TryParse(raw, out var value) && value >= min && value <= max)
                {
                    return value;
                }

                Console.WriteLine($"Ошибка ввода. Введите число от {min} до {max}.");
            }
        }

        private static double ReadDouble(string prompt, double min, double max)
        {
            while (true)
            {
                Console.Write(prompt);
                var raw = Console.ReadLine();
                if (double.TryParse(raw, out var value) && value >= min && value <= max)
                {
                    return value;
                }

                Console.WriteLine($"Ошибка ввода. Введите число от {min} до {max}.");
            }
        }

        private static string ReadRequiredString(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                var value = Console.ReadLine();

                if (!string.IsNullOrWhiteSpace(value))
                {
                    return value.Trim();
                }

                Console.WriteLine("Строка не может быть пустой. Повторите ввод.");
            }
        }

        private static void AddGame(IGameLogic logic)
        {
            try
            {
                var name = ReadRequiredString("Название: ");
                var genre = ReadRequiredString("Жанр: ");
                var rating = ReadDouble("Рейтинг (0-10): ", 0, 10);

                var game = new Game { Name = name, Genre = genre, Rating = rating };
                logic.AddGame(game);
                Console.WriteLine($"Игра добавлена (ID={game.Id}).");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Не удалось добавить игру: {ex.Message}");
            }

            Pause();
        }

        private static void ShowAllGames(IGameLogic logic)
        {
            PrintGames(logic.GetAllGames(), "Пока игр нет.");
        }

        private static void UpdateGame(IGameLogic logic)
        {
            var games = logic.GetAllGames();
            PrintGames(games, "Список пуст. Изменять нечего.");
            if (games.Count == 0)
            {
                return;
            }

            try
            {
                var id = ReadInt("ID для обновления: ", 1, int.MaxValue);
                var storedGame = logic.GetGameById(id);
                if (storedGame == null)
                {
                    Console.WriteLine("Игра не найдена.");
                    Pause();
                    return;
                }

                Console.Write($"Новое название ({storedGame.Name}): ");
                var name = Console.ReadLine();
                Console.Write($"Новый жанр ({storedGame.Genre}): ");
                var genre = Console.ReadLine();
                Console.Write($"Новый рейтинг ({storedGame.Rating:0.0}): ");
                var ratingRaw = Console.ReadLine();

                var updated = new Game
                {
                    Id = storedGame.Id,
                    Name = string.IsNullOrWhiteSpace(name) ? storedGame.Name : name.Trim(),
                    Genre = string.IsNullOrWhiteSpace(genre) ? storedGame.Genre : genre.Trim(),
                    Rating = double.TryParse(ratingRaw, out var ratingValue) && ratingValue >= 0 && ratingValue <= 10
                        ? ratingValue
                        : storedGame.Rating
                };

                if (logic.UpdateGame(updated))
                {
                    Console.WriteLine("Игра обновлена.");
                }
                else
                {
                    Console.WriteLine("Не удалось обновить игру.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при обновлении: {ex.Message}");
            }

            Pause();
        }

        private static void DeleteGame(IGameLogic logic)
        {
            var games = logic.GetAllGames();
            PrintGames(games, "Список пуст. Удалять нечего.");
            if (games.Count == 0)
            {
                return;
            }

            try
            {
                var id = ReadInt("ID для удаления: ", 1, int.MaxValue);
                if (logic.DeleteGame(id))
                {
                    Console.WriteLine("Игра удалена.");
                }
                else
                {
                    Console.WriteLine("Игра с таким ID не найдена.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при удалении: {ex.Message}");
            }

            Pause();
        }

        private static void FilterByGenre(IGameLogic logic)
        {
            var genre = ReadRequiredString("Введите жанр для фильтрации: ");
            try
            {
                PrintGames(logic.FilterByGenre(genre), "Игры с таким жанром не найдены.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка фильтрации: {ex.Message}");
                Pause();
            }
        }

        private static void ShowTopRated(IGameLogic logic)
        {
            var count = ReadInt("Сколько игр показать? ", 1, 100);
            try
            {
                PrintGames(logic.GetTopRatedGames(count), "По заданным условиям нет игр.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка выборки: {ex.Message}");
                Pause();
            }
        }

        private static void ExportGames(IGameLogic logic)
        {
            Console.Write("Укажите путь для JSON (по умолчанию games_export.json): ");
            var inputPath = Console.ReadLine();
            var path = string.IsNullOrWhiteSpace(inputPath) ? "games_export.json" : inputPath.Trim();

            try
            {
                logic.ExportToJson(path);
                Console.WriteLine($"Экспорт завершён. Файл: {path}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Не удалось экспортировать: {ex.Message}");
            }

            Pause();
        }

        private static void PrintGames(IReadOnlyCollection<Game> games, string emptyMessage)
        {
            if (games.Count == 0)
            {
                Console.WriteLine(emptyMessage);
                Pause();
                return;
            }

            foreach (var game in games)
            {
                Console.WriteLine($"{game.Id}. {game.Name} ({game.Genre}), рейтинг: {game.Rating:0.0}");
            }

            Pause();
        }

        private static void Pause()
        {
            Console.WriteLine("\nНажмите любую клавишу для продолжения...");
            Console.ReadKey();
            Console.Clear();
        }
    }
}
