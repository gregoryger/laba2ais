using System;
using System.Linq;
using System.Windows.Forms;
using Logic;
using Models;

namespace GameApp.UI
{
    /// <summary>
    /// Главная форма для управления каталогом игр.
    /// </summary>
    public partial class MainForm : Form
    {
        private readonly IGameLogic _logic;

        /// <summary>
        /// Инициализирует форму.
        /// </summary>
        /// <param name="logic">Логика работы с играми.</param>
        public MainForm(IGameLogic logic)
        {
            _logic = logic ?? throw new ArgumentNullException(nameof(logic));

            InitializeComponent();
            RefreshGames();
        }

        private void RefreshGames()
        {
            try
            {
                var games = _logic.GetAllGames();
                dgvGames.DataSource = null;
                dgvGames.DataSource = games;

                cbFilterGenre.Items.Clear();
                cbFilterGenre.Items.Add("Все жанры");

                foreach (var genre in games.Select(g => g.Genre).Distinct().OrderBy(g => g))
                {
                    cbFilterGenre.Items.Add(genre);
                }

                if (cbFilterGenre.Items.Count > 0)
                {
                    cbFilterGenre.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                ShowError("Не удалось обновить список игр", ex);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                var dialog = new GameDialog();
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    var game = new Game
                    {
                        Name = dialog.GameName,
                        Genre = dialog.GameGenre,
                        Rating = dialog.GameRating
                    };

                    _logic.AddGame(game);
                    RefreshGames();
                    ShowInfo("Игра успешно добавлена!");
                }
            }
            catch (Exception ex)
            {
                ShowError("Не удалось добавить игру", ex);
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                var game = GetSelectedGame();
                if (game == null)
                {
                    return;
                }

                var dialog = new GameDialog
                {
                    GameName = game.Name,
                    GameGenre = game.Genre,
                    GameRating = game.Rating
                };

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    var updatedGame = new Game
                    {
                        Id = game.Id,
                        Name = dialog.GameName,
                        Genre = dialog.GameGenre,
                        Rating = dialog.GameRating
                    };

                    if (_logic.UpdateGame(updatedGame))
                    {
                        RefreshGames();
                        ShowInfo("Игра обновлена.");
                    }
                    else
                    {
                        ShowInfo("Игра не найдена для обновления.");
                    }
                }
            }
            catch (Exception ex)
            {
                ShowError("Не удалось изменить игру", ex);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                var game = GetSelectedGame();
                if (game == null)
                {
                    return;
                }

                var result = MessageBox.Show(
                    $"Удалить игру \"{game.Name}\"?",
                    "Подтверждение удаления",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    if (_logic.DeleteGame(game.Id))
                    {
                        RefreshGames();
                        ShowInfo("Игра удалена.");
                    }
                    else
                    {
                        ShowInfo("Игра не найдена.");
                    }
                }
            }
            catch (Exception ex)
            {
                ShowError("Не удалось удалить игру", ex);
            }
        }

        private void btnFilter_Click(object sender, EventArgs e)
        {
            try
            {
                if (cbFilterGenre.SelectedItem == null)
                {
                    ShowInfo("Выберите жанр для фильтрации.");
                    return;
                }

                var genre = cbFilterGenre.SelectedItem.ToString();
                if (genre == "Все жанры")
                {
                    dgvGames.DataSource = _logic.GetAllGames();
                }
                else
                {
                    dgvGames.DataSource = _logic.FilterByGenre(genre!);
                }
            }
            catch (Exception ex)
            {
                ShowError("Ошибка фильтрации", ex);
            }
        }

        private void btnTopRated_Click(object sender, EventArgs e)
        {
            try
            {
                dgvGames.DataSource = _logic.GetTopRatedGames(3);
                ShowInfo("Показаны 3 игры с наивысшим рейтингом.");
            }
            catch (Exception ex)
            {
                ShowError("Не удалось получить топ-игры", ex);
            }
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            using var dialog = new SaveFileDialog
            {
                Filter = "JSON файлы (*.json)|*.json|Все файлы (*.*)|*.*",
                FileName = "games_export.json",
                Title = "Сохранение каталога игр"
            };

            if (dialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            try
            {
                _logic.ExportToJson(dialog.FileName);
                ShowInfo($"Экспорт выполнен: {dialog.FileName}");
            }
            catch (Exception ex)
            {
                ShowError("Не удалось экспортировать каталог", ex);
            }
        }

        private Game? GetSelectedGame()
        {
            if (dgvGames.CurrentRow == null)
            {
                ShowInfo("Сначала выберите игру в таблице.");
                return null;
            }

            return dgvGames.CurrentRow.DataBoundItem as Game;
        }

        private static void ShowInfo(string message)
        {
            MessageBox.Show(message, "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private static void ShowError(string message, Exception ex)
        {
            MessageBox.Show($"{message}: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
