using System;
using System.Windows.Forms;

namespace GameApp.UI
{
    /// <summary>
    /// Диалоговое окно для ввода и редактирования информации об игре.
    /// </summary>
    public partial class GameDialog : Form
    {
        /// <summary>
        /// Создаёт новый диалог.
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
            get => txtName.Text;
            set => txtName.Text = value;
        }

        /// <summary>
        /// Жанр игры.
        /// </summary>
        public string GameGenre
        {
            get => txtGenre.Text;
            set => txtGenre.Text = value;
        }

        /// <summary>
        /// Рейтинг от 0 до 10.
        /// </summary>
        public double GameRating
        {
            get => (double)nudRating.Value;
            set => nudRating.Value = (decimal)value;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(GameName))
            {
                MessageBox.Show("Название игры обязательно.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtName.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(GameGenre))
            {
                MessageBox.Show("Укажите жанр игры.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtGenre.Focus();
                return;
            }

            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
