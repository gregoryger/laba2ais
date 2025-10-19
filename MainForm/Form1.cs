using System;
using System.Linq;
using System.Windows.Forms;
using Logic;
using Models;
using DataAccessLayer;
using DataAccessLayer.EF;
using Microsoft.EntityFrameworkCore;

namespace GameApp.UI
{
    /// <summary>
    /// Главная форма приложения для управления играми.
    /// </summary>
    public partial class MainForm : Form
    {
        private readonly GameLogic _logic;

        /// <summary>
        /// Конструктор главной формы.
        /// </summary>
        public MainForm()
        {
            InitializeComponent();

            // Настройка подключения к SQL Server
            var options = new DbContextOptionsBuilder<GameDbContext>()
                .UseSqlServer("Server=LAPTOP-11O4LT8E\\SQLEXPRESS02;Database=GamesDB;Trusted_Connection=True;TrustServerCertificate=True;")
                .Options;


            var context = new GameDbContext(options);

            // Создаст базу данных, если её нет
            context.Database.EnsureCreated();

            // Инициализация репозитория и бизнес-логики
            IRepository<Game> repository = new EntityRepository<Game>(context);
            _logic = new GameLogic(repository);

            RefreshGames();
        }


        /// <summary>
        /// Обновляет отображение списка игр и жанров.
        /// </summary>
        private void RefreshGames()
        {
            try
            {
                dgvGames.DataSource = null;
                dgvGames.DataSource = _logic.GetAllGames();

                cbFilterGenre.Items.Clear();
                cbFilterGenre.Items.Add("Все");

                var genres = _logic.GetAllGames()
                    .Select(g => g.Genre)
                    .Distinct()
                    .OrderBy(g => g);

                foreach (var genre in genres)
                {
                    cbFilterGenre.Items.Add(genre);
                }

                if (cbFilterGenre.Items.Count > 0)
                    cbFilterGenre.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при обновлении списка: {ex.Message}", 
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Обработчик добавления новой игры.
        /// </summary>
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
                    MessageBox.Show("Игра успешно добавлена!", "Успех", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при добавлении игры: {ex.Message}", 
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Обработчик редактирования выбранной игры.
        /// </summary>
        private void btnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvGames.CurrentRow == null)
                {
                    MessageBox.Show("Выберите игру для редактирования.", 
                        "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var game = (Game)dgvGames.CurrentRow.DataBoundItem;
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

                    _logic.UpdateGame(updatedGame);
                    RefreshGames();
                    MessageBox.Show("Игра успешно обновлена!", "Успех", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при редактировании игры: {ex.Message}", 
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Обработчик удаления выбранной игры.
        /// </summary>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvGames.CurrentRow == null)
                {
                    MessageBox.Show("Выберите игру для удаления.", 
                        "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var game = (Game)dgvGames.CurrentRow.DataBoundItem;
                var result = MessageBox.Show(
                    $"Вы уверены, что хотите удалить игру \"{game.Name}\"?",
                    "Подтверждение удаления",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);


                if (result == DialogResult.Yes)
                {
                    _logic.DeleteGame(game.Id);
                    RefreshGames();
                    MessageBox.Show("Игра успешно удалена!", "Успех", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при удалении игры: {ex.Message}", 
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Обработчик фильтрации игр по жанру.
        /// </summary>
        private void btnFilter_Click(object sender, EventArgs e)
        {
            try
            {
                if (cbFilterGenre.SelectedItem == null)
                {
                    MessageBox.Show("Выберите жанр для фильтрации.", 
                        "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string genre = cbFilterGenre.SelectedItem.ToString();

                if (genre == "Все")
                {
                    dgvGames.DataSource = _logic.GetAllGames();
                }
                else
                {
                    dgvGames.DataSource = _logic.FilterByGenre(genre);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при фильтрации: {ex.Message}", 
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Обработчик показа топ-3 игр по рейтингу.
        /// </summary>
        private void btnTopRated_Click(object sender, EventArgs e)
        {
            try
            {
                var topGames = _logic.GetTopRatedGames(3);
                dgvGames.DataSource = topGames;

                if (topGames.Count == 0)
                {
                    MessageBox.Show("Нет игр для отображения.", 
                        "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при получении топа игр: {ex.Message}", 
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
