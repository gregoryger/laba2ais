using System;
using System.Windows.Forms;

namespace GameApp.UI
{
    /// <summary>
    /// Диалоговое окно для добавления и редактирования игр.
    /// </summary>
    public partial class GameDialog : Form
    {
        /// <summary>
        /// Конструктор диалогового окна.
        /// </summary>
        public GameDialog()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Название игры, введённое пользователем.
        /// </summary>
        public string GameName
        {
            get => txtName.Text;
            set => txtName.Text = value;
        }

        /// <summary>
        /// Жанр игры, введённый пользователем.
        /// </summary>
        public string GameGenre
        {
            get => txtGenre.Text;
            set => txtGenre.Text = value;
        }

        /// <summary>
        /// Рейтинг игры (0-10), введённый пользователем.
        /// </summary>
        public double GameRating
        {
            get => (double)nudRating.Value;
            set => nudRating.Value = (decimal)value;
        }

        /// <summary>
        /// Обработка нажатия кнопки "OK". Проверяет корректность введённых данных.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void btnOK_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(GameName))
            {
                MessageBox.Show("Название не может быть пустым.",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtName.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(GameGenre))
            {
                MessageBox.Show("Жанр не может быть пустым.",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtGenre.Focus();
                return;
            }

            DialogResult = DialogResult.OK;
            Close();
        }

        /// <summary>
        /// Обработка нажатия кнопки "Отмена". Закрывает форму без сохранения.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void nudRating_ValueChanged(object sender, EventArgs e)
        {

        }
    }
}
