
namespace MusicLibrary
{
    partial class MainForm
    {
        // Contenedor de componentes visuales.
        private System.ComponentModel.IContainer components = null;

        // Libera los recursos cuando se cierra el formulario.
        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
            {
                components.Dispose();
            }

            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            // Controles que ya teníamos.
            dgvSongs = new DataGridView();
            btnAddToLibrary = new Button();
            btnMyLibrary = new Button();
            txtSearch = new TextBox();
            btnSearch = new Button();
            btnAdminPanel = new Button();

            // Nuevos controles para los filtros.
            grpFilters = new GroupBox();
            lblMinDuration = new Label();
            nudMinDuration = new NumericUpDown();

            lblMaxDuration = new Label();
            nudMaxDuration = new NumericUpDown();

            lblGenreFilter = new Label();
            clbGenres = new CheckedListBox();

            btnApplyFilters = new Button();
            btnClearFilters = new Button();
            btnTestPermissions = new Button();

            ((System.ComponentModel.ISupportInitialize)
                dgvSongs).BeginInit();

            ((System.ComponentModel.ISupportInitialize)
                nudMinDuration).BeginInit();

            ((System.ComponentModel.ISupportInitialize)
                nudMaxDuration).BeginInit();

            grpFilters.SuspendLayout();
            SuspendLayout();

            // ==========================================
            // SEARCH TEXTBOX
            // ==========================================

            // Campo para buscar por título, artista o género.
            txtSearch.Location = new Point(265, 24);
            txtSearch.Name = "txtSearch";
            txtSearch.Size = new Size(370, 25);
            txtSearch.TabIndex = 0;

            // ==========================================
            // SEARCH BUTTON
            // ==========================================

            btnSearch.Location = new Point(650, 24);
            btnSearch.Name = "btnSearch";
            btnSearch.Size = new Size(110, 25);
            btnSearch.TabIndex = 1;
            btnSearch.Text = "Search";
            btnSearch.UseVisualStyleBackColor = true;

            btnSearch.Click += btnSearch_Click;

            // ==========================================
            // FILTERS GROUP
            // ==========================================

            // Agrupa los filtros SQL de duración y género.
            grpFilters.Controls.Add(lblMinDuration);
            grpFilters.Controls.Add(nudMinDuration);

            grpFilters.Controls.Add(lblMaxDuration);
            grpFilters.Controls.Add(nudMaxDuration);

            grpFilters.Controls.Add(lblGenreFilter);
            grpFilters.Controls.Add(clbGenres);

            grpFilters.Controls.Add(btnApplyFilters);
            grpFilters.Controls.Add(btnClearFilters);
            grpFilters.Controls.Add(btnTestPermissions);

            grpFilters.Location = new Point(12, 65);
            grpFilters.Name = "grpFilters";
            grpFilters.Size = new Size(1026, 145);
            grpFilters.TabIndex = 2;
            grpFilters.TabStop = false;
            grpFilters.Text = "Advanced Filters";

            // ==========================================
            // MINIMUM DURATION
            // ==========================================

            // Duración mínima expresada en segundos.
            lblMinDuration.AutoSize = true;
            lblMinDuration.Location = new Point(20, 30);
            lblMinDuration.Name = "lblMinDuration";
            lblMinDuration.Text = "Min Duration (seconds)";

            nudMinDuration.Location = new Point(20, 58);
            nudMinDuration.Name = "nudMinDuration";
            nudMinDuration.Size = new Size(145, 23);
            nudMinDuration.Minimum = 0M;
            nudMinDuration.Maximum = 86400M;
            nudMinDuration.Increment = 10M;
            nudMinDuration.Value = 0M;
            nudMinDuration.TabIndex = 0;

            // ==========================================
            // MAXIMUM DURATION
            // ==========================================

            // Duración máxima expresada en segundos.
            lblMaxDuration.AutoSize = true;
            lblMaxDuration.Location = new Point(190, 30);
            lblMaxDuration.Name = "lblMaxDuration";
            lblMaxDuration.Text = "Max Duration (seconds)";

            nudMaxDuration.Location = new Point(190, 58);
            nudMaxDuration.Name = "nudMaxDuration";
            nudMaxDuration.Size = new Size(145, 23);
            nudMaxDuration.Minimum = 0M;
            nudMaxDuration.Maximum = 86400M;
            nudMaxDuration.Increment = 10M;
            nudMaxDuration.Value = 3600M;
            nudMaxDuration.TabIndex = 1;

            // ==========================================
            // GENRE FILTER
            // ==========================================

            // Permite seleccionar varios géneros musicales.
            lblGenreFilter.AutoSize = true;
            lblGenreFilter.Location = new Point(375, 24);
            lblGenreFilter.Name = "lblGenreFilter";
            lblGenreFilter.Text = "Genres (select one or more)";

            // CheckedListBox:
            // Cada género tiene su propia casilla.
            clbGenres.CheckOnClick = true;
            clbGenres.FormattingEnabled = true;
            clbGenres.IntegralHeight = false;
            clbGenres.Location = new Point(375, 47);
            clbGenres.Name = "clbGenres";
            clbGenres.Size = new Size(280, 85);
            clbGenres.TabIndex = 2;

            // Los géneros se agregarán desde MySQL
            // mediante el código de MainForm.cs.

            // ==========================================
            // APPLY FILTERS BUTTON
            // ==========================================

            // Ejecuta BETWEEN y, si hay géneros
            // seleccionados, también IN.
            btnApplyFilters.Location = new Point(715, 47);
            btnApplyFilters.Name = "btnApplyFilters";
            btnApplyFilters.Size = new Size(125, 38);
            btnApplyFilters.TabIndex = 3;
            btnApplyFilters.Text = "Apply Filters";
            btnApplyFilters.UseVisualStyleBackColor = true;

