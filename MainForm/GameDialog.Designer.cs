using System;
using System.Drawing;
using System.Windows.Forms;

namespace GameApp.UI
{
    partial class GameDialog
    {
        private System.ComponentModel.IContainer components = null;
        private Label lblName;
        private Label lblGenre;
        private Label lblRating;
        private TextBox txtName;
        private TextBox txtGenre;
        private NumericUpDown nudRating;
        private Button btnOK;
        private Button btnCancel;

        /// <summary>
        /// Освобождает ресурсы диалога.
        /// </summary>
        /// <param name="disposing">True для освобождения управляемых ресурсов.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }

            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Метод для инициализации интерфейса.
        /// </summary>
        private void InitializeComponent()
        {
            lblName = new Label();
            lblGenre = new Label();
            lblRating = new Label();
            txtName = new TextBox();
            txtGenre = new TextBox();
            nudRating = new NumericUpDown();
            btnOK = new Button();
            btnCancel = new Button();
            ((System.ComponentModel.ISupportInitialize)nudRating).BeginInit();
            SuspendLayout();
            // 
            // lblName
            // 
            lblName.AutoSize = true;
            lblName.Location = new Point(20, 20);
            lblName.Name = "lblName";
            lblName.Size = new Size(98, 32);
            lblName.TabIndex = 0;
            lblName.Text = "Название";
            // 
            // lblGenre
            // 
            lblGenre.AutoSize = true;
            lblGenre.Location = new Point(20, 76);
            lblGenre.Name = "lblGenre";
            lblGenre.Size = new Size(74, 32);
            lblGenre.TabIndex = 2;
            lblGenre.Text = "Жанр";
            // 
            // lblRating
            // 
            lblRating.AutoSize = true;
            lblRating.Location = new Point(20, 132);
            lblRating.Name = "lblRating";
            lblRating.Size = new Size(96, 32);
            lblRating.TabIndex = 4;
            lblRating.Text = "Рейтинг";
            // 
            // txtName
            // 
            txtName.Location = new Point(140, 17);
            txtName.Name = "txtName";
            txtName.Size = new Size(320, 39);
            txtName.TabIndex = 1;
            // 
            // txtGenre
            // 
            txtGenre.Location = new Point(140, 73);
            txtGenre.Name = "txtGenre";
            txtGenre.Size = new Size(320, 39);
            txtGenre.TabIndex = 3;
            // 
            // nudRating
            // 
            nudRating.DecimalPlaces = 1;
            nudRating.Increment = new decimal(new int[] { 1, 0, 0, 65536 });
            nudRating.Location = new Point(140, 130);
            nudRating.Maximum = new decimal(new int[] { 10, 0, 0, 0 });
            nudRating.Name = "nudRating";
            nudRating.Size = new Size(150, 39);
            nudRating.TabIndex = 5;
            nudRating.Value = new decimal(new int[] { 5, 0, 0, 0 });
            // 
            // btnOK
            // 
            btnOK.Location = new Point(140, 190);
            btnOK.Name = "btnOK";
            btnOK.Size = new Size(150, 45);
            btnOK.TabIndex = 6;
            btnOK.Text = "Сохранить";
            btnOK.UseVisualStyleBackColor = true;
            btnOK.Click += btnOK_Click;
            // 
            // btnCancel
            // 
            btnCancel.Location = new Point(310, 190);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(150, 45);
            btnCancel.TabIndex = 7;
            btnCancel.Text = "Отмена";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += btnCancel_Click;
            // 
            // GameDialog
            // 
            AutoScaleDimensions = new SizeF(13F, 32F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(482, 255);
            Controls.Add(btnCancel);
            Controls.Add(btnOK);
            Controls.Add(nudRating);
            Controls.Add(txtGenre);
            Controls.Add(txtName);
            Controls.Add(lblRating);
            Controls.Add(lblGenre);
            Controls.Add(lblName);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            AcceptButton = btnOK;
            CancelButton = btnCancel;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "GameDialog";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Игра";
            ((System.ComponentModel.ISupportInitialize)nudRating).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
    }
}
