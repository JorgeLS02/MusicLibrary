
using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Windows.Forms;

namespace MusicLibrary
{
    public partial class TransactionDemoForm : Form
    {
        // Conexión que recibiremos desde Admin Panel.
        // No la cerramos porque pertenece a la sesión.
        private readonly MySqlConnection? _connection;

        // ==========================================
        // CONSTRUCTORS
        // ==========================================

        // Constructor utilizado por Visual Studio.
        public TransactionDemoForm()
        {
            InitializeComponent();

            // Conecta los botones con sus funciones.
            btnExecute.Click += btnExecute_Click;
            btnClearLog.Click += btnClearLog_Click;
            btnClose.Click += btnClose_Click;

            // Datos iniciales para facilitar la prueba.
            txtCountry.Text = "Mexico";
            txtArtistGenre.Text = "Pop";
            txtSongTitle.Text = "Transaction Demo Song";
            txtSongGenre.Text = "Pop";

            // Nombre único para evitar duplicados
            // al ejecutar varias pruebas con COMMIT.
            GenerateArtistName();

            // Iniciamos con ROLLBACK como prueba segura.
            chkSimulateFailure.Checked = true;

            LogLine(
                "Ready. Simulate Failure is enabled.");

            LogLine(
                "This demonstration inserts into " +
                "artistas and canciones.");
        }

