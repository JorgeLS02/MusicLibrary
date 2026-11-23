
namespace MusicLibrary
{
    partial class AdminForm
    {
        // Contenedor de los componentes del formulario.
        private System.ComponentModel.IContainer components = null;

        // Libera los recursos al cerrar la ventana.
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
            // ==========================================
            // TABS
            // ==========================================

            tabAdmin = new TabControl();
            tabArtists = new TabPage();
            tabSongs = new TabPage();

            // ==========================================
            // ARTISTS CONTROLS
            // ==========================================

            lblArtistName = new Label();
            txtArtistName = new TextBox();

            lblCountry = new Label();
            txtCountry = new TextBox();

            lblGenre = new Label();
            txtGenre = new TextBox();

            lblRegistrationDate = new Label();
            dtpRegistrationDate = new DateTimePicker();

            dgvArtists = new DataGridView();

            btnAddArtist = new Button();
            btnEditArtist = new Button();
            btnDeleteArtist = new Button();
            btnReportsArtists = new Button();

            // Nuevo botón de transacciones.
            btnTransactionsArtists = new Button();

            // ==========================================
            // SONGS CONTROLS
            // ==========================================

            lblSongTitle = new Label();
            txtSongTitle = new TextBox();

            lblSongArtist = new Label();
            cmbSongArtist = new ComboBox();

            lblSongDuration = new Label();
            txtSongDuration = new TextBox();

            lblSongGenre = new Label();
            txtSongGenre = new TextBox();

            lblReleaseDate = new Label();
            dtpReleaseDate = new DateTimePicker();

            dgvAdminSongs = new DataGridView();

            btnAddSong = new Button();
            btnEditSong = new Button();
            btnDeleteSong = new Button();
            btnReportsSongs = new Button();

            // Nuevo botón de transacciones.
            btnTransactionsSongs = new Button();

            // ==========================================
            // INITIALIZE LAYOUT
            // ==========================================

            tabAdmin.SuspendLayout();
            tabArtists.SuspendLayout();
            tabSongs.SuspendLayout();

            ((System.ComponentModel.ISupportInitialize)
                dgvArtists).BeginInit();

            ((System.ComponentModel.ISupportInitialize)
                dgvAdminSongs).BeginInit();

            SuspendLayout();

            // ==========================================
            // TAB CONTROL
            // ==========================================

            // Contiene Artists y Songs.
            tabAdmin.Controls.Add(tabArtists);
            tabAdmin.Controls.Add(tabSongs);

            tabAdmin.Dock = DockStyle.Fill;
            tabAdmin.Location = new Point(0, 0);
            tabAdmin.Name = "tabAdmin";
            tabAdmin.SelectedIndex = 0;
            tabAdmin.Size = new Size(800, 450);
            tabAdmin.TabIndex = 0;

            // ==========================================
            // ARTISTS TAB
            // ==========================================

            // Campos de artistas.
            tabArtists.Controls.Add(lblArtistName);
            tabArtists.Controls.Add(txtArtistName);
            tabArtists.Controls.Add(lblCountry);
            tabArtists.Controls.Add(txtCountry);
            tabArtists.Controls.Add(lblGenre);
            tabArtists.Controls.Add(txtGenre);
            tabArtists.Controls.Add(lblRegistrationDate);
            tabArtists.Controls.Add(dtpRegistrationDate);

            // Tabla y botones existentes.
            tabArtists.Controls.Add(dgvArtists);
            tabArtists.Controls.Add(btnAddArtist);
            tabArtists.Controls.Add(btnEditArtist);
            tabArtists.Controls.Add(btnDeleteArtist);
            tabArtists.Controls.Add(btnReportsArtists);

            // Botón nuevo.
            tabArtists.Controls.Add(btnTransactionsArtists);

            tabArtists.Location = new Point(4, 24);
            tabArtists.Name = "tabArtists";
            tabArtists.Padding = new Padding(3);
            tabArtists.Size = new Size(792, 422);
            tabArtists.TabIndex = 0;
            tabArtists.Text = "Artists";
            tabArtists.UseVisualStyleBackColor = true;

            // ------------------------------------------
            // ARTIST NAME
            // ------------------------------------------

            lblArtistName.AutoSize = true;
            lblArtistName.Location = new Point(22, 38);
            lblArtistName.Name = "lblArtistName";
            lblArtistName.Text = "Artist Name";

            txtArtistName.Location = new Point(22, 70);
            txtArtistName.Name = "txtArtistName";
            txtArtistName.Size = new Size(200, 23);
            txtArtistName.TabIndex = 0;

            // ------------------------------------------
            // COUNTRY
            // ------------------------------------------

            lblCountry.AutoSize = true;
            lblCountry.Location = new Point(22, 116);
            lblCountry.Name = "lblCountry";
            lblCountry.Text = "Country";

