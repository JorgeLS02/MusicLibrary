
namespace MusicLibrary
{
    partial class ReportsForm
    {
        // Contenedor de componentes del formulario.
        private System.ComponentModel.IContainer components = null;

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
            lblTitle = new Label();
            lblReportType = new Label();
            cmbReportType = new ComboBox();

            lblMinimumSongs = new Label();
            nudMinimumSongs = new NumericUpDown();

            // Nuevos controles para el procedimiento OUT.
            lblProcedureArtist = new Label();
            cmbProcedureArtist = new ComboBox();

            btnRunReport = new Button();
            lblDescription = new Label();
            dgvReportResults = new DataGridView();
            lblStatus = new Label();
            btnClose = new Button();

            ((System.ComponentModel.ISupportInitialize)
                nudMinimumSongs).BeginInit();

            ((System.ComponentModel.ISupportInitialize)
                dgvReportResults).BeginInit();

            SuspendLayout();

            // ==========================================
            // WINDOW TITLE
            // ==========================================

            lblTitle.AutoSize = true;
            lblTitle.Font = new Font(
                "Segoe UI", 16F, FontStyle.Bold);

            lblTitle.Location = new Point(24, 19);
            lblTitle.Name = "lblTitle";
            lblTitle.Text = "Music Library Reports";

            // ==========================================
            // REPORT TYPE LABEL
            // ==========================================

            lblReportType.AutoSize = true;
            lblReportType.Location = new Point(24, 66);
            lblReportType.Name = "lblReportType";
            lblReportType.Text = "Report Type";

            // ==========================================
            // REPORT TYPE COMBOBOX
            // ==========================================

            // Permite seleccionar cualquiera de los
            // siete reportes existentes o el nuevo.
            cmbReportType.DropDownStyle =
                ComboBoxStyle.DropDownList;

            cmbReportType.FormattingEnabled = true;
            cmbReportType.Location = new Point(24, 89);
            cmbReportType.Name = "cmbReportType";
            cmbReportType.Size = new Size(530, 23);
            cmbReportType.TabIndex = 0;

            cmbReportType.Items.AddRange(new object[]
            {
                "Artist Statistics (GROUP BY + HAVING)",
                "Artists and Songs (LEFT JOIN)",
                "Songs Above Catalog Average (Subquery)",
                "Songs Above Artist Average (Correlated Subquery)",
                "Complete Catalog (UNION ALL)",
                "Music Genres (UNION)",
                "Song Durations (SQL Function)",
                "Artist Song Count (Stored Procedure OUT)"
            });

            // ==========================================
            // MINIMUM SONGS
            // ==========================================

            // Se usa únicamente con Artist Statistics.
            lblMinimumSongs.AutoSize = true;
            lblMinimumSongs.Location = new Point(585, 66);
            lblMinimumSongs.Name = "lblMinimumSongs";
            lblMinimumSongs.Text = "Minimum Songs";

            nudMinimumSongs.Location = new Point(585, 89);
            nudMinimumSongs.Name = "nudMinimumSongs";
            nudMinimumSongs.Size = new Size(140, 23);
            nudMinimumSongs.Minimum = 1M;
            nudMinimumSongs.Maximum = 1000M;
            nudMinimumSongs.Value = 1M;
            nudMinimumSongs.TabIndex = 1;

            // ==========================================
            // PROCEDURE ARTIST
            // ==========================================

            // Este campo aparece cuando seleccionamos
            // el reporte Stored Procedure OUT.
            lblProcedureArtist.AutoSize = true;
            lblProcedureArtist.Location = new Point(585, 66);
            lblProcedureArtist.Name = "lblProcedureArtist";
            lblProcedureArtist.Text = "Select Artist";
            lblProcedureArtist.Visible = false;

            // La lista de artistas se carga desde MySQL.
            cmbProcedureArtist.DropDownStyle =
                ComboBoxStyle.DropDownList;

            cmbProcedureArtist.FormattingEnabled = true;
            cmbProcedureArtist.Location = new Point(585, 89);
            cmbProcedureArtist.Name = "cmbProcedureArtist";
            cmbProcedureArtist.Size = new Size(190, 23);
            cmbProcedureArtist.TabIndex = 1;
            cmbProcedureArtist.Visible = false;

