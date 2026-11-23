
using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Globalization;
using System.Windows.Forms;

namespace MusicLibrary
{
    public partial class AdminForm : Form
    {
        // Conexión compartida con MainForm.
        private readonly MySqlConnection? _connection;

        // DAO que concentra el acceso a los datos de Artists.
        private readonly ArtistsRepository? _artistsRepository;

        // DAO que contiene las consultas SQL de Songs.
        private readonly SongsRepository? _songsRepository;

        // Identificadores de los registros seleccionados.
        private int? _selectedArtistId;
        private int? _selectedSongId;

        // Evitan procesar eventos mientras se cargan datos.
        private bool _loadingArtists;
        private bool _loadingSongs;

        // ==========================================
        // CONSTRUCTORS
        // ==========================================

        public AdminForm()
        {
            InitializeComponent();

            // Conectamos los botones de Artists.
            btnAddArtist.Click -= btnAddArtist_Click;
            btnAddArtist.Click += btnAddArtist_Click;

            btnEditArtist.Click -= btnEditArtist_Click;
            btnEditArtist.Click += btnEditArtist_Click;

            btnDeleteArtist.Click -= btnDeleteArtist_Click;
            btnDeleteArtist.Click += btnDeleteArtist_Click;

            // Conectamos los botones de Songs.
            btnAddSong.Click -= btnAddSong_Click;
            btnAddSong.Click += btnAddSong_Click;

            btnEditSong.Click -= btnEditSong_Click;
            btnEditSong.Click += btnEditSong_Click;

            btnDeleteSong.Click -= btnDeleteSong_Click;
            btnDeleteSong.Click += btnDeleteSong_Click;

            // Detectamos la selección de un artista.
            dgvArtists.SelectionChanged -=
                dgvArtists_SelectionChanged;

            dgvArtists.SelectionChanged +=
                dgvArtists_SelectionChanged;

            // Detectamos la selección de una canción.
            dgvAdminSongs.SelectionChanged -=
                dgvAdminSongs_SelectionChanged;

            dgvAdminSongs.SelectionChanged +=
                dgvAdminSongs_SelectionChanged;

            // Actualizamos los datos al cambiar de pestaña.
            tabAdmin.SelectedIndexChanged +=
                tabAdmin_SelectedIndexChanged;

            // Cargamos los datos cuando aparece la ventana.
            Shown += AdminForm_Shown;
        }

        // Recibimos la conexión del administrador.
        public AdminForm(MySqlConnection connection) : this()
        {
            _connection = connection;
            _artistsRepository = new ArtistsRepository(connection);
            _songsRepository = new SongsRepository(connection);
        }

        // Devuelve el DAO de artistas y comprueba que exista.
        private ArtistsRepository GetArtistsRepository()
        {
            return _artistsRepository ??
                throw new InvalidOperationException(
                    "Artists repository is not available.");
        }

        // Devuelve el DAO de canciones y comprueba que exista.
        private SongsRepository GetSongsRepository()
        {
            return _songsRepository ??
                throw new InvalidOperationException(
                    "Songs repository is not available.");
        }

        // ==========================================
        // DATABASE CONNECTION
        // ==========================================

        // Comprueba que la conexión exista y esté abierta.
        private void EnsureConnection()
        {
            if (_connection == null)
            {
                throw new InvalidOperationException(
                    "Database connection is not available.");
            }

            if (_connection.State != ConnectionState.Open)
            {
                _connection.Open();
            }
        }

        // ==========================================
        // ADMINISTRATOR SECURITY
        // ==========================================

