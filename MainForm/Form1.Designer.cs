using System;
using System.Drawing;
using System.Windows.Forms;

namespace GameApp.UI
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;
        private DataGridView dgvGames;
        private ComboBox cbFilterGenre;
        private Button btnAdd;
        private Button btnEdit;
        private Button btnDelete;
        private Button btnFilter;
        private Button btnTopRated;
        private Panel bottomPanel;

        /// <summary>
        /// Освобождает ресурсы формы.
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
        /// Инициализация компонентов формы.
        /// </summary>
        private void InitializeComponent()
        {
            dgvGames = new DataGridView();
            cbFilterGenre = new ComboBox();
            btnAdd = new Button();
            btnEdit = new Button();
            btnDelete = new Button();
            btnFilter = new Button();
            btnTopRated = new Button();
            bottomPanel = new Panel();
            ((System.ComponentModel.ISupportInitialize)dgvGames).BeginInit();
            bottomPanel.SuspendLayout();
            SuspendLayout();
            // 
            // dgvGames
            // 
            dgvGames.AllowUserToAddRows = false;
            dgvGames.AllowUserToDeleteRows = false;
            dgvGames.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvGames.Dock = DockStyle.Fill;
            dgvGames.Location = new Point(0, 0);
            dgvGames.MultiSelect = false;
            dgvGames.Name = "dgvGames";
            dgvGames.ReadOnly = true;
            dgvGames.RowHeadersWidth = 82;
            dgvGames.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvGames.Size = new Size(1021, 491);
            dgvGames.TabIndex = 0;
            // 
            // cbFilterGenre
            // 
            cbFilterGenre.DropDownStyle = ComboBoxStyle.DropDownList;
            cbFilterGenre.Location = new Point(10, 15);
            cbFilterGenre.Name = "cbFilterGenre";
            cbFilterGenre.Size = new Size(150, 40);
            cbFilterGenre.TabIndex = 0;
            // 
            // btnAdd
            // 
            btnAdd.Location = new Point(511, 5);
            btnAdd.Name = "btnAdd";
            btnAdd.Size = new Size(131, 41);
            btnAdd.TabIndex = 3;
            btnAdd.Text = "Добавить";
            btnAdd.Click += btnAdd_Click;
            // 
            // btnEdit
            // 
            btnEdit.Location = new Point(648, 6);
            btnEdit.Name = "btnEdit";
            btnEdit.Size = new Size(189, 40);
            btnEdit.TabIndex = 4;
            btnEdit.Text = "Редактировать";
            btnEdit.Click += btnEdit_Click;
            // 
            // btnDelete
            // 
            btnDelete.Location = new Point(856, 6);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new Size(153, 40);
            btnDelete.TabIndex = 5;
            btnDelete.Text = "Удалить";
            btnDelete.Click += btnDelete_Click;
            // 
            // btnFilter
            // 
            btnFilter.Location = new Point(240, 8);
            btnFilter.Name = "btnFilter";
            btnFilter.Size = new Size(138, 40);
            btnFilter.TabIndex = 1;
            btnFilter.Text = "Фильтр";
            btnFilter.Click += btnFilter_Click;
            // 
            // btnTopRated
            // 
            btnTopRated.Location = new Point(384, 7);
            btnTopRated.Name = "btnTopRated";
            btnTopRated.Size = new Size(121, 40);
            btnTopRated.TabIndex = 2;
            btnTopRated.Text = "Топ 3";
            btnTopRated.Click += btnTopRated_Click;
            // 
            // bottomPanel
            // 
            bottomPanel.Controls.Add(cbFilterGenre);
            bottomPanel.Controls.Add(btnFilter);
            bottomPanel.Controls.Add(btnTopRated);
            bottomPanel.Controls.Add(btnAdd);
            bottomPanel.Controls.Add(btnEdit);
            bottomPanel.Controls.Add(btnDelete);
            bottomPanel.Dock = DockStyle.Bottom;
            bottomPanel.Location = new Point(0, 491);
            bottomPanel.Name = "bottomPanel";
            bottomPanel.Padding = new Padding(10);
            bottomPanel.Size = new Size(1021, 60);
            bottomPanel.TabIndex = 1;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(13F, 32F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1021, 551);
            Controls.Add(dgvGames);
            Controls.Add(bottomPanel);
            MinimumSize = new Size(800, 500);
            Name = "MainForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "GameApp - Управление играми";
            ((System.ComponentModel.ISupportInitialize)dgvGames).EndInit();
            bottomPanel.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion
    }
}
