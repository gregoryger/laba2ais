using System;
using System.Drawing;
using System.Windows.Forms;

namespace GameApp.UI
{
    /// <summary>
    /// Диалог ввода или редактирования игры.
    /// </summary>
    public class GameDialog : Form
    {
        private TextBox _nameBox = null!;
        private TextBox _genreBox = null!;
        private NumericUpDown _ratingBox = null!;
        private Button _okButton = null!;
        private Button _cancelButton = null!;

        /// <summary>
        /// Создает диалог ввода игры.
        /// </summary>
        public GameDialog()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Название игры.
        /// </summary>
        public string GameName
        {
            get => _nameBox.Text;
            set => _nameBox.Text = value;
        }

        /// <summary>
        /// Жанр игры.
        /// </summary>
        public string GameGenre
        {
            get => _genreBox.Text;
            set => _genreBox.Text = value;
        }

        /// <summary>
        /// Рейтинг игры.
        /// </summary>
        public double GameRating
        {
            get => (double)_ratingBox.Value;
            set => _ratingBox.Value = (decimal)Math.Clamp(value, 0, 10);
        }

        private void InitializeComponent()
        {
            Text = "Игра";
            StartPosition = FormStartPosition.CenterParent;
            ClientSize = new Size(440, 220);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;

            var layout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 4,
                Padding = new Padding(12),
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink
            };
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 120));
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 36));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 36));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 36));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 50));

            var nameLabel = new Label { Text = "Название:", AutoSize = true, Anchor = AnchorStyles.Left };
            _nameBox = new TextBox { Dock = DockStyle.Fill, Height = 28, Margin = new Padding(4) };

            var genreLabel = new Label { Text = "Жанр:", AutoSize = true, Anchor = AnchorStyles.Left };
            _genreBox = new TextBox { Dock = DockStyle.Fill, Height = 28, Margin = new Padding(4) };

            var ratingLabel = new Label { Text = "Рейтинг (0-10):", AutoSize = true, Anchor = AnchorStyles.Left };
            _ratingBox = new NumericUpDown
            {
                Minimum = 0,
                Maximum = 10,
                DecimalPlaces = 1,
                Increment = 0.1M,
                Height = 28,
                Width = 140,
                Margin = new Padding(4),
                Anchor = AnchorStyles.Left
            };

            var buttonsPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.RightToLeft,
                Padding = new Padding(0, 6, 0, 0)
            };
            _okButton = new Button { Text = "ОК", DialogResult = DialogResult.OK, Width = 110, Height = 36, Margin = new Padding(8, 0, 0, 0) };
            _okButton.Click += OnOkClick;
            _cancelButton = new Button { Text = "Отмена", DialogResult = DialogResult.Cancel, Width = 110, Height = 36, Margin = new Padding(8, 0, 0, 0) };
            buttonsPanel.Controls.Add(_cancelButton);
            buttonsPanel.Controls.Add(_okButton);

            layout.Controls.Add(nameLabel, 0, 0);
            layout.Controls.Add(_nameBox, 1, 0);
            layout.Controls.Add(genreLabel, 0, 1);
            layout.Controls.Add(_genreBox, 1, 1);
            layout.Controls.Add(ratingLabel, 0, 2);
            layout.Controls.Add(_ratingBox, 1, 2);
            layout.SetColumnSpan(buttonsPanel, 2);
            layout.Controls.Add(buttonsPanel, 0, 3);

            Controls.Add(layout);
            AcceptButton = _okButton;
            CancelButton = _cancelButton;
        }

        private void OnOkClick(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(GameName))
            {
                MessageBox.Show(this, "Введите название игры.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                DialogResult = DialogResult.None;
                _nameBox.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(GameGenre))
            {
                MessageBox.Show(this, "Введите жанр игры.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                DialogResult = DialogResult.None;
                _genreBox.Focus();
                return;
            }
        }
    }
}