        // Comprueba si el usuario tiene activo rol_admin.
        private bool HasAdminRole()
        {
            EnsureConnection();

            // Activa solamente los roles ya concedidos.
            using (var activateRoles = new MySqlCommand(
                "SET ROLE ALL;", _connection!))
            {
                activateRoles.ExecuteNonQuery();
            }

            using var command = new MySqlCommand(
                "SELECT CURRENT_ROLE();", _connection!);

            string activeRoles =
                Convert.ToString(command.ExecuteScalar()) ?? "";

            // Ejemplo de respuesta:
            // `rol_admin`@`%`
            string[] roles = activeRoles.Split(
                ',',
                StringSplitOptions.RemoveEmptyEntries);

            foreach (string role in roles)
            {
                int atPosition = role.IndexOf('@');

                if (atPosition < 0)
                {
                    continue;
                }

                string roleName = role
                    .Substring(0, atPosition)
                    .Trim()
                    .Trim('`', '\'', '"');

                if (string.Equals(
                    roleName,
                    "rol_admin",
                    StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }

        // ==========================================
        // ADMIN FORM SHOWN
        // ==========================================

        // Este evento carga los artistas y canciones
        // cuando abrimos Admin Panel.
        private void AdminForm_Shown(
            object? sender, EventArgs e)
        {
            try
            {
                EnsureConnection();

                // Verificamos el permiso de administrador.
                if (!HasAdminRole())
                {
                    MessageBox.Show(
                        "Access denied. Administrator role required.",
                        "Admin Panel",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    Close();
                    return;
                }

                // Cargamos los datos de ambas pestañas.
                LoadArtists();
                LoadSongArtists();
                LoadSongs();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Could not load Admin Panel:\n" + ex.Message,
                    "Music Library",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                Close();
            }
        }

        // ==========================================
        // ARTISTS - LOAD
        // ==========================================

        // Obtiene todos los artistas a través del DAO.
        private void LoadArtists()
        {
            DataTable table = GetArtistsRepository().GetArtists();

            _loadingArtists = true;

            try
            {
                // Mostramos los artistas en la tabla.
                dgvArtists.DataSource = table;
                dgvArtists.ClearSelection();
                dgvArtists.CurrentCell = null;

                _selectedArtistId = null;
                ClearArtistFields();
            }
            finally
            {
                _loadingArtists = false;
            }
        }

        // Limpia los campos del artista.
        private void ClearArtistFields()
        {
            txtArtistName.Clear();
            txtCountry.Clear();
            txtGenre.Clear();

            dtpRegistrationDate.Value = DateTime.Today;
        }

        // ==========================================
        // ARTISTS - SELECT
        // ==========================================

        // Llena los TextBoxes al seleccionar un artista.
        private void dgvArtists_SelectionChanged(
            object? sender, EventArgs e)
        {
            if (_loadingArtists)
            {
                return;
            }

            if (dgvArtists.SelectedRows.Count == 0)
            {
                _selectedArtistId = null;
                return;
            }

            DataGridViewRow row = dgvArtists.SelectedRows[0];

            if (row.IsNewRow)
            {
                return;
            }

            try
            {
                _selectedArtistId = Convert.ToInt32(
                    row.Cells["ArtistID"].Value);

                txtArtistName.Text = Convert.ToString(
                    row.Cells["Name"].Value) ?? "";

                txtCountry.Text = Convert.ToString(
                    row.Cells["Country"].Value) ?? "";

                txtGenre.Text = Convert.ToString(
                    row.Cells["Genre"].Value) ?? "";

                object dateValue =
                    row.Cells["RegistrationDate"].Value;

                dtpRegistrationDate.Value =
                    dateValue == DBNull.Value
                    ? DateTime.Today
                    : Convert.ToDateTime(dateValue);
            }
            catch (Exception ex)
            {
                ShowGeneralError(ex);
            }
        }

        // ==========================================
        // ARTISTS - VALIDATION
        // ==========================================

        // Comprueba que los campos obligatorios
        // tengan información.
        private bool ValidateArtist()
        {
            if (string.IsNullOrWhiteSpace(txtArtistName.Text))
            {
                MessageBox.Show("Enter the artist name.");
                txtArtistName.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtCountry.Text))
            {
                MessageBox.Show("Enter the country.");
                txtCountry.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtGenre.Text))
            {
                MessageBox.Show("Enter the music genre.");
                txtGenre.Focus();
                return false;
            }

            return true;
        }

        // ==========================================
        // ARTISTS - ADD
        // ==========================================

        // Agrega un artista mediante ArtistsRepository.
        private void btnAddArtist_Click(
            object? sender, EventArgs e)
        {
            if (!ValidateArtist())
            {
                return;
            }

            try
            {
                GetArtistsRepository().AddArtist(
                    txtArtistName.Text.Trim(),
                    txtCountry.Text.Trim(),
                    txtGenre.Text.Trim(),
                    dtpRegistrationDate.Value.Date);

                MessageBox.Show(
                    "Artist added successfully!",
                    "Music Library");

                RefreshArtistData();
            }
            catch (MySqlException ex)
            {
                ShowDatabaseError(ex, "add artist");
            }
            catch (Exception ex)
            {
                ShowGeneralError(ex);
            }
        }

        // ==========================================
        // ARTISTS - EDIT
        // ==========================================

        // Modifica un artista mediante ArtistsRepository.
        private void btnEditArtist_Click(
            object? sender, EventArgs e)
        {
            if (!_selectedArtistId.HasValue)
            {
                MessageBox.Show(
                    "Please select an artist first.");
                return;
            }

            if (!ValidateArtist())
            {
                return;
            }

            try
            {
                int affectedRows = GetArtistsRepository().UpdateArtist(
                    _selectedArtistId.Value,
                    txtArtistName.Text.Trim(),
                    txtCountry.Text.Trim(),
                    txtGenre.Text.Trim(),
                    dtpRegistrationDate.Value.Date);

                if (affectedRows == 0)
                {
                    MessageBox.Show(
                        "No changes were saved, or the artist no longer exists.",
                        "Music Library");
                }
                else
                {
                    MessageBox.Show(
                        "Artist updated successfully!",
                        "Music Library");
                }

                RefreshArtistData();
            }
            catch (MySqlException ex)
            {
                ShowDatabaseError(ex, "edit artist");
            }
            catch (Exception ex)
            {
                ShowGeneralError(ex);
            }
        }

        // ==========================================
        // ARTISTS - DELETE
        // ==========================================

        // Elimina un artista solamente si no tiene canciones.
        private void btnDeleteArtist_Click(
            object? sender, EventArgs e)
        {
            if (!_selectedArtistId.HasValue)
            {
                MessageBox.Show(
                    "Please select an artist first.");
                return;
            }

            try
            {
                // DAO: comprueba si existen canciones relacionadas.
                long songCount = GetArtistsRepository()
                    .CountSongsByArtist(_selectedArtistId.Value);

                if (songCount > 0)
                {
                    MessageBox.Show(
                        "This artist has registered songs.\n" +
                        "Delete or reassign their songs first.",
                        "Cannot Delete Artist",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    return;
                }

                // Se conserva la confirmación de borrado.
                DialogResult confirmation = MessageBox.Show(
                    "Delete artist '" +
                    txtArtistName.Text.Trim() + "'?",
                    "Confirm Deletion",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                if (confirmation != DialogResult.Yes)
                {
                    return;
                }

                int affectedRows = GetArtistsRepository()
                    .DeleteArtist(_selectedArtistId.Value);

                if (affectedRows > 0)
                {
                    MessageBox.Show(
                        "Artist deleted successfully!",
                        "Music Library");
                }

                RefreshArtistData();
            }
            catch (MySqlException ex)
            {
                // También protege ante restricciones FK de MySQL.
                ShowDatabaseError(ex, "delete artist");
            }
            catch (Exception ex)
            {
                ShowGeneralError(ex);
            }
        }

        // Actualiza todos los datos relacionados.
        private void RefreshArtistData()
        {
            LoadArtists();
            LoadSongArtists();
            LoadSongs();
        }

        // ==========================================
        // SONGS - ARTISTS COMBOBOX
        // ==========================================

        // Solicita al DAO los artistas disponibles para el ComboBox.
        private void LoadSongArtists()
        {
            DataTable table = GetSongsRepository().GetArtistOptions();

            // Name es lo que aparece en el ComboBox.
            // ArtistID es el identificador que guardamos.
            cmbSongArtist.DataSource = null;
            cmbSongArtist.DisplayMember = "Name";
            cmbSongArtist.ValueMember = "ArtistID";
            cmbSongArtist.DataSource = table;

            // No seleccionamos ningún artista automáticamente.
            cmbSongArtist.SelectedIndex = -1;
        }

        // ==========================================
        // SONGS - LOAD
        // ==========================================

        // Recupera el catálogo de canciones desde SongsRepository.
        private void LoadSongs()
        {
            DataTable table = GetSongsRepository().GetSongs();

            _loadingSongs = true;

            try
            {
                // Esta línea muestra los datos de MySQL
                // en el DataGridView de la pestaña Songs.
                dgvAdminSongs.DataSource = table;

                // Ocultamos el ID del artista.
                // Se utiliza internamente para editar.
                if (dgvAdminSongs.Columns["ArtistID"] != null)
                {
                    dgvAdminSongs.Columns["ArtistID"].Visible =
                        false;
                }

                dgvAdminSongs.ClearSelection();
                dgvAdminSongs.CurrentCell = null;

                _selectedSongId = null;
                ClearSongFields();
            }
            finally
            {
                _loadingSongs = false;
            }
        }

        // Limpia los campos de Songs.
        private void ClearSongFields()
        {
            txtSongTitle.Clear();
            txtSongDuration.Clear();
            txtSongGenre.Clear();

            cmbSongArtist.SelectedIndex = -1;
            dtpReleaseDate.Value = DateTime.Today;
        }

        // ==========================================
        // SONGS - SELECT
        // ==========================================

        // Cuando seleccionamos una canción, mostramos
        // sus datos en los campos de la izquierda.
        private void dgvAdminSongs_SelectionChanged(
            object? sender, EventArgs e)
        {
            if (_loadingSongs)
            {
                return;
            }

            if (dgvAdminSongs.SelectedRows.Count == 0)
            {
                _selectedSongId = null;
                return;
            }

            DataGridViewRow row =
                dgvAdminSongs.SelectedRows[0];

            if (row.IsNewRow)
            {
                return;
            }

            try
            {
                // ID de la canción seleccionada.
                _selectedSongId = Convert.ToInt32(
                    row.Cells["SongID"].Value);

                // Título.
                txtSongTitle.Text = Convert.ToString(
                    row.Cells["Title"].Value) ?? "";

                // Artista existente.
                cmbSongArtist.SelectedValue = Convert.ToInt32(
                    row.Cells["ArtistID"].Value);

                // Género.
                txtSongGenre.Text = Convert.ToString(
                    row.Cells["Genre"].Value) ?? "";

                // Convierte segundos al formato HH:MM:SS.
                int seconds = Convert.ToInt32(
                    row.Cells["Duration"].Value);

                TimeSpan duration =
                    TimeSpan.FromSeconds(seconds);

                txtSongDuration.Text =
                    duration.ToString(@"hh\:mm\:ss");

                // Fecha de lanzamiento.
                object dateValue =
                    row.Cells["ReleaseDate"].Value;

                dtpReleaseDate.Value =
                    dateValue == DBNull.Value
                    ? DateTime.Today
                    : Convert.ToDateTime(dateValue);
            }
            catch (Exception ex)
            {
                ShowGeneralError(ex);
            }
        }

        // ==========================================
        // SONGS - VALIDATION
        // ==========================================

        // Valida los campos antes de INSERT o UPDATE.
        // Devuelve el ID del artista y la duración
        // convertida a segundos.
        private bool ValidateSong(
            out int artistId,
            out int durationSeconds)
        {
            artistId = 0;
            durationSeconds = 0;

            // Comprueba el título.
            if (string.IsNullOrWhiteSpace(txtSongTitle.Text))
            {
                MessageBox.Show("Enter the song title.");
                txtSongTitle.Focus();
                return false;
            }

            // Comprueba que hayamos elegido un artista.
            if (cmbSongArtist.SelectedIndex < 0 ||
                cmbSongArtist.SelectedValue == null)
            {
                MessageBox.Show("Select an artist.");
                cmbSongArtist.Focus();
                return false;
            }

            if (!int.TryParse(
                Convert.ToString(cmbSongArtist.SelectedValue),
                out artistId))
            {
                MessageBox.Show("Select a valid artist.");
                return false;
            }

            // Comprueba el género.
            if (string.IsNullOrWhiteSpace(txtSongGenre.Text))
            {
                MessageBox.Show("Enter the music genre.");
                txtSongGenre.Focus();
                return false;
            }

            // Convierte HH:MM:SS a segundos.
            // Ejemplo: 00:03:45 = 225 segundos.
            bool validDuration = TimeSpan.TryParseExact(
                txtSongDuration.Text.Trim(),
                @"hh\:mm\:ss",
                CultureInfo.InvariantCulture,
                out TimeSpan duration);

            if (!validDuration ||
                duration.TotalSeconds <= 0 ||
                duration.TotalSeconds > int.MaxValue)
            {
                MessageBox.Show(
                    "Enter a valid duration in HH:MM:SS format.\n" +
                    "Example: 00:03:45",
                    "Invalid Duration");

                txtSongDuration.Focus();
                return false;
            }

            durationSeconds = (int)duration.TotalSeconds;

            return true;
        }

        // Los parámetros SQL de Songs se preparan ahora dentro
        // de SongsRepository; la validación sigue aquí.

        // ==========================================
        // SONGS - ADD
        // ==========================================

        // INSERT para registrar una canción nueva.
        private void btnAddSong_Click(
            object? sender, EventArgs e)
        {
            if (!ValidateSong(
                out int artistId,
                out int durationSeconds))
            {
                return;
            }

            try
            {
                // La consulta INSERT se ejecuta en SongsRepository.
                GetSongsRepository().AddSong(
                    txtSongTitle.Text.Trim(),
                    artistId,
                    durationSeconds,
                    txtSongGenre.Text.Trim(),
                    dtpReleaseDate.Value.Date);

                MessageBox.Show(
                    "Song added successfully!",
                    "Music Library");

                // Recarga la tabla para mostrar el cambio.
                LoadSongs();
            }
            catch (MySqlException ex)
            {
                ShowDatabaseError(ex, "add song");
            }
            catch (Exception ex)
            {
                ShowGeneralError(ex);
            }
        }

        // ==========================================
        // SONGS - EDIT
        // ==========================================

        // UPDATE para modificar una canción existente.
        private void btnEditSong_Click(
            object? sender, EventArgs e)
        {
            if (!_selectedSongId.HasValue)
            {
                MessageBox.Show(
                    "Please select a song first.");
                return;
            }

            if (!ValidateSong(
                out int artistId,
                out int durationSeconds))
            {
                return;
            }

            try
            {
                // La consulta UPDATE se ejecuta en SongsRepository.
                int affectedRows = GetSongsRepository().UpdateSong(
                    _selectedSongId.Value,
                    txtSongTitle.Text.Trim(),
                    artistId,
                    durationSeconds,
                    txtSongGenre.Text.Trim(),
                    dtpReleaseDate.Value.Date);

                if (affectedRows == 0)
                {
                    MessageBox.Show(
                        "No changes were saved, or the song no longer exists.",
                        "Music Library");
                }
                else
                {
                    MessageBox.Show(
                        "Song updated successfully!",
                        "Music Library");
                }

                LoadSongs();
            }
            catch (MySqlException ex)
            {
                ShowDatabaseError(ex, "edit song");
            }
            catch (Exception ex)
            {
                ShowGeneralError(ex);
            }
        }

        // ==========================================
        // SONGS - DELETE
        // ==========================================

        // DELETE para eliminar una canción.
        private void btnDeleteSong_Click(
            object? sender, EventArgs e)
        {
            if (!_selectedSongId.HasValue)
            {
                MessageBox.Show(
                    "Please select a song first.");
                return;
            }

            try
            {
                // Revisa las relaciones usando el DAO.
                long libraryCount = GetSongsRepository()
                    .CountLibrariesForSong(_selectedSongId.Value);

                if (libraryCount > 0)
                {
                    MessageBox.Show(
                        "This song is saved in a user library.\n" +
                        "Remove those references before deleting it.",
                        "Cannot Delete Song",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    return;
                }

                // Confirmación antes del DELETE.
                DialogResult confirmation = MessageBox.Show(
                    "Delete song '" +
                    txtSongTitle.Text.Trim() + "'?",
                    "Confirm Deletion",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                if (confirmation != DialogResult.Yes)
                {
                    return;
                }

                // El DAO realiza DELETE usando un parámetro seguro.
                int affectedRows = GetSongsRepository()
                    .DeleteSong(_selectedSongId.Value);

                if (affectedRows > 0)
                {
                    MessageBox.Show(
                        "Song deleted successfully!",
                        "Music Library");
                }

                LoadSongs();
            }
            catch (MySqlException ex)
            {
                ShowDatabaseError(ex, "delete song");
            }
            catch (Exception ex)
            {
                ShowGeneralError(ex);
            }
        }

        // ==========================================
        // TAB CHANGED
        // ==========================================

        // Actualiza los datos al cambiar entre pestañas.
        private void tabAdmin_SelectedIndexChanged(
            object? sender, EventArgs e)
        {
            if (_connection == null)
            {
                return;
            }

            try
            {
                // Recarga los artistas.
                if (tabAdmin.SelectedTab == tabArtists)
                {
                    LoadArtists();
                }

                // Recarga canciones y ComboBox de artistas.
                if (tabAdmin.SelectedTab == tabSongs)
                {
                    LoadSongArtists();
                    LoadSongs();
                }
            }
            catch (Exception ex)
            {
                ShowGeneralError(ex);
            }
        }

        // ==========================================
        // DATABASE ERRORS
        // ==========================================

        // Convierte errores de MySQL en mensajes claros.
        private void ShowDatabaseError(
            MySqlException ex,
            string operation)
        {
            string message;

            switch (ex.Number)
            {
                case 1062:
                    message =
                        "A duplicate record already exists.";
                    break;

                // No permite eliminar registros que
                // todavía tienen referencias relacionadas.
                case 1451:
                    if (operation == "delete artist")
                    {
                        message =
                            "This artist has registered songs.\n" +
                            "Delete or reassign their songs first.";
                    }
                    else
                    {
                        message =
                            "This record has related information " +
                            "and cannot be deleted.";
                    }
                    break;

                case 1452:
                    message =
                        "The selected related record does not exist.";
                    break;

                case 1044:
                case 1142:
                    message =
                        "Access denied. You do not have permission " +
                        "to perform this operation.";
                    break;

                default:
                    message =
                        "Could not " + operation + ":\n" +
                        ex.Message;
                    break;
            }

            MessageBox.Show(
                message,
                "Database Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }

        // Maneja los errores generales de C#.
        private void ShowGeneralError(Exception ex)
        {
            MessageBox.Show(
                "Error: " + ex.Message,
                "Music Library",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }
}
