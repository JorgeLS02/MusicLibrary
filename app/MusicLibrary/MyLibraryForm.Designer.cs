namespace MusicLibrary
{
    partial class MyLibraryForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            dgvMyLibrary = new DataGridView();
            btnRemoveFromLibrary = new Button();
            btnToggleFavorite = new Button();
            nudRating = new NumericUpDown();
            btnRateSong = new Button();
            btnSearch = new Button();
            txtSearch = new TextBox();
            ((System.ComponentModel.ISupportInitialize)dgvMyLibrary).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudRating).BeginInit();
            SuspendLayout();
            // 
            // dgvMyLibrary
            // 
            dgvMyLibrary.AllowUserToAddRows = false;
            dgvMyLibrary.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvMyLibrary.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvMyLibrary.Location = new Point(12, 79);
            dgvMyLibrary.MultiSelect = false;
            dgvMyLibrary.Name = "dgvMyLibrary";
            dgvMyLibrary.ReadOnly = true;
            dgvMyLibrary.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvMyLibrary.Size = new Size(776, 206);
            dgvMyLibrary.TabIndex = 0;
            // 
            // btnRemoveFromLibrary
            // 
            btnRemoveFromLibrary.Location = new Point(24, 399);
            btnRemoveFromLibrary.Name = "btnRemoveFromLibrary";
            btnRemoveFromLibrary.Size = new Size(160, 35);
            btnRemoveFromLibrary.TabIndex = 1;
            btnRemoveFromLibrary.Text = "Remove from Library";
            btnRemoveFromLibrary.UseVisualStyleBackColor = true;
            btnRemoveFromLibrary.Click += btnRemoveFromLibrary_Click;
            // 
            // btnToggleFavorite
            // 
            btnToggleFavorite.Location = new Point(325, 399);
            btnToggleFavorite.Name = "btnToggleFavorite";
            btnToggleFavorite.Size = new Size(150, 35);
            btnToggleFavorite.TabIndex = 2;
            btnToggleFavorite.Text = "Add to Favorite";
            btnToggleFavorite.UseVisualStyleBackColor = true;
            btnToggleFavorite.Click += btnToggleFavorite_Click;
            // 
            // nudRating
            // 
            nudRating.Location = new Point(593, 405);
            nudRating.Maximum = new decimal(new int[] { 5, 0, 0, 0 });
            nudRating.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            nudRating.Name = "nudRating";
            nudRating.Size = new Size(65, 23);
            nudRating.TabIndex = 3;
            nudRating.Value = new decimal(new int[] { 5, 0, 0, 0 });
            // 
            // btnRateSong
            // 
            btnRateSong.Location = new Point(668, 399);
            btnRateSong.Name = "btnRateSong";
            btnRateSong.RightToLeft = RightToLeft.No;
            btnRateSong.Size = new Size(120, 35);
            btnRateSong.TabIndex = 4;
            btnRateSong.Text = "Rate Song";
            btnRateSong.UseVisualStyleBackColor = true;
            btnRateSong.Click += btnRateSong_Click;
            // 
            // btnSearch
            // 
            btnSearch.Location = new Point(482, 30);
            btnSearch.Name = "btnSearch";
            btnSearch.Size = new Size(100, 23);
            btnSearch.TabIndex = 6;
            btnSearch.Text = "Search";
            btnSearch.UseVisualStyleBackColor = true;
            // 
            // txtSearch
            // 
            txtSearch.Location = new Point(208, 30);
            txtSearch.Name = "txtSearch";
            txtSearch.Size = new Size(250, 23);
            txtSearch.TabIndex = 5;
            // 
            // MyLibraryForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(btnSearch);
            Controls.Add(txtSearch);
            Controls.Add(btnRateSong);
            Controls.Add(nudRating);
            Controls.Add(btnToggleFavorite);
            Controls.Add(btnRemoveFromLibrary);
            Controls.Add(dgvMyLibrary);
            Name = "MyLibraryForm";
            Text = "My Library";
            ((System.ComponentModel.ISupportInitialize)dgvMyLibrary).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudRating).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DataGridView dgvMyLibrary;
        private Button btnRemoveFromLibrary;
        private Button btnToggleFavorite;
        private NumericUpDown nudRating;
        private Button btnRateSong;
        private Button btnSearch;
        private TextBox txtSearch;
    }
}