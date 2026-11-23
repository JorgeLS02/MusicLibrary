
using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Windows.Forms;

namespace MusicLibrary
{
    public partial class ReportsForm : Form
    {
        // ==========================================
        // DATA ACCESS REPOSITORY
        // ==========================================

        // Esta clase se encarga de las consultas SQL.
        // ReportsForm solo controla la interfaz.
        private ReportsRepository? _repository;

        // ==========================================
        // CONSTRUCTORS
        // ==========================================

        public ReportsForm()
        {
            InitializeComponent();

            // Conecta los eventos de la ventana.
            btnRunReport.Click += btnRunReport_Click;
            btnClose.Click += btnClose_Click;

            cmbReportType.SelectedIndexChanged +=
                cmbReportType_SelectedIndexChanged;

            Shown += ReportsForm_Shown;

            // Selecciona inicialmente Artist Statistics.
            cmbReportType.SelectedIndex = 0;
        }

        // Recibe la conexión del administrador.
        public ReportsForm(
            MySqlConnection connection) : this()
        {
            // Creamos el repositorio usando la
            // misma conexión que ya está abierta.
            _repository = new ReportsRepository(
                connection);
        }

        // ==========================================
        // FORM SHOWN
        // ==========================================

        // Se ejecuta cuando aparece Reports.
        private void ReportsForm_Shown(
            object? sender,
            EventArgs e)
        {
            try
            {
                if (_repository == null)
                {
                    throw new InvalidOperationException(
                        "Database repository is not available.");
                }

                // El DAO verifica los permisos.
                if (!_repository.HasAdminRole())
                {
                    MessageBox.Show(
                        "Access denied. Administrator role required.",
                        "Reports",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    Close();
                    return;
                }

                // Carga los artistas del ComboBox.
                LoadArtistsForProcedure();

                // Ejecuta el reporte inicial.
                RunReport();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Could not open Reports:\n" +
                    ex.Message,
                    "Music Library",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                Close();
            }
        }

        // ==========================================
        // LOAD ARTISTS
        // ==========================================

        // Recibe los artistas desde el DAO.
        // No necesita escribir consultas SQL.
        private void LoadArtistsForProcedure()
        {
            if (_repository == null)
            {
                return;
            }

            cmbProcedureArtist.Items.Clear();

            // El repositorio consulta MySQL.
            var artists = _repository.GetArtists();

            // Agrega los resultados al ComboBox.
            foreach (ReportArtistOption artist in artists)
            {
                cmbProcedureArtist.Items.Add(artist);
            }

            // Selecciona el primer artista.
            if (cmbProcedureArtist.Items.Count > 0)
            {
                cmbProcedureArtist.SelectedIndex = 0;
            }
        }

        // ==========================================
        // SELECT REPORT
        // ==========================================

        // Actualiza los controles visibles.
        private void cmbReportType_SelectedIndexChanged(
            object? sender,
            EventArgs e)
        {
            int index = cmbReportType.SelectedIndex;

            // Minimum Songs solo se utiliza
            // para GROUP BY + HAVING.
            bool showMinimumSongs = index == 0;

            lblMinimumSongs.Visible = showMinimumSongs;
            nudMinimumSongs.Visible = showMinimumSongs;

            // Select Artist solo se utiliza
            // para el procedimiento con OUT.
            bool showArtist = index == 7;

            lblProcedureArtist.Visible = showArtist;
            cmbProcedureArtist.Visible = showArtist;

            // Conservamos las descripciones
            // de los ocho reportes.
            switch (index)
            {
                case 0:
                    lblDescription.Text =
                        "Groups songs by artist and shows " +
                        "counts, averages and total duration.";
                    break;

                case 1:
                    lblDescription.Text =
                        "Shows every artist, including artists " +
                        "who do not have registered songs.";
                    break;

                case 2:
                    lblDescription.Text =
                        "Shows songs longer than the average " +
                        "duration of the entire catalog.";
                    break;

                case 3:
                    lblDescription.Text =
                        "Shows songs longer than the average " +
                        "duration of their own artist.";
                    break;

                case 4:
                    lblDescription.Text =
                        "Combines artists and songs in one " +
                        "catalog using UNION ALL.";
                    break;

                case 5:
                    lblDescription.Text =
                        "Combines artist and song genres " +
                        "without repeating identical values.";
                    break;

                case 6:
                    lblDescription.Text =
                        "Uses fn_duracion_mmss to convert " +
                        "song duration to minutes and seconds.";
                    break;

                case 7:
                    lblDescription.Text =
                        "Calls sp_contar_canciones_artista using " +
                        "IN and OUT parameters.";
                    break;

                default:
                    lblDescription.Text =
                        "Select a report.";
                    break;
            }

            // Limpia el reporte anterior.
            dgvReportResults.DataSource = null;

            lblStatus.Text = "Select Run Report.";
        }

        // ==========================================
        // RUN REPORT BUTTON
        // ==========================================

        private void btnRunReport_Click(
            object? sender,
            EventArgs e)
        {
            RunReport();
        }

        // ==========================================
        // RUN REPORT
        // ==========================================

        // Decide qué operación solicitar al DAO.
        private void RunReport()
        {
            if (_repository == null)
            {
                MessageBox.Show(
                    "Database repository is not available.",
                    "Reports");

                return;
            }

            int index = cmbReportType.SelectedIndex;

            if (index < 0)
            {
                MessageBox.Show(
                    "Please select a report.",
                    "Reports");

                return;
            }

            try
            {
                // ==================================
                // STORED PROCEDURE REPORT
                // ==================================

                if (index == 7)
                {
                    RunArtistCountProcedure();
                    return;
                }

                // ==================================
                // STANDARD SQL REPORTS
                // ==================================

                // Obtiene Minimum Songs de la interfaz.
                int minimumSongs =
                    Convert.ToInt32(
                        nudMinimumSongs.Value);

                // El DAO ejecuta la consulta SQL
                // y devuelve un DataTable.
                DataTable table = _repository.GetReport(
                    index,
                    minimumSongs);

                // El formulario solo muestra datos.
                ShowResults(table);
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(
                    "Database error:\n" +
                    ex.Message,
                    "Reports",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                lblStatus.Text = "Report failed.";
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Could not generate report:\n" +
                    ex.Message,
                    "Music Library",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                lblStatus.Text = "Report failed.";
            }
        }

        // ==========================================
        // STORED PROCEDURE REPORT
        // ==========================================

        // Obtiene el total de canciones mediante
        // un procedimiento almacenado con OUT.
        private void RunArtistCountProcedure()
        {
            if (_repository == null)
            {
                throw new InvalidOperationException(
                    "Database repository is not available.");
            }

            // Comprueba que seleccionamos un artista.
            if (cmbProcedureArtist.SelectedItem
                is not ReportArtistOption selectedArtist)
            {
                MessageBox.Show(
                    "Please select an artist.",
                    "Reports");

                return;
            }

            // ==================================
            // CALL DATA ACCESS REPOSITORY
            // ==================================

            // El repositorio se encarga de:
            //
            // CommandType.StoredProcedure
            // parámetro IN
            // parámetro OUT
            // ejecución del procedimiento MySQL.
            int totalSongs =
                _repository.GetArtistSongCount(
                    selectedArtist.Id);

            // ==================================
            // CREATE VISUAL RESULT
            // ==================================

            // Creamos una tabla temporal para
            // mostrar el valor recibido.
            DataTable result = new DataTable();

            result.Columns.Add(
                "ArtistID",
                typeof(int));

            result.Columns.Add(
                "Artist",
                typeof(string));

            result.Columns.Add(
                "TotalSongs",
                typeof(int));

            result.Rows.Add(
                selectedArtist.Id,
                selectedArtist.Name,
                totalSongs);

            // Presentamos el resultado.
            ShowResults(result);

            lblStatus.Text =
                "Procedure executed successfully. " +
                "Total songs: " + totalSongs;
        }

        // ==========================================
        // SHOW RESULTS
        // ==========================================

        // Recibe un DataTable del DAO y
        // lo muestra en el DataGridView.
        private void ShowResults(DataTable table)
        {
            dgvReportResults.DataSource = table;

            dgvReportResults.ClearSelection();
            dgvReportResults.CurrentCell = null;

            if (table.Rows.Count == 0)
            {
                lblStatus.Text =
                    "No records matched this report.";
            }
            else
            {
                lblStatus.Text =
                    "Rows returned: " + table.Rows.Count;
            }
        }

        // ==========================================
        // CLOSE BUTTON
        // ==========================================

        private void btnClose_Click(
            object? sender,
            EventArgs e)
        {
            Close();
        }
    }
}
