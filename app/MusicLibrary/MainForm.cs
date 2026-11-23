
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace MusicLibrary
{
    public partial class MainForm : Form
    {
        // Conexión MySQL recibida desde el Login.
        private MySqlConnection? _connection;

        // Indica si el usuario activó los filtros.
        private bool _filtersActive = false;

        // ==========================================
        // CONSTRUCTORS
        // ==========================================

        // Constructor utilizado por Visual Studio.
        public MainForm()
        {
            InitializeComponent();

            // Texto de ayuda del buscador.
            txtSearch.PlaceholderText =
                "Search songs, artists or genres...";

            // El botón de administrador inicia oculto.
            btnAdminPanel.Visible = false;

            // Cuando la ventana ya esté visible,
            // comprobaremos los roles de MySQL.
            Shown += MainForm_Shown;
        }

        // Constructor utilizado después del Login.
        public MainForm(MySqlConnection connection) : this()
        {
            // Guardamos la conexión del usuario.
            _connection = connection;

            // Cargamos los géneros disponibles.
            LoadGenres();

            // Mostramos el catálogo de canciones.
            LoadSongs();
        }

        // ==========================================
        // DATABASE CONNECTION
        // ==========================================

        // Comprueba que MySQL esté conectado.
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
        // LOAD GENRES
        // ==========================================

        // Obtiene todos los géneros existentes en MySQL.
        //
        // No escribimos los géneros manualmente:
        // se cargan automáticamente desde canciones.
        private void LoadGenres()
        {
            if (_connection == null)
            {
                return;
            }

            try
            {
                EnsureConnection();

                // Guardamos las selecciones actuales para
                // mantenerlas al actualizar el catálogo.
                HashSet<string> selectedGenres =
                    new HashSet<string>(
                        StringComparer.OrdinalIgnoreCase);

                foreach (object item in clbGenres.CheckedItems)
                {
                    selectedGenres.Add(
                        Convert.ToString(item) ?? "");
                }

                // DISTINCT evita repetir géneros.
                string query = @"
                    SELECT DISTINCT genero
                    FROM canciones
                    WHERE genero IS NOT NULL
                      AND TRIM(genero) <> ''
                    ORDER BY genero;";

                using var command = new MySqlCommand(
                    query, _connection);

                using var reader = command.ExecuteReader();

                List<string> genres = new List<string>();

                // Leemos todos los géneros de la consulta.
                while (reader.Read())
                {
                    genres.Add(reader.GetString(0));
                }

                // Actualizamos la lista visual.
                clbGenres.Items.Clear();

                foreach (string genre in genres)
                {
                    // Mantiene seleccionado un género
                    // si el usuario ya lo había marcado.
                    bool wasChecked =
                        selectedGenres.Contains(genre);

                    clbGenres.Items.Add(
                        genre, wasChecked);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Could not load genres: " + ex.Message,
                    "Music Library",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        // ==========================================
        // LOAD SONGS
        // ==========================================

        // Carga el catálogo de canciones.
        //
        // Si no hay filtros activos:
        // muestra todas las canciones.
        //
        // Si hay filtros activos:
        // utiliza BETWEEN para duración y
        // IN para los géneros seleccionados.
        private void LoadSongs(string search = "")
        {
            if (_connection == null)
            {
                MessageBox.Show(
                    "Database connection is not available.");
                return;
            }

            try
            {
                EnsureConnection();

                // StringBuilder permite construir la
                // consulta agregando filtros opcionales.
                //
                // El texto SQL es fijo y los valores
                // introducidos por el usuario se envían
                // exclusivamente como parámetros.
                StringBuilder query = new StringBuilder(@"
                    SELECT
                        c.id_cancion AS SongID,
                        c.titulo AS Title,
                        a.nombre AS Artist,
                        c.genero AS Genre,
                        c.duracion AS Duration
                    FROM canciones AS c
                    INNER JOIN artistas AS a
                        ON c.id_artista = a.id_artista
                    WHERE
                    (
                        c.titulo LIKE @search
                        OR a.nombre LIKE @search
                        OR c.genero LIKE @search
                    )");

                // Guardamos los géneros seleccionados.
                List<string> selectedGenres =
                    new List<string>();

                // Si están activados los filtros avanzados:
                if (_filtersActive)
                {
                    // ==================================
                    // BETWEEN
                    // ==================================

                    // Busca canciones cuya duración esté
                    // entre los dos valores indicados.
                    query.Append(@"
                        AND c.duracion
                        BETWEEN @minDuration AND @maxDuration");

                    // ==================================
                    // IN
                    // ==================================

                    // Recorremos las casillas marcadas.
                    foreach (object item in clbGenres.CheckedItems)
                    {
                        string genre =
                            Convert.ToString(item) ?? "";

                        selectedGenres.Add(genre);
                    }

                    // IN solo se agrega cuando el usuario
                    // selecciona uno o más géneros.
                    //
                    // Si no marca ninguno, se filtra
                    // únicamente por duración.
                    if (selectedGenres.Count > 0)
                    {
                        List<string> parameterNames =
                            new List<string>();

                        // Cada género recibe su propio
                        // parámetro SQL.
                        for (int i = 0;
                             i < selectedGenres.Count;
                             i++)
                        {
                            parameterNames.Add(
                                "@genre" + i);
                        }

                        // Ejemplo generado:
                        // AND c.genero IN (@genre0, @genre1)
                        query.Append(
                            " AND c.genero IN (" +
                            string.Join(", ", parameterNames) +
                            ")");
                    }
                }

                // Ordenamos las canciones por título.
                query.Append(" ORDER BY c.titulo;");

                // Creamos el comando SQL.
                using var command = new MySqlCommand(
                    query.ToString(),
                    _connection);

                // ==================================
                // SEARCH PARAMETER
                // ==================================

                // LIKE permite buscar coincidencias
                // parciales en título, artista o género.
                command.Parameters.Add(
                    "@search",
                    MySqlDbType.VarChar
                ).Value = "%" + search + "%";

                // ==================================
                // FILTER PARAMETERS
                // ==================================

                if (_filtersActive)
                {
                    // Valor mínimo de duración.
                    command.Parameters.Add(
                        "@minDuration",
                        MySqlDbType.Int32
                    ).Value = Convert.ToInt32(
                        nudMinDuration.Value);

                    // Valor máximo de duración.
                    command.Parameters.Add(
                        "@maxDuration",
                        MySqlDbType.Int32
                    ).Value = Convert.ToInt32(
                        nudMaxDuration.Value);

                    // Parámetros individuales de IN.
                    for (int i = 0;
                         i < selectedGenres.Count;
                         i++)
                    {
                        command.Parameters.Add(
                            "@genre" + i,
                            MySqlDbType.VarChar
                        ).Value = selectedGenres[i];
                    }
                }

                // Ejecutamos la consulta con ADO.NET.
                using var adapter =
                    new MySqlDataAdapter(command);

                DataTable table = new DataTable();

                adapter.Fill(table);

                // Muestra las canciones en el DataGridView.
                dgvSongs.DataSource = table;

                // Quitamos la selección automática.
                dgvSongs.ClearSelection();
                dgvSongs.CurrentCell = null;
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(
                    "Error loading songs: " + ex.Message,
                    "Music Library");
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error: " + ex.Message,
                    "Music Library");
            }
        }

        // ==========================================
        // SEARCH BUTTON
        // ==========================================

        // Busca canciones usando el texto escrito.
        //
        // Si hay filtros activados, también los aplica.
        private void btnSearch_Click(
            object sender, EventArgs e)
        {
            LoadSongs(txtSearch.Text.Trim());
        }

        // ==========================================
        // APPLY FILTERS
        // ==========================================

        // Se ejecuta cuando presionamos Apply Filters.
        private void btnApplyFilters_Click(
            object sender, EventArgs e)
        {
            // BETWEEN requiere que el mínimo
            // no sea mayor que el máximo.
            if (nudMinDuration.Value > nudMaxDuration.Value)
            {
                MessageBox.Show(
                    "Minimum duration cannot be greater " +
                    "than maximum duration.",
                    "Invalid Range",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            // Activamos los filtros avanzados.
            _filtersActive = true;

            // Ejecuta la consulta con BETWEEN e IN
            // según los géneros seleccionados.
            LoadSongs(txtSearch.Text.Trim());
        }

        // ==========================================
        // CLEAR FILTERS
        // ==========================================

        // Restaura los valores iniciales y
        // vuelve a mostrar todo el catálogo.
        private void btnClearFilters_Click(
            object sender, EventArgs e)
        {
            // Desactiva las condiciones avanzadas.
            _filtersActive = false;

            // Limpia el buscador.
            txtSearch.Clear();

            // Restaura el rango original.
            nudMinDuration.Value = 0M;
            nudMaxDuration.Value = 3600M;

            // Desmarca todos los géneros.
            for (int i = 0;
                 i < clbGenres.Items.Count;
                 i++)
            {
                clbGenres.SetItemChecked(i, false);
            }

            // Muestra todas las canciones nuevamente.
            LoadSongs();
        }

        // ==========================================
        // ADD TO LIBRARY
        // ==========================================

        // Agrega una canción a la biblioteca personal
        // mediante un procedimiento almacenado.
        private void btnAddToLibrary_Click(
            object sender, EventArgs e)
        {
            if (_connection == null)
            {
                MessageBox.Show(
                    "Database connection is not available.");
                return;
            }

            // Verificamos que el usuario haya seleccionado
            // una canción de la tabla.
            if (dgvSongs.CurrentRow == null ||
                dgvSongs.CurrentRow.IsNewRow)
            {
                MessageBox.Show(
                    "Please select a song first.");
                return;
            }

            try
            {
                EnsureConnection();

                // Obtiene el ID de la canción.
                int songId = Convert.ToInt32(
                    dgvSongs.CurrentRow.Cells["SongID"].Value);

                // Ejecuta el procedimiento almacenado.
                // Conservamos la llamada CALL que ya funcionaba.
                using var command = new MySqlCommand(
                    "CALL biblioteca_musical.sp_agregar_a_biblioteca(@songId);",
                    _connection);

                command.CommandType = CommandType.Text;

                // Enviamos el ID de forma segura.
                command.Parameters.Add(
                    "@songId",
                    MySqlDbType.Int32
                ).Value = songId;

                command.ExecuteNonQuery();

                MessageBox.Show(
                    "Song added to your library successfully!",
                    "Music Library");
            }
            catch (MySqlException ex)
            {
                if (ex.Number == 1062)
                {
                    MessageBox.Show(
                        "This song is already in your library.",
                        "Music Library");
                }
                else
                {
                    MessageBox.Show(
                        "Could not add the song: " + ex.Message,
                        "Music Library");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error: " + ex.Message,
                    "Music Library");
            }
        }

        // ==========================================
        // MY LIBRARY
        // ==========================================

        // Abre la ventana MyLibraryForm.
        private void btnMyLibrary_Click(
            object sender, EventArgs e)
        {
            if (_connection == null)
            {
                MessageBox.Show(
                    "Database connection is not available.");
                return;
            }

            try
            {
                EnsureConnection();

                // Compartimos la conexión de la sesión.
                using (MyLibraryForm libraryForm =
                    new MyLibraryForm(_connection))
                {
                    libraryForm.ShowDialog(this);
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(
                    "Error opening library: " + ex.Message,
                    "Music Library");
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error: " + ex.Message,
                    "Music Library");
            }
        }

        // ==========================================
        // ADMINISTRATOR ROLE
        // ==========================================

        // Revisa si rol_admin está entre los roles activos.
        private bool HasAdminRole()
        {
            EnsureConnection();

            // Activa los roles ya concedidos por MySQL.
            using (var activateRoles = new MySqlCommand(
                "SET ROLE ALL;",
                _connection!))
            {
                activateRoles.ExecuteNonQuery();
            }

            // Obtiene los roles activos.
            using var command = new MySqlCommand(
                "SELECT CURRENT_ROLE();",
                _connection!);

            string activeRoles =
                Convert.ToString(command.ExecuteScalar()) ?? "";

            // MySQL puede devolver varios roles.
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

                // Ejemplo:
                // `rol_admin`@`%`
                //
                // Extraemos solamente rol_admin.
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
        // CONFIGURE ADMIN ACCESS
        // ==========================================

        // Muestra Admin Panel únicamente
        // a los usuarios administradores.
        private void ConfigureAdminAccess()
        {
            btnAdminPanel.Visible = false;

            if (_connection == null)
            {
                return;
            }

            try
            {
                bool isAdmin = HasAdminRole();

                btnAdminPanel.Visible = isAdmin;
            }
            catch (Exception ex)
            {
                btnAdminPanel.Visible = false;

                MessageBox.Show(
                    "Could not verify administrator permissions: " +
                    ex.Message,
                    "Music Library",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
        }

        // ==========================================
        // ADMIN PANEL
        // ==========================================

        // Abre AdminForm con la conexión actual.
        private void btnAdminPanel_Click(
            object sender, EventArgs e)
        {
            if (_connection == null)
            {
                MessageBox.Show(
                    "Database connection is not available.");
                return;
            }

            try
            {
                EnsureConnection();

                // Verificamos el rol nuevamente.
                if (!HasAdminRole())
                {
                    btnAdminPanel.Visible = false;

                    MessageBox.Show(
                        "Access denied. Administrator role required.",
                        "Admin Panel",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    return;
                }

                // Abre el panel administrativo.
                using (AdminForm adminForm =
                    new AdminForm(_connection))
                {
                    adminForm.ShowDialog(this);
                }

                // Al cerrar AdminForm, actualizamos los
                // géneros y las canciones del catálogo.
                //
                // Esto permite detectar nuevos géneros
                // agregados por el administrador.
                LoadGenres();
                LoadSongs(txtSearch.Text.Trim());
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(
                    "Error opening Admin Panel: " + ex.Message,
                    "Music Library");
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error: " + ex.Message,
                    "Music Library");
            }
        }

        // ==========================================
        // PERMISSION TEST
        // ==========================================

        // Prueba de solo lectura para demostrar el control
        // de permisos de MySQL desde la aplicación.
        // Un usuario normal no debe poder consultar usuarios.
        private void btnTestPermissions_Click(
            object sender, EventArgs e)
        {
            try
            {
                EnsureConnection();

                // Activa los roles concedidos a esta cuenta.
                // Esto NO le otorga permisos nuevos.
                using (var activateRoles = new MySqlCommand(
                    "SET ROLE ALL;", _connection!))
                {
                    activateRoles.ExecuteNonQuery();
                }

                // SELECT de prueba: no inserta, edita ni borra datos.
                // MySQL debe rechazarlo para rol_usuario.
                using var command = new MySqlCommand(
                    "SELECT COUNT(*) FROM biblioteca_musical.usuarios;",
                    _connection!);

                command.ExecuteScalar();

                // Un administrador tiene permiso para consultar.
                MessageBox.Show(
                    "Permission test successful.\n" +
                    "This account can read the restricted users table.",
                    "Permission Test",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            catch (MySqlException ex) when (
                ex.Number == 1142 ||
                ex.Number == 1143 ||
                ex.Number == 1044)
            {
                // El acceso denegado es el resultado esperado
                // cuando inicia sesión un usuario normal.
                MessageBox.Show(
                    "Access denied.\n\n" +
                    "Your account does not have permission to " +
                    "read the restricted users table.\n\n" +
                    "MySQL blocked the operation (error " +
                    ex.Number + ").\n" +
                    "The application handled the error correctly.",
                    "Permission Test",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
            catch (MySqlException ex)
            {
                // Otro error de MySQL no debe confundirse
                // con una prueba de seguridad exitosa.
                MessageBox.Show(
                    "Could not complete the permission test:\n" +
                    ex.Message,
                    "Permission Test",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Could not complete the permission test:\n" +
                    ex.Message,
                    "Permission Test",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        // ==========================================
        // FORM EVENTS
        // ==========================================

        // Evento que ya estaba conectado al diseñador.
        private void MainForm_Load(
            object sender, EventArgs e)
        {
        }

        // Comprueba el acceso administrativo cuando
        // aparece MainForm por primera vez.
        private void MainForm_Shown(
            object? sender, EventArgs e)
        {
            ConfigureAdminAccess();
        }
    }
}