            btnApplyFilters.Click += btnApplyFilters_Click;

            // ==========================================
            // CLEAR FILTERS BUTTON
            // ==========================================

            // Restablece los filtros y muestra el catálogo.
            btnClearFilters.Location = new Point(858, 47);
            btnClearFilters.Name = "btnClearFilters";
            btnClearFilters.Size = new Size(125, 38);
            btnClearFilters.TabIndex = 4;
            btnClearFilters.Text = "Clear Filters";
            btnClearFilters.UseVisualStyleBackColor = true;

            btnClearFilters.Click += btnClearFilters_Click;

            // ==========================================
            // TEST PERMISSIONS BUTTON
            // ==========================================

            // Comprueba desde C# los permisos reales en MySQL.
            // Cabe debajo de los botones de filtros existentes.
            btnTestPermissions.Location = new Point(715, 96);
            btnTestPermissions.Name = "btnTestPermissions";
            btnTestPermissions.Size = new Size(268, 29);
            btnTestPermissions.TabIndex = 5;
            btnTestPermissions.Text = "Test Permissions";
            btnTestPermissions.UseVisualStyleBackColor = true;
            btnTestPermissions.Click += btnTestPermissions_Click;

            // ==========================================
            // SONGS TABLE
            // ==========================================

            // Tabla de canciones del catálogo.
            dgvSongs.AllowUserToAddRows = false;
            dgvSongs.AllowUserToDeleteRows = false;
            dgvSongs.ReadOnly = true;
            dgvSongs.MultiSelect = false;

            dgvSongs.AutoSizeColumnsMode =
                DataGridViewAutoSizeColumnsMode.Fill;

            dgvSongs.ColumnHeadersHeightSizeMode =
                DataGridViewColumnHeadersHeightSizeMode.AutoSize;

            dgvSongs.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;

            dgvSongs.Location = new Point(12, 223);
            dgvSongs.Name = "dgvSongs";
            dgvSongs.Size = new Size(1026, 390);
            dgvSongs.TabIndex = 3;

            // ==========================================
            // ADD TO LIBRARY
            // ==========================================

            // Conservamos la función existente.
            btnAddToLibrary.Location = new Point(12, 637);
            btnAddToLibrary.Name = "btnAddToLibrary";
            btnAddToLibrary.Size = new Size(140, 38);
            btnAddToLibrary.TabIndex = 4;
            btnAddToLibrary.Text = "Add to Library";
            btnAddToLibrary.UseVisualStyleBackColor = true;

            btnAddToLibrary.Click += btnAddToLibrary_Click;

            // ==========================================
            // MY LIBRARY
            // ==========================================

            // Conservamos la biblioteca personal.
            btnMyLibrary.Location = new Point(445, 637);
            btnMyLibrary.Name = "btnMyLibrary";
            btnMyLibrary.Size = new Size(140, 38);
            btnMyLibrary.TabIndex = 5;
            btnMyLibrary.Text = "My Library";
            btnMyLibrary.UseVisualStyleBackColor = true;

            btnMyLibrary.Click += btnMyLibrary_Click;

            // ==========================================
            // ADMIN PANEL
            // ==========================================

            // Conservamos el acceso administrativo.
            btnAdminPanel.Location = new Point(898, 637);
            btnAdminPanel.Name = "btnAdminPanel";
            btnAdminPanel.Size = new Size(140, 38);
            btnAdminPanel.TabIndex = 6;
            btnAdminPanel.Text = "Admin Panel";
            btnAdminPanel.UseVisualStyleBackColor = true;

            // El botón solamente se mostrará si
            // el usuario tiene activo el rol rol_admin.
            btnAdminPanel.Visible = false;

            btnAdminPanel.Click += btnAdminPanel_Click;

            // ==========================================
            // MAIN FORM
            // ==========================================

            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;

            ClientSize = new Size(1050, 692);

            Controls.Add(txtSearch);
            Controls.Add(btnSearch);
            Controls.Add(grpFilters);
            Controls.Add(dgvSongs);
            Controls.Add(btnAddToLibrary);
            Controls.Add(btnMyLibrary);
            Controls.Add(btnAdminPanel);

            Name = "MainForm";
            Text = "Music Library";
            StartPosition = FormStartPosition.CenterScreen;

            Load += MainForm_Load;

            grpFilters.ResumeLayout(false);
            grpFilters.PerformLayout();

            ((System.ComponentModel.ISupportInitialize)
                dgvSongs).EndInit();

            ((System.ComponentModel.ISupportInitialize)
                nudMinDuration).EndInit();

            ((System.ComponentModel.ISupportInitialize)
                nudMaxDuration).EndInit();

            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        // ==========================================
        // EXISTING CONTROLS
        // ==========================================

        private DataGridView dgvSongs;

        private Button btnAddToLibrary;
        private Button btnMyLibrary;
        private Button btnSearch;
        private Button btnAdminPanel;

        private TextBox txtSearch;

        // ==========================================
        // NEW FILTER CONTROLS
        // ==========================================

        private GroupBox grpFilters;

        private Label lblMinDuration;
        private NumericUpDown nudMinDuration;

        private Label lblMaxDuration;
        private NumericUpDown nudMaxDuration;

        private Label lblGenreFilter;
        private CheckedListBox clbGenres;

        private Button btnApplyFilters;
        private Button btnClearFilters;
        private Button btnTestPermissions;
    }
}
