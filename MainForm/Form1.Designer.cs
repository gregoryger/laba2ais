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
        private Button btnExport;
        private Panel bottomPanel;

        /// <summary>
        /// Освобождает ресурсы, используемые формой.
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
        /// Метод для инициализации элементов интерфейса.
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
            btnExport = new Button();
            bottomPanel = new Panel();
            ((System.ComponentModel.ISupportInitialize)dgvGames).BeginInit();
            bottomPanel.SuspendLayout();
            SuspendLayout();
            // 
            // dgvGames
            // 
            dgvGames.AllowUserToAddRows = false;
            dgvGames.AllowUserToDeleteRows = false;
            dgvGames.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvGames.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvGames.Dock = DockStyle.Fill;
            dgvGames.Location = new Point(0, 0);
            dgvGames.MultiSelect = false;
            dgvGames.Name = "dgvGames";
            dgvGames.ReadOnly = true;
            dgvGames.RowHeadersWidth = 62;
            dgvGames.RowTemplate.Height = 33;
            dgvGames.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvGames.Size = new Size(1102, 569);
            dgvGames.TabIndex = 0;
            // 
            // cbFilterGenre
            // 
            cbFilterGenre.DropDownStyle = ComboBoxStyle.DropDownList;
            cbFilterGenre.Location = new Point(10, 14);
            cbFilterGenre.Name = "cbFilterGenre";
            cbFilterGenre.Size = new Size(200, 40);
            cbFilterGenre.TabIndex = 0;
            // 
            // btnAdd
            // 
            btnAdd.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnAdd.Location = new Point(588, 12);
            btnAdd.Name = "btnAdd";
            btnAdd.Size = new Size(160, 40);
            btnAdd.TabIndex = 4;
            btnAdd.Text = "Добавить";
            btnAdd.UseVisualStyleBackColor = true;
            btnAdd.Click += btnAdd_Click;
            // 
            // btnEdit
            // 
            btnEdit.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnEdit.Location = new Point(754, 12);
            btnEdit.Name = "btnEdit";
            btnEdit.Size = new Size(140, 40);
            btnEdit.TabIndex = 5;
            btnEdit.Text = "Изменить";
            btnEdit.UseVisualStyleBackColor = true;
            btnEdit.Click += btnEdit_Click;
            // 
            // btnDelete
            // 
            btnDelete.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnDelete.Location = new Point(900, 12);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new Size(160, 40);
            btnDelete.TabIndex = 6;
            btnDelete.Text = "Удалить";
            btnDelete.UseVisualStyleBackColor = true;
            btnDelete.Click += btnDelete_Click;
            // 
            // btnFilter
            // 
            btnFilter.Location = new Point(216, 14);
            btnFilter.Name = "btnFilter";
            btnFilter.Size = new Size(150, 40);
            btnFilter.TabIndex = 1;
            btnFilter.Text = "Фильтр";
            btnFilter.UseVisualStyleBackColor = true;
            btnFilter.Click += btnFilter_Click;
            // 
            // btnTopRated
            // 
            btnTopRated.Location = new Point(372, 14);
            btnTopRated.Name = "btnTopRated";
            btnTopRated.Size = new Size(120, 40);
            btnTopRated.TabIndex = 2;
            btnTopRated.Text = "Топ-3";
            btnTopRated.UseVisualStyleBackColor = true;
            btnTopRated.Click += btnTopRated_Click;
            // 
            // btnExport
            // 
            btnExport.Location = new Point(498, 14);
            btnExport.Name = "btnExport";
            btnExport.Size = new Size(84, 40);
            btnExport.TabIndex = 3;
            btnExport.Text = "JSON";
            btnExport.UseVisualStyleBackColor = true;
            btnExport.Click += btnExport_Click;
            // 
            // bottomPanel
            // 
            bottomPanel.Controls.Add(btnExport);
            bottomPanel.Controls.Add(cbFilterGenre);
            bottomPanel.Controls.Add(btnFilter);
            bottomPanel.Controls.Add(btnTopRated);
            bottomPanel.Controls.Add(btnAdd);
            bottomPanel.Controls.Add(btnEdit);
            bottomPanel.Controls.Add(btnDelete);
            bottomPanel.Dock = DockStyle.Bottom;
            bottomPanel.Location = new Point(0, 569);
            bottomPanel.Name = "bottomPanel";
            bottomPanel.Padding = new Padding(10);
            bottomPanel.Size = new Size(1102, 68);
            bottomPanel.TabIndex = 1;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(13F, 32F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1102, 637);
            Controls.Add(dgvGames);
            Controls.Add(bottomPanel);
            MinimumSize = new Size(900, 550);
            Name = "MainForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "GameApp - Каталог игр";
            ((System.ComponentModel.ISupportInitialize)dgvGames).EndInit();
            bottomPanel.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion
    }
}
