using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using GameApp.Shared;

namespace GameApp.UI
{
    /// <summary>
    /// Главное окно WinForms, реализующее интерфейс IMainView.
    /// </summary>
    public class MainForm : Form, IMainView
    {
        private readonly BindingList<GameViewModel> _games = new();
        private DataGridView _gamesGrid = null!;
        private ComboBox _genreFilter = null!;
        private TextBox _searchBox = null!;
        private NumericUpDown _topCount = null!;
        private Button _addButton = null!;
        private Button _editButton = null!;
        private Button _deleteButton = null!;
        private Button _filterButton = null!;
        private Button _topButton = null!;
        private Button _exportButton = null!;
        private Button _importButton = null!;
        private Label _helpLabel = null!;
        private Panel _helpPanel = null!;
        private Label _topLabel = null!;
        private bool _suppressFilterEvents;

        /// <summary>
        /// Создает главное окно.
        /// </summary>
        public MainForm()
        {
            InitializeComponent();
        }

        /// <inheritdoc/>
        public event EventHandler? LoadRequested;

        /// <inheritdoc/>
        public event EventHandler? AddGameRequested;

        /// <inheritdoc/>
        public event EventHandler<GameEventArgs>? EditGameRequested;

        /// <inheritdoc/>
        public event EventHandler<GameEventArgs>? DeleteGameRequested;

        /// <inheritdoc/>
        public event EventHandler<GenreFilterEventArgs>? FilterRequested;

        /// <inheritdoc/>
        public event EventHandler<TopGamesRequestEventArgs>? TopGamesRequested;

        /// <inheritdoc/>
        public event EventHandler<FilePathEventArgs>? ExportRequested;

        /// <inheritdoc/>
        public event EventHandler<FilePathEventArgs>? ImportRequested;

        /// <inheritdoc/>
        public event EventHandler<string>? SearchRequested;

        /// <inheritdoc/>
        public void ShowGames(IReadOnlyCollection<GameViewModel> games)
        {
            _games.Clear();
            foreach (var game in games)
            {
                _games.Add(game);
            }
        }

        /// <inheritdoc/>
        public void ShowGenres(IReadOnlyCollection<string> genres, string? selectedGenre)
        {
            _suppressFilterEvents = true;
            try
            {
                _genreFilter.Items.Clear();
                _genreFilter.Items.Add("Все жанры");
                foreach (var genre in genres.OrderBy(g => g))
                {
                    _genreFilter.Items.Add(genre);
                }

                if (!string.IsNullOrWhiteSpace(selectedGenre) && _genreFilter.Items.Contains(selectedGenre))
                {
                    _genreFilter.SelectedItem = selectedGenre;
                }
                else
                {
                    _genreFilter.SelectedIndex = _genreFilter.Items.Count > 0 ? 0 : -1;
                }
            }
            finally
            {
                _suppressFilterEvents = false;
            }
        }

        /// <inheritdoc/>
        public GameViewModel? RequestGameData(GameViewModel? source)
        {
            using var dialog = new GameDialog
            {
                GameName = source?.Name ?? string.Empty,
                GameGenre = source?.Genre ?? string.Empty,
                GameRating = source?.Rating ?? 5
            };

            if (dialog.ShowDialog(this) != DialogResult.OK)
            {
                return null;
            }

            return new GameViewModel
            {
                Id = source?.Id ?? 0,
                Name = dialog.GameName.Trim(),
                Genre = dialog.GameGenre.Trim(),
                Rating = dialog.GameRating
            };
        }

        /// <inheritdoc/>
        public void ShowInfo(string message)
        {
            MessageBox.Show(this, message, "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <inheritdoc/>
        public void ShowError(string message)
        {
            MessageBox.Show(this, message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /// <inheritdoc/>
        public bool Confirm(string message, string caption)
        {
            return MessageBox.Show(this, message, caption, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;
        }

        private void InitializeComponent()
        {
            Text = "Каталог игр";
            MinimumSize = new Size(900, 600);
            StartPosition = FormStartPosition.CenterScreen;

            _gamesGrid = new DataGridView
            {
                Dock = DockStyle.Fill,
                AutoGenerateColumns = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                AllowUserToAddRows = false
            };
            _gamesGrid.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "ID", DataPropertyName = nameof(GameViewModel.Id), Width = 60 });
            _gamesGrid.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Название", DataPropertyName = nameof(GameViewModel.Name), AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });
            _gamesGrid.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Жанр", DataPropertyName = nameof(GameViewModel.Genre), Width = 150 });
            _gamesGrid.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Рейтинг",
                DataPropertyName = nameof(GameViewModel.Rating),
                Width = 110,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "0.0" }
            });
            _gamesGrid.DataSource = _games;
            _gamesGrid.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;

            _genreFilter = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Width = 180, Height = 36, Margin = new Padding(5) };
            _genreFilter.SelectedIndexChanged += (_, _) =>
            {
                if (_suppressFilterEvents)
                {
                    return;
                }

                var genre = _genreFilter.SelectedItem?.ToString() ?? string.Empty;
                FilterRequested?.Invoke(this, new GenreFilterEventArgs(genre == "Все жанры" ? string.Empty : genre));
            };

