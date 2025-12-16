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
        private TableLayoutPanel layout;
        private Label nameLabel;
        private Label genreLabel;
        private Label ratingLabel;
        private FlowLayoutPanel buttonsPanel;
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
            layout = new TableLayoutPanel();
            nameLabel = new Label();
            _nameBox = new TextBox();
            genreLabel = new Label();
            _genreBox = new TextBox();
            ratingLabel = new Label();
            _ratingBox = new NumericUpDown();
            buttonsPanel = new FlowLayoutPanel();
            _cancelButton = new Button();
            _okButton = new Button();
            layout.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)_ratingBox).BeginInit();
            buttonsPanel.SuspendLayout();
            SuspendLayout();
            // 
            // layout
            // 
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 120F));
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            layout.Controls.Add(nameLabel, 0, 0);
            layout.Controls.Add(_nameBox, 1, 0);
            layout.Controls.Add(genreLabel, 0, 1);
            layout.Controls.Add(_genreBox, 1, 1);
            layout.Controls.Add(ratingLabel, 0, 2);
            layout.Controls.Add(_ratingBox, 1, 2);
            layout.Controls.Add(buttonsPanel, 0, 3);
            layout.Location = new Point(0, 0);
            layout.Name = "layout";
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 36F));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 36F));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 36F));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
            layout.Size = new Size(200, 100);
            layout.TabIndex = 0;
            // 
            // nameLabel
            // 
            nameLabel.Location = new Point(3, 0);
            nameLabel.Name = "nameLabel";
            nameLabel.Size = new Size(100, 23);
            nameLabel.TabIndex = 0;
            // 
            // _nameBox
            // 
            _nameBox.Location = new Point(123, 3);
            _nameBox.Name = "_nameBox";
            _nameBox.Size = new Size(74, 39);
            _nameBox.TabIndex = 1;
            // 
            // genreLabel
            // 
            genreLabel.Location = new Point(3, 36);
            genreLabel.Name = "genreLabel";
            genreLabel.Size = new Size(100, 23);
            genreLabel.TabIndex = 2;
            // 
            // _genreBox
            // 
            _genreBox.Location = new Point(123, 39);
            _genreBox.Name = "_genreBox";
            _genreBox.Size = new Size(74, 39);
            _genreBox.TabIndex = 3;
            // 
            // ratingLabel
            // 
            ratingLabel.Location = new Point(3, 72);
            ratingLabel.Name = "ratingLabel";
            ratingLabel.Size = new Size(100, 23);
            ratingLabel.TabIndex = 4;
            // 
            // _ratingBox
            // 
            _ratingBox.Location = new Point(123, 75);
            _ratingBox.Name = "_ratingBox";
            _ratingBox.Size = new Size(74, 39);
            _ratingBox.TabIndex = 5;
            // 
            // buttonsPanel
            // 
            layout.SetColumnSpan(buttonsPanel, 2);
            buttonsPanel.Controls.Add(_cancelButton);
            buttonsPanel.Controls.Add(_okButton);
            buttonsPanel.Location = new Point(3, 111);
            buttonsPanel.Name = "buttonsPanel";
            buttonsPanel.Size = new Size(194, 44);
            buttonsPanel.TabIndex = 6;
            // 
            // _cancelButton
            // 
            _cancelButton.Location = new Point(3, 3);
            _cancelButton.Name = "_cancelButton";
            _cancelButton.Size = new Size(75, 23);
            _cancelButton.TabIndex = 0;
            // 
            // _okButton
            // 
            _okButton.Location = new Point(84, 3);
            _okButton.Name = "_okButton";
            _okButton.Size = new Size(75, 23);
            _okButton.TabIndex = 1;
            _okButton.Click += OnOkClick;
            // 
            // GameDialog
            // 
            AcceptButton = _okButton;
            CancelButton = _cancelButton;
            ClientSize = new Size(440, 220);
            Controls.Add(layout);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "GameDialog";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Игра";
            layout.ResumeLayout(false);
            layout.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)_ratingBox).EndInit();
            buttonsPanel.ResumeLayout(false);
            ResumeLayout(false);
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