            txtCountry.Location = new Point(22, 146);
            txtCountry.Name = "txtCountry";
            txtCountry.Size = new Size(200, 23);
            txtCountry.TabIndex = 1;

            // ------------------------------------------
            // ARTIST GENRE
            // ------------------------------------------

            lblGenre.AutoSize = true;
            lblGenre.Location = new Point(22, 190);
            lblGenre.Name = "lblGenre";
            lblGenre.Text = "Genre";

            txtGenre.Location = new Point(22, 227);
            txtGenre.Name = "txtGenre";
            txtGenre.Size = new Size(200, 23);
            txtGenre.TabIndex = 2;

            // ------------------------------------------
            // REGISTRATION DATE
            // ------------------------------------------

            lblRegistrationDate.AutoSize = true;
            lblRegistrationDate.Location = new Point(22, 273);
            lblRegistrationDate.Name = "lblRegistrationDate";
            lblRegistrationDate.Text = "Registration Date";

            dtpRegistrationDate.Format =
                DateTimePickerFormat.Short;

            dtpRegistrationDate.Location =
                new Point(22, 315);

            dtpRegistrationDate.Name =
                "dtpRegistrationDate";

            dtpRegistrationDate.Size = new Size(200, 23);
            dtpRegistrationDate.TabIndex = 3;

            // ------------------------------------------
            // ARTISTS TABLE
            // ------------------------------------------

            dgvArtists.AllowUserToAddRows = false;
            dgvArtists.AllowUserToDeleteRows = false;
            dgvArtists.ReadOnly = true;
            dgvArtists.MultiSelect = false;

            dgvArtists.AutoSizeColumnsMode =
                DataGridViewAutoSizeColumnsMode.Fill;

            dgvArtists.ColumnHeadersHeightSizeMode =
                DataGridViewColumnHeadersHeightSizeMode.AutoSize;

            dgvArtists.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;

            dgvArtists.Location = new Point(260, 19);
            dgvArtists.Name = "dgvArtists";
            dgvArtists.Size = new Size(510, 330);
            dgvArtists.TabIndex = 4;

            // ------------------------------------------
            // ADD ARTIST
            // ------------------------------------------

            btnAddArtist.Location = new Point(22, 365);
            btnAddArtist.Name = "btnAddArtist";
            btnAddArtist.Size = new Size(120, 35);
            btnAddArtist.TabIndex = 5;
            btnAddArtist.Text = "Add Artist";
            btnAddArtist.UseVisualStyleBackColor = true;

            // ------------------------------------------
            // EDIT ARTIST
            // ------------------------------------------

            btnEditArtist.Location = new Point(153, 365);
            btnEditArtist.Name = "btnEditArtist";
            btnEditArtist.Size = new Size(120, 35);
            btnEditArtist.TabIndex = 6;
            btnEditArtist.Text = "Edit Artist";
            btnEditArtist.UseVisualStyleBackColor = true;

            // ------------------------------------------
            // DELETE ARTIST
            // ------------------------------------------

            btnDeleteArtist.Location = new Point(284, 365);
            btnDeleteArtist.Name = "btnDeleteArtist";
            btnDeleteArtist.Size = new Size(120, 35);
            btnDeleteArtist.TabIndex = 7;
            btnDeleteArtist.Text = "Delete Artist";
            btnDeleteArtist.UseVisualStyleBackColor = true;

            // ------------------------------------------
            // REPORTS - ARTISTS
            // ------------------------------------------

            // Abre la ventana de reportes SQL.
            btnReportsArtists.Location = new Point(645, 365);
            btnReportsArtists.Name = "btnReportsArtists";
            btnReportsArtists.Size = new Size(125, 35);
            btnReportsArtists.TabIndex = 8;
            btnReportsArtists.Text = "Reports";
            btnReportsArtists.UseVisualStyleBackColor = true;

            btnReportsArtists.Click += btnReports_Click;

            // ------------------------------------------
            // TRANSACTIONS - ARTISTS
            // ------------------------------------------

            // Abre la demostración de COMMIT y ROLLBACK.
            btnTransactionsArtists.Location =
                new Point(500, 365);

            btnTransactionsArtists.Name =
                "btnTransactionsArtists";

            btnTransactionsArtists.Size =
                new Size(130, 35);

            btnTransactionsArtists.TabIndex = 9;
            btnTransactionsArtists.Text = "Transactions";
            btnTransactionsArtists.UseVisualStyleBackColor =
                true;

            // Conecta con el método que abre la ventana.
            btnTransactionsArtists.Click +=
                btnTransactions_Click;

            // ==========================================
            // SONGS TAB
            // ==========================================