        // Recibe la conexión de MySQL desde Admin Panel.
        public TransactionDemoForm(
            MySqlConnection connection) : this()
        {
            _connection = connection;
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
        // GENERATE TEST ARTIST
        // ==========================================

        // Genera un nombre distinto para las pruebas.
        // Esto ayuda a evitar errores por UNIQUE.
        private void GenerateArtistName()
        {
            txtArtistName.Text =
                "Transaction Artist " +
                DateTime.Now.ToString("yyyyMMdd_HHmmss_fff");
        }

        // ==========================================
        // VALIDATION
        // ==========================================

        // Revisa que los campos obligatorios tengan datos.
        private bool ValidateInputs()
        {
            if (string.IsNullOrWhiteSpace(txtArtistName.Text))
            {
                MessageBox.Show(
                    "Enter an artist name.");

                txtArtistName.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtCountry.Text))
            {
                MessageBox.Show(
                    "Enter a country.");

                txtCountry.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtArtistGenre.Text))
            {
                MessageBox.Show(
                    "Enter an artist genre.");

                txtArtistGenre.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtSongTitle.Text))
            {
                MessageBox.Show(
                    "Enter a song title.");

                txtSongTitle.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtSongGenre.Text))
            {
                MessageBox.Show(
                    "Enter a song genre.");

                txtSongGenre.Focus();
                return false;
            }

            return true;
        }

        // ==========================================
        // TRANSACTION LOG
        // ==========================================

        // Escribe cada paso en el cuadro de resultados.
        private void LogLine(string message)
        {
            txtLog.AppendText(
                "[" + DateTime.Now.ToString("HH:mm:ss") +
                "] " + message + Environment.NewLine);
        }

        // ==========================================
        // RUN TRANSACTION
        // ==========================================

        // Ejecuta una transacción que afecta
        // dos tablas: artistas y canciones.
        private void btnExecute_Click(
            object? sender, EventArgs e)
        {
            if (!ValidateInputs())
            {
                return;
            }

            if (_connection == null)
            {
                MessageBox.Show(
                    "Database connection is not available.");

                return;
            }

            // Si no simulamos error, los datos
            // se guardarán permanentemente con COMMIT.
            if (!chkSimulateFailure.Checked)
            {
                DialogResult confirmation = MessageBox.Show(
                    "This transaction will save a new artist " +
                    "and song in MySQL.\n\nContinue?",
                    "Confirm COMMIT",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (confirmation != DialogResult.Yes)
                {
                    return;
                }
            }

            btnExecute.Enabled = false;

            // Guardamos los IDs creados para poder
            // verificar los resultados del ROLLBACK.
            long artistId = 0;
            long songId = 0;

            try
            {
                EnsureConnection();

                LogLine("");
                LogLine("============================");
                LogLine("Starting a new transaction.");

                // ======================================
                // BEGIN TRANSACTION
                // ======================================

                // Inicia una transacción real de MySQL.
                // Las operaciones no serán definitivas
                // hasta llamar a Commit().
                using MySqlTransaction transaction =
                    _connection.BeginTransaction();

                LogLine("BEGIN TRANSACTION executed.");

                try
                {
                    // ==================================
                    // INSERT INTO ARTISTAS
                    // ==================================

                    string artistQuery = @"
                        INSERT INTO artistas
                        (
                            nombre,
                            pais,
                            genero,
                            fecha_registro
                        )
                        VALUES
                        (
                            @name,
                            @country,
                            @genre,
                            @date
                        );";

                    // Es importante pasar la transacción
                    // al comando de MySQL.
                    //
                    // Así este INSERT forma parte de la
                    // misma operación que el segundo INSERT.
                    using (var artistCommand =
                        new MySqlCommand(
                            artistQuery,
                            _connection,
                            transaction))
                    {
                        artistCommand.Parameters.Add(
                            "@name",
                            MySqlDbType.VarChar
                        ).Value = txtArtistName.Text.Trim();

                        artistCommand.Parameters.Add(
                            "@country",
                            MySqlDbType.VarChar
                        ).Value = txtCountry.Text.Trim();

                        artistCommand.Parameters.Add(
                            "@genre",
                            MySqlDbType.VarChar
                        ).Value = txtArtistGenre.Text.Trim();

                        artistCommand.Parameters.Add(
                            "@date",
                            MySqlDbType.Date
                        ).Value = DateTime.Today;

                        // Ejecuta el primer INSERT.
                        artistCommand.ExecuteNonQuery();

                        // Obtiene el ID del artista nuevo.
                        artistId =
                            artistCommand.LastInsertedId;
                    }

                    LogLine(
                        "INSERT INTO artistas completed.");

                    LogLine(
                        "Temporary ArtistID: " + artistId);

                    // ==================================
                    // INSERT INTO CANCIONES
                    // ==================================

                    string songQuery = @"
                        INSERT INTO canciones
                        (
                            titulo,
                            id_artista,
                            duracion,
                            genero,
                            fecha_lanzamiento
                        )
                        VALUES
                        (
                            @title,
                            @artistId,
                            @duration,
                            @genre,
                            @releaseDate
                        );";

                    using (var songCommand =
                        new MySqlCommand(
                            songQuery,
                            _connection,
                            transaction))
                    {
                        songCommand.Parameters.Add(
                            "@title",
                            MySqlDbType.VarChar
                        ).Value = txtSongTitle.Text.Trim();

                        // Relacionamos la nueva canción
                        // con el artista recién creado.
                        songCommand.Parameters.Add(
                            "@artistId",
                            MySqlDbType.Int64
                        ).Value = artistId;

                        songCommand.Parameters.Add(
                            "@duration",
                            MySqlDbType.Int32
                        ).Value = Convert.ToInt32(
                            nudDuration.Value);

                        songCommand.Parameters.Add(
                            "@genre",
                            MySqlDbType.VarChar
                        ).Value = txtSongGenre.Text.Trim();

                        songCommand.Parameters.Add(
                            "@releaseDate",
                            MySqlDbType.Date
                        ).Value = DateTime.Today;

                        // Ejecuta el segundo INSERT.
                        songCommand.ExecuteNonQuery();

                        // Guarda el ID de la nueva canción.
                        songId = songCommand.LastInsertedId;
                    }

                    LogLine(
                        "INSERT INTO canciones completed.");

                    LogLine(
                        "Temporary SongID: " + songId);

                    // ==================================
                    // SIMULATED FAILURE
                    // ==================================

                    // Para demostrar ROLLBACK,
                    // provocamos una excepción DESPUÉS
                    // de ejecutar ambos INSERT.
                    //
                    // Así el rollback tiene que revertir
                    // las operaciones de las dos tablas.
                    if (chkSimulateFailure.Checked)
                    {
                        LogLine(
                            "Simulating an error before COMMIT.");

                        throw new InvalidOperationException(
                            "Simulated transaction failure.");
                    }

                    // ==================================
                    // COMMIT
                    // ==================================

                    // Si todo salió bien, guarda ambos
                    // registros permanentemente.
                    transaction.Commit();

                    LogLine("COMMIT executed successfully.");

                    LogLine(
                        "Both records were saved in MySQL.");

                    lblStatus.Text =
                        "COMMIT successful: artist and song saved.";

                    // Prepara otro nombre para futuras pruebas.
                    GenerateArtistName();
                }
                catch (Exception ex)
                {
                    // ==================================
                    // ERROR HANDLING
                    // ==================================

                    LogLine("Error: " + ex.Message);

                    // ==================================
                    // ROLLBACK
                    // ==================================

                    // Cuando ocurre un error, deshacemos
                    // todas las operaciones de esta
                    // transacción, no solo la última.
                    bool rollbackSucceeded = false;

                    try
                    {
                        transaction.Rollback();

                        rollbackSucceeded = true;

                        LogLine(
                            "ROLLBACK executed successfully.");

                        LogLine(
                            "Changes to both tables were reverted.");

                        lblStatus.Text =
                            "ROLLBACK successful: no records saved.";
                    }
                    catch (Exception rollbackError)
                    {
                        LogLine(
                            "ROLLBACK could not be confirmed: " +
                            rollbackError.Message);

                        lblStatus.Text =
                            "Transaction failed. Check the database.";
                    }

                    // ==================================
                    // VERIFY ROLLBACK
                    // ==================================

                    // Si el primer INSERT sí obtuvo un ID,
                    // comprobamos que el artista y su
                    // canción no hayan quedado guardados.
                    if (rollbackSucceeded && artistId > 0)
                    {
                        try
                        {
                            VerifyRollback(artistId);
                        }
                        catch (Exception verifyError)
                        {
                            LogLine(
                                "Could not verify rollback: " +
                                verifyError.Message);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Maneja problemas al iniciar la conexión
                // o comenzar la transacción.
                LogLine(
                    "Could not start transaction: " +
                    ex.Message);

                lblStatus.Text =
                    "Transaction could not be started.";
            }
            finally
            {
                // Vuelve a habilitar el botón.
                btnExecute.Enabled = true;
            }
        }

        // ==========================================
        // VERIFY ROLLBACK
        // ==========================================

        // Comprueba en ambas tablas si quedaron
        // registros asociados con el artista temporal.
        private void VerifyRollback(long artistId)
        {
            EnsureConnection();

            string query = @"
                SELECT
                    (
                        SELECT COUNT(*)
                        FROM artistas
                        WHERE id_artista = @artistId
                    ) AS ArtistCount,
                    (
                        SELECT COUNT(*)
                        FROM canciones
                        WHERE id_artista = @artistId
                    ) AS SongCount;";

            using var command = new MySqlCommand(
                query, _connection!);

            command.Parameters.Add(
                "@artistId",
                MySqlDbType.Int64
            ).Value = artistId;

            using var reader = command.ExecuteReader();

            if (reader.Read())
            {
                long artistCount =
                    Convert.ToInt64(reader["ArtistCount"]);

                long songCount =
                    Convert.ToInt64(reader["SongCount"]);

                LogLine(
                    "Artist records remaining: " +
                    artistCount);

                LogLine(
                    "Song records remaining: " +
                    songCount);

                if (artistCount == 0 && songCount == 0)
                {
                    LogLine(
                        "Verified: neither record was saved.");
                }
                else
                {
                    LogLine(
                        "Warning: review the remaining records.");
                }
            }
        }

        // ==========================================
        // CLEAR LOG
        // ==========================================

        // Limpia solamente los mensajes de pantalla.
        // No modifica registros de MySQL.
        private void btnClearLog_Click(
            object? sender, EventArgs e)
        {
            txtLog.Clear();

            lblStatus.Text =
                "Ready to run a transaction.";
        }

        // ==========================================
        // CLOSE
        // ==========================================

        // Regresa a la ventana anterior.
        private void btnClose_Click(
            object? sender, EventArgs e)
        {
            Close();
        }
    }
}