            _searchBox = new TextBox { Width = 220, Height = 36, Margin = new Padding(5), PlaceholderText = "Поиск по названию..." };
            _searchBox.TextChanged += (_, _) => SearchRequested?.Invoke(this, _searchBox.Text);

            _topCount = new NumericUpDown { Minimum = 1, Maximum = 100, Value = 3, Width = 70, Height = 36, Margin = new Padding(5) };

            _addButton = new Button { Text = "Добавить", AutoSize = true, AutoSizeMode = AutoSizeMode.GrowAndShrink, Margin = new Padding(5) };
            _addButton.Click += (_, _) => AddGameRequested?.Invoke(this, EventArgs.Empty);

            _editButton = new Button { Text = "Изменить", AutoSize = true, AutoSizeMode = AutoSizeMode.GrowAndShrink, Margin = new Padding(5) };
            _editButton.Click += (_, _) =>
            {
                var game = GetSelectedGame();
                if (game != null)
                {
                    EditGameRequested?.Invoke(this, new GameEventArgs(game));
                }
                else
                {
                    ShowInfo("Выберите строку для редактирования.");
                }
            };

            _deleteButton = new Button { Text = "Удалить", AutoSize = true, AutoSizeMode = AutoSizeMode.GrowAndShrink, Margin = new Padding(5) };
            _deleteButton.Click += (_, _) =>
            {
                var game = GetSelectedGame();
                if (game != null)
                {
                    DeleteGameRequested?.Invoke(this, new GameEventArgs(game));
                }
                else
                {
                    ShowInfo("Выберите строку для удаления.");
                }
            };

            _filterButton = new Button { Text = "Фильтр жанра", AutoSize = true, AutoSizeMode = AutoSizeMode.GrowAndShrink, Margin = new Padding(5) };
            _filterButton.Click += (_, _) =>
            {
                var genre = _genreFilter.SelectedItem?.ToString() ?? string.Empty;
                FilterRequested?.Invoke(this, new GenreFilterEventArgs(genre == "Все жанры" ? string.Empty : genre));
            };

            _topButton = new Button { Text = "Топ N", AutoSize = true, AutoSizeMode = AutoSizeMode.GrowAndShrink, Margin = new Padding(5) };
            _topButton.Click += (_, _) => TopGamesRequested?.Invoke(this, new TopGamesRequestEventArgs((int)_topCount.Value));

            _exportButton = new Button { Text = "Экспорт JSON", AutoSize = true, AutoSizeMode = AutoSizeMode.GrowAndShrink, Margin = new Padding(5) };
            _exportButton.Click += (_, _) =>
            {
                using var dialog = new SaveFileDialog { Filter = "JSON (*.json)|*.json|Все файлы (*.*)|*.*", FileName = "games_export.json" };
                if (dialog.ShowDialog(this) == DialogResult.OK)
                {
                    ExportRequested?.Invoke(this, new FilePathEventArgs(dialog.FileName));
                }
            };

            _importButton = new Button { Text = "Импорт JSON", AutoSize = true, AutoSizeMode = AutoSizeMode.GrowAndShrink, Margin = new Padding(5) };
            _importButton.Click += (_, _) =>
            {
                using var dialog = new OpenFileDialog { Filter = "JSON (*.json)|*.json|Все файлы (*.*)|*.*" };
                if (dialog.ShowDialog(this) == DialogResult.OK)
                {
                    ImportRequested?.Invoke(this, new FilePathEventArgs(dialog.FileName));
                }
            };

            _helpLabel = new Label
            {
                Text = "Подсказка: используйте поиск по названию, фильтр жанра, импорт/экспорт JSON.",
                AutoSize = true,
                Padding = new Padding(8, 6, 8, 6),
                Dock = DockStyle.Fill
            };
            _helpPanel = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 44,
                Padding = new Padding(4, 0, 4, 0)
            };
            _helpPanel.Controls.Add(_helpLabel);
            _topLabel = new Label { Text = "Топ:", AutoSize = true, TextAlign = ContentAlignment.MiddleCenter, Padding = new Padding(4, 12, 4, 0), Margin = new Padding(5) };

            var buttonsPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Top,
                AutoSize = false,
                Padding = new Padding(8),
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = true,
                AutoScroll = true,
                Height = 110
            };

            buttonsPanel.Controls.Add(_addButton);
            buttonsPanel.Controls.Add(_editButton);
            buttonsPanel.Controls.Add(_deleteButton);
            buttonsPanel.Controls.Add(_filterButton);
            buttonsPanel.Controls.Add(new Panel { Width = 5 });
            buttonsPanel.Controls.Add(_genreFilter);
            buttonsPanel.Controls.Add(_searchBox);
            buttonsPanel.Controls.Add(_topLabel);
            buttonsPanel.Controls.Add(_topCount);
            buttonsPanel.Controls.Add(_topButton);
            buttonsPanel.Controls.Add(_exportButton);
            buttonsPanel.Controls.Add(_importButton);

            Controls.Add(_gamesGrid);
            Controls.Add(buttonsPanel);
            Controls.Add(_helpPanel);

            Load += (_, _) => LoadRequested?.Invoke(this, EventArgs.Empty);
        }

        private GameViewModel? GetSelectedGame()
        {
            return _gamesGrid.CurrentRow?.DataBoundItem as GameViewModel;
        }
    }
}