            // Campos de canciones.
            tabSongs.Controls.Add(lblSongTitle);
            tabSongs.Controls.Add(txtSongTitle);
            tabSongs.Controls.Add(lblSongArtist);
            tabSongs.Controls.Add(cmbSongArtist);
            tabSongs.Controls.Add(lblSongDuration);
            tabSongs.Controls.Add(txtSongDuration);
            tabSongs.Controls.Add(lblSongGenre);
            tabSongs.Controls.Add(txtSongGenre);
            tabSongs.Controls.Add(lblReleaseDate);
            tabSongs.Controls.Add(dtpReleaseDate);

            // Tabla y botones existentes.
            tabSongs.Controls.Add(dgvAdminSongs);
            tabSongs.Controls.Add(btnAddSong);
            tabSongs.Controls.Add(btnEditSong);
            tabSongs.Controls.Add(btnDeleteSong);
            tabSongs.Controls.Add(btnReportsSongs);

            // Botón nuevo.
            tabSongs.Controls.Add(btnTransactionsSongs);

            tabSongs.Location = new Point(4, 24);
            tabSongs.Name = "tabSongs";
            tabSongs.Padding = new Padding(3);
            tabSongs.Size = new Size(792, 422);
            tabSongs.TabIndex = 1;
            tabSongs.Text = "Songs";
            tabSongs.UseVisualStyleBackColor = true;

            // ------------------------------------------
            // SONG TITLE
            // ------------------------------------------

            lblSongTitle.AutoSize = true;
            lblSongTitle.Location = new Point(22, 14);
            lblSongTitle.Name = "lblSongTitle";
            lblSongTitle.Text = "Song Title";

            txtSongTitle.Location = new Point(22, 36);
            txtSongTitle.Name = "txtSongTitle";
            txtSongTitle.Size = new Size(200, 23);
            txtSongTitle.TabIndex = 0;

            // ------------------------------------------
            // SONG ARTIST
            // ------------------------------------------

            lblSongArtist.AutoSize = true;
            lblSongArtist.Location = new Point(22, 80);
            lblSongArtist.Name = "lblSongArtist";
            lblSongArtist.Text = "Artist";

            cmbSongArtist.DropDownStyle =
                ComboBoxStyle.DropDownList;

            cmbSongArtist.FormattingEnabled = true;
            cmbSongArtist.Location = new Point(22, 102);
            cmbSongArtist.Name = "cmbSongArtist";
            cmbSongArtist.Size = new Size(200, 23);
            cmbSongArtist.TabIndex = 1;

            // ------------------------------------------
            // SONG DURATION
            // ------------------------------------------

            lblSongDuration.AutoSize = true;
            lblSongDuration.Location = new Point(22, 146);
            lblSongDuration.Name = "lblSongDuration";
            lblSongDuration.Text = "Duration (HH:MM:SS)";

            txtSongDuration.Location = new Point(22, 168);
            txtSongDuration.Name = "txtSongDuration";
            txtSongDuration.Size = new Size(200, 23);
            txtSongDuration.TabIndex = 2;
            txtSongDuration.PlaceholderText = "00:03:45";

            // ------------------------------------------
            // SONG GENRE
            // ------------------------------------------

            lblSongGenre.AutoSize = true;
            lblSongGenre.Location = new Point(22, 212);
            lblSongGenre.Name = "lblSongGenre";
            lblSongGenre.Text = "Genre";

            txtSongGenre.Location = new Point(22, 234);
            txtSongGenre.Name = "txtSongGenre";
            txtSongGenre.Size = new Size(200, 23);
            txtSongGenre.TabIndex = 3;

            // ------------------------------------------
            // RELEASE DATE
            // ------------------------------------------

            lblReleaseDate.AutoSize = true;
            lblReleaseDate.Location = new Point(22, 278);
            lblReleaseDate.Name = "lblReleaseDate";
            lblReleaseDate.Text = "Release Date";

            dtpReleaseDate.Format =
                DateTimePickerFormat.Short;

            dtpReleaseDate.Location = new Point(22, 300);
            dtpReleaseDate.Name = "dtpReleaseDate";
            dtpReleaseDate.Size = new Size(200, 23);
            dtpReleaseDate.TabIndex = 4;

            // ------------------------------------------
            // SONGS TABLE
            // ------------------------------------------

            dgvAdminSongs.AllowUserToAddRows = false;
            dgvAdminSongs.AllowUserToDeleteRows = false;
            dgvAdminSongs.ReadOnly = true;
            dgvAdminSongs.MultiSelect = false;

            dgvAdminSongs.AutoSizeColumnsMode =
                DataGridViewAutoSizeColumnsMode.Fill;

            dgvAdminSongs.ColumnHeadersHeightSizeMode =
                DataGridViewColumnHeadersHeightSizeMode.AutoSize;

            dgvAdminSongs.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;

