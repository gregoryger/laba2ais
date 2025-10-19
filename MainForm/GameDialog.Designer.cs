using System;
using System.Drawing;
using System.Windows.Forms;

namespace GameApp.UI
{
    partial class GameDialog
    {
        private System.ComponentModel.IContainer components = null;
        private TextBox txtName;
        private TextBox txtGenre;
        private NumericUpDown nudRating;
        private Button btnOK;
        private Button btnCancel;
        private Label lblName;
        private Label lblGenre;
        private Label lblRating;

        /// <summary>
        /// Освобождает ресурсы диалога.
        /// </summary>
        /// <param name="disposing">True если управляемые ресурсы должны быть освобождены.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Инициализация компонентов диалога.
        /// </summary>
        private void InitializeComponent()
        {
            txtName = new TextBox();
            txtGenre = new TextBox();
            nudRating = new NumericUpDown();
            btnOK = new Button();
            btnCancel = new Button();
            lblName = new Label();
            lblGenre = new Label();
            lblRating = new Label();
            ((System.ComponentModel.ISupportInitialize)nudRating).BeginInit();
            SuspendLayout();
            // 
            // txtName
            // 
            txtName.Location = new Point(147, 20);
            txtName.Name = "txtName";
            txtName.Size = new Size(181, 39);
            txtName.TabIndex = 1;
            // 
            // txtGenre
            // 
            txtGenre.Location = new Point(147, 60);
            txtGenre.Name = "txtGenre";
            txtGenre.Size = new Size(181, 39);
            txtGenre.TabIndex = 3;
            // 
            // nudRating
            // 
            nudRating.DecimalPlaces = 1;
            nudRating.Increment = new decimal(new int[] { 1, 0, 0, 65536 });
            nudRating.Location = new Point(132, 105);
            nudRating.Maximum = new decimal(new int[] { 10, 0, 0, 0 });
            nudRating.Name = "nudRating";
            nudRating.Size = new Size(74, 39);
            nudRating.TabIndex = 5;
            nudRating.ValueChanged += nudRating_ValueChanged;
            // 
            // btnOK
            // 
            btnOK.Location = new Point(20, 143);
            btnOK.Name = "btnOK";
            btnOK.Size = new Size(80, 48);
            btnOK.TabIndex = 6;
            btnOK.Text = "OK";
            btnOK.Click += btnOK_Click;
            // 
            // btnCancel
            // 
            btnCancel.Location = new Point(220, 156);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(108, 35);
            btnCancel.TabIndex = 7;
            btnCancel.Text = "Отмена";
            btnCancel.Click += btnCancel_Click;
            // 
            // lblName
            // 
            lblName.AutoSize = true;
            lblName.Location = new Point(20, 20);
            lblName.Name = "lblName";
            lblName.Size = new Size(125, 32);
            lblName.TabIndex = 0;
            lblName.Text = "Название:";
            // 
            // lblGenre
            // 
            lblGenre.AutoSize = true;
            lblGenre.Location = new Point(20, 60);
            lblGenre.Name = "lblGenre";
            lblGenre.Size = new Size(80, 32);
            lblGenre.TabIndex = 2;
            lblGenre.Text = "Жанр:";
            // 
            // lblRating
            // 
            lblRating.AutoSize = true;
            lblRating.Location = new Point(20, 100);
            lblRating.Name = "lblRating";
            lblRating.Size = new Size(106, 32);
            lblRating.TabIndex = 4;
            lblRating.Text = "Рейтинг:";
            // 
            // GameDialog
            // 
            AcceptButton = btnOK;
            CancelButton = btnCancel;
            ClientSize = new Size(340, 200);
            Controls.Add(lblName);
            Controls.Add(txtName);
            Controls.Add(lblGenre);
            Controls.Add(txtGenre);
            Controls.Add(lblRating);
            Controls.Add(nudRating);
            Controls.Add(btnOK);
            Controls.Add(btnCancel);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "GameDialog";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Добавить/Редактировать игру";
            ((System.ComponentModel.ISupportInitialize)nudRating).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
    }
}