            // ==========================================
            // RUN REPORT
            // ==========================================

            btnRunReport.Location = new Point(806, 82);
            btnRunReport.Name = "btnRunReport";
            btnRunReport.Size = new Size(150, 35);
            btnRunReport.TabIndex = 2;
            btnRunReport.Text = "Run Report";
            btnRunReport.UseVisualStyleBackColor = true;

            btnRunReport.Anchor =
                AnchorStyles.Top | AnchorStyles.Right;

            // ==========================================
            // DESCRIPTION
            // ==========================================

            lblDescription.Location = new Point(24, 128);
            lblDescription.Name = "lblDescription";
            lblDescription.Size = new Size(932, 29);
            lblDescription.Text =
                "Select a report to view its results.";

            lblDescription.Anchor =
                AnchorStyles.Top |
                AnchorStyles.Left |
                AnchorStyles.Right;

            // ==========================================
            // RESULTS TABLE
            // ==========================================

            dgvReportResults.AllowUserToAddRows = false;
            dgvReportResults.AllowUserToDeleteRows = false;
            dgvReportResults.ReadOnly = true;
            dgvReportResults.MultiSelect = false;

            dgvReportResults.AutoSizeColumnsMode =
                DataGridViewAutoSizeColumnsMode.Fill;

            dgvReportResults.ColumnHeadersHeightSizeMode =
                DataGridViewColumnHeadersHeightSizeMode.AutoSize;

            dgvReportResults.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;

            dgvReportResults.RowHeadersVisible = false;

            dgvReportResults.Location = new Point(24, 165);
            dgvReportResults.Name = "dgvReportResults";
            dgvReportResults.Size = new Size(932, 378);
            dgvReportResults.TabIndex = 3;

            dgvReportResults.Anchor =
                AnchorStyles.Top |
                AnchorStyles.Bottom |
                AnchorStyles.Left |
                AnchorStyles.Right;

            // ==========================================
            // STATUS
            // ==========================================

            lblStatus.AutoSize = true;
            lblStatus.Location = new Point(24, 567);
            lblStatus.Name = "lblStatus";
            lblStatus.Text = "No report loaded.";

            lblStatus.Anchor =
                AnchorStyles.Bottom | AnchorStyles.Left;

            // ==========================================
            // CLOSE
            // ==========================================

            btnClose.Location = new Point(836, 557);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(120, 35);
            btnClose.TabIndex = 4;
            btnClose.Text = "Close";
            btnClose.UseVisualStyleBackColor = true;

            btnClose.Anchor =
                AnchorStyles.Bottom | AnchorStyles.Right;

            // ==========================================
            // REPORTS FORM
            // ==========================================

            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;

            ClientSize = new Size(980, 612);
            MinimumSize = new Size(900, 600);

            Controls.Add(lblTitle);
            Controls.Add(lblReportType);
            Controls.Add(cmbReportType);

            Controls.Add(lblMinimumSongs);
            Controls.Add(nudMinimumSongs);

            Controls.Add(lblProcedureArtist);
            Controls.Add(cmbProcedureArtist);

            Controls.Add(btnRunReport);
            Controls.Add(lblDescription);
            Controls.Add(dgvReportResults);
            Controls.Add(lblStatus);
            Controls.Add(btnClose);

            Name = "ReportsForm";
            Text = "Reports - Music Library";
            StartPosition = FormStartPosition.CenterParent;

            ((System.ComponentModel.ISupportInitialize)
                nudMinimumSongs).EndInit();

            ((System.ComponentModel.ISupportInitialize)
                dgvReportResults).EndInit();

            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        // ==========================================
        // CONTROL DECLARATIONS
        // ==========================================

        private Label lblTitle;

        private Label lblReportType;
        private ComboBox cmbReportType;

        private Label lblMinimumSongs;
        private NumericUpDown nudMinimumSongs;

        // Controles nuevos del procedimiento almacenado.
        private Label lblProcedureArtist;
        private ComboBox cmbProcedureArtist;

        private Button btnRunReport;
        private Label lblDescription;
        private DataGridView dgvReportResults;
        private Label lblStatus;
        private Button btnClose;
    }
}