            dgvAdminSongs.Location = new Point(260, 19);
            dgvAdminSongs.Name = "dgvAdminSongs";
            dgvAdminSongs.Size = new Size(510, 330);
            dgvAdminSongs.TabIndex = 5;

            // ------------------------------------------
            // ADD SONG
            // ------------------------------------------

            btnAddSong.Location = new Point(22, 365);
            btnAddSong.Name = "btnAddSong";
            btnAddSong.Size = new Size(120, 35);
            btnAddSong.TabIndex = 6;
            btnAddSong.Text = "Add Song";
            btnAddSong.UseVisualStyleBackColor = true;

            // ------------------------------------------
            // EDIT SONG
            // ------------------------------------------

            btnEditSong.Location = new Point(153, 365);
            btnEditSong.Name = "btnEditSong";
            btnEditSong.Size = new Size(120, 35);
            btnEditSong.TabIndex = 7;
            btnEditSong.Text = "Edit Song";
            btnEditSong.UseVisualStyleBackColor = true;

            // ------------------------------------------
            // DELETE SONG
            // ------------------------------------------

            btnDeleteSong.Location = new Point(284, 365);
            btnDeleteSong.Name = "btnDeleteSong";
            btnDeleteSong.Size = new Size(120, 35);
            btnDeleteSong.TabIndex = 8;
            btnDeleteSong.Text = "Delete Song";
            btnDeleteSong.UseVisualStyleBackColor = true;

            // ------------------------------------------
            // REPORTS - SONGS
            // ------------------------------------------

            // Abre la misma ventana de reportes.
            btnReportsSongs.Location = new Point(645, 365);
            btnReportsSongs.Name = "btnReportsSongs";
            btnReportsSongs.Size = new Size(125, 35);
            btnReportsSongs.TabIndex = 9;
            btnReportsSongs.Text = "Reports";
            btnReportsSongs.UseVisualStyleBackColor = true;

            btnReportsSongs.Click += btnReports_Click;

            // ------------------------------------------
            // TRANSACTIONS - SONGS
            // ------------------------------------------

            // Abre la demostración de transacciones.
            btnTransactionsSongs.Location =
                new Point(500, 365);

            btnTransactionsSongs.Name =
                "btnTransactionsSongs";

            btnTransactionsSongs.Size =
                new Size(130, 35);

            btnTransactionsSongs.TabIndex = 10;
            btnTransactionsSongs.Text = "Transactions";
            btnTransactionsSongs.UseVisualStyleBackColor =
                true;

            btnTransactionsSongs.Click +=
                btnTransactions_Click;

            // ==========================================
            // ADMIN FORM
            // ==========================================

            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;

            ClientSize = new Size(800, 450);
            Controls.Add(tabAdmin);

            Name = "AdminForm";
            Text = "Admin Panel";
            StartPosition = FormStartPosition.CenterScreen;

            // Finalizamos la configuración.
            tabAdmin.ResumeLayout(false);

            tabArtists.ResumeLayout(false);
            tabArtists.PerformLayout();

            tabSongs.ResumeLayout(false);
            tabSongs.PerformLayout();

            ((System.ComponentModel.ISupportInitialize)
                dgvArtists).EndInit();

            ((System.ComponentModel.ISupportInitialize)
                dgvAdminSongs).EndInit();

            ResumeLayout(false);
        }

        #endregion

        // ==========================================
        // TABS
        // ==========================================

        private TabControl tabAdmin;
        private TabPage tabArtists;
        private TabPage tabSongs;

        // ==========================================
        // ARTISTS CONTROLS
        // ==========================================

        private Label lblArtistName;
        private TextBox txtArtistName;

        private Label lblCountry;
        private TextBox txtCountry;

        private Label lblGenre;
        private TextBox txtGenre;

        private Label lblRegistrationDate;
        private DateTimePicker dtpRegistrationDate;

        private DataGridView dgvArtists;

        private Button btnAddArtist;
        private Button btnEditArtist;
        private Button btnDeleteArtist;
        private Button btnReportsArtists;

        // Nuevo botón.
        private Button btnTransactionsArtists;

        // ==========================================
        // SONGS CONTROLS
        // ==========================================

        private Label lblSongTitle;
        private TextBox txtSongTitle;

        private Label lblSongArtist;
        private ComboBox cmbSongArtist;

        private Label lblSongDuration;
        private TextBox txtSongDuration;

        private Label lblSongGenre;
        private TextBox txtSongGenre;

        private Label lblReleaseDate;
        private DateTimePicker dtpReleaseDate;

        private DataGridView dgvAdminSongs;

        private Button btnAddSong;
        private Button btnEditSong;
        private Button btnDeleteSong;
        private Button btnReportsSongs;

        // Nuevo botón.
        private Button btnTransactionsSongs;
    }
}
