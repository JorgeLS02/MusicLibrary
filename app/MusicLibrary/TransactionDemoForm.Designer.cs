
namespace MusicLibrary
{
    partial class TransactionDemoForm
    {
        // Contenedor de los componentes de la ventana.
        private System.ComponentModel.IContainer components = null;

        // Libera los recursos utilizados por el formulario.
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
            lblDescription = new Label();

            grpData = new GroupBox();
            lblArtistName = new Label();
            txtArtistName = new TextBox();
            lblCountry = new Label();
            txtCountry = new TextBox();
            lblArtistGenre = new Label();
            txtArtistGenre = new TextBox();

            lblSongTitle = new Label();
            txtSongTitle = new TextBox();
            lblSongGenre = new Label();
            txtSongGenre = new TextBox();
            lblDuration = new Label();
            nudDuration = new NumericUpDown();

            grpLog = new GroupBox();
            txtLog = new TextBox();

            chkSimulateFailure = new CheckBox();
            btnExecute = new Button();
            btnClearLog = new Button();
            btnClose = new Button();
            lblStatus = new Label();

            grpData.SuspendLayout();
            grpLog.SuspendLayout();

            ((System.ComponentModel.ISupportInitialize)
                nudDuration).BeginInit();

            SuspendLayout();

            // ==========================================
            // WINDOW TITLE
            // ==========================================

            lblTitle.AutoSize = true;
            lblTitle.Font = new Font(
                "Segoe UI",
                16F,
                FontStyle.Bold);

            lblTitle.Location = new Point(22, 18);
            lblTitle.Name = "lblTitle";
            lblTitle.Text = "Database Transactions";

            // ==========================================
            // DESCRIPTION
            // ==========================================

            lblDescription.Location = new Point(22, 61);
            lblDescription.Name = "lblDescription";
            lblDescription.Size = new Size(920, 38);
            lblDescription.Text =
                "Insert an artist and a song in one transaction. " +
                "Registration and release dates use today's date.";

            // ==========================================
            // DATA GROUP
            // ==========================================

            grpData.Controls.Add(lblArtistName);
            grpData.Controls.Add(txtArtistName);
            grpData.Controls.Add(lblCountry);
            grpData.Controls.Add(txtCountry);
            grpData.Controls.Add(lblArtistGenre);
            grpData.Controls.Add(txtArtistGenre);

            grpData.Controls.Add(lblSongTitle);
            grpData.Controls.Add(txtSongTitle);
            grpData.Controls.Add(lblSongGenre);
            grpData.Controls.Add(txtSongGenre);
            grpData.Controls.Add(lblDuration);
            grpData.Controls.Add(nudDuration);

            grpData.Location = new Point(22, 112);
            grpData.Name = "grpData";
            grpData.Size = new Size(360, 422);
            grpData.TabIndex = 0;
            grpData.TabStop = false;
            grpData.Text = "Transaction Data";

            // ==========================================
            // ARTIST NAME
            // ==========================================

            lblArtistName.AutoSize = true;
            lblArtistName.Location = new Point(16, 27);
            lblArtistName.Text = "Artist Name";

            txtArtistName.Location = new Point(16, 49);
            txtArtistName.Name = "txtArtistName";
            txtArtistName.Size = new Size(325, 23);
            txtArtistName.MaxLength = 100;
            txtArtistName.TabIndex = 0;

            // ==========================================
            // COUNTRY
            // ==========================================

            lblCountry.AutoSize = true;
            lblCountry.Location = new Point(16, 90);
            lblCountry.Text = "Country";

            txtCountry.Location = new Point(16, 112);
            txtCountry.Name = "txtCountry";
            txtCountry.Size = new Size(325, 23);
            txtCountry.MaxLength = 60;
            txtCountry.TabIndex = 1;

            // ==========================================
            // ARTIST GENRE
            // ==========================================

            lblArtistGenre.AutoSize = true;
            lblArtistGenre.Location = new Point(16, 153);
            lblArtistGenre.Text = "Artist Genre";

            txtArtistGenre.Location = new Point(16, 175);
            txtArtistGenre.Name = "txtArtistGenre";
            txtArtistGenre.Size = new Size(325, 23);
            txtArtistGenre.MaxLength = 50;
            txtArtistGenre.TabIndex = 2;

            // ==========================================
            // SONG TITLE
            // ==========================================

            lblSongTitle.AutoSize = true;
            lblSongTitle.Location = new Point(16, 216);
            lblSongTitle.Text = "Song Title";

            txtSongTitle.Location = new Point(16, 238);
            txtSongTitle.Name = "txtSongTitle";
            txtSongTitle.Size = new Size(325, 23);
            txtSongTitle.MaxLength = 100;
            txtSongTitle.TabIndex = 3;

            // ==========================================
            // SONG GENRE
            // ==========================================

            lblSongGenre.AutoSize = true;
            lblSongGenre.Location = new Point(16, 279);
            lblSongGenre.Text = "Song Genre";

            txtSongGenre.Location = new Point(16, 301);
            txtSongGenre.Name = "txtSongGenre";
            txtSongGenre.Size = new Size(325, 23);
            txtSongGenre.MaxLength = 50;
            txtSongGenre.TabIndex = 4;

            // ==========================================
            // DURATION
            // ==========================================

            lblDuration.AutoSize = true;
            lblDuration.Location = new Point(16, 342);
            lblDuration.Text = "Duration (seconds)";

            nudDuration.Location = new Point(16, 364);
            nudDuration.Name = "nudDuration";
            nudDuration.Size = new Size(150, 23);
            nudDuration.Minimum = 1M;
            nudDuration.Maximum = 36000M;
            nudDuration.Value = 210M;
            nudDuration.TabIndex = 5;

            // ==========================================
            // TRANSACTION LOG
            // ==========================================

            grpLog.Controls.Add(txtLog);
            grpLog.Location = new Point(400, 112);
            grpLog.Name = "grpLog";
            grpLog.Size = new Size(550, 422);
            grpLog.TabStop = false;
            grpLog.Text = "Transaction Log";

            // Permite visualizar cada operación.
            txtLog.Location = new Point(12, 23);
            txtLog.Name = "txtLog";
            txtLog.Size = new Size(526, 386);
            txtLog.Multiline = true;
            txtLog.ReadOnly = true;
            txtLog.ScrollBars = ScrollBars.Vertical;
            txtLog.Font = new Font("Consolas", 9F);
            txtLog.TabIndex = 7;

            // ==========================================
            // SIMULATE FAILURE
            // ==========================================

            // Si está marcada, se ejecuta ROLLBACK.
            chkSimulateFailure.AutoSize = true;
            chkSimulateFailure.Location = new Point(22, 548);
            chkSimulateFailure.Name = "chkSimulateFailure";
            chkSimulateFailure.Text =
                "Simulate Failure (ROLLBACK)";
            chkSimulateFailure.Checked = true;
            chkSimulateFailure.TabIndex = 8;

            // ==========================================
            // EXECUTE BUTTON
            // ==========================================

            btnExecute.Location = new Point(22, 587);
            btnExecute.Name = "btnExecute";
            btnExecute.Size = new Size(150, 38);
            btnExecute.TabIndex = 9;
            btnExecute.Text = "Run Transaction";
            btnExecute.UseVisualStyleBackColor = true;

            // ==========================================
            // CLEAR LOG BUTTON
            // ==========================================

            btnClearLog.Location = new Point(190, 587);
            btnClearLog.Name = "btnClearLog";
            btnClearLog.Size = new Size(120, 38);
            btnClearLog.TabIndex = 10;
            btnClearLog.Text = "Clear Log";
            btnClearLog.UseVisualStyleBackColor = true;

            // ==========================================
            // CLOSE BUTTON
            // ==========================================

            btnClose.Location = new Point(830, 587);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(120, 38);
            btnClose.TabIndex = 11;
            btnClose.Text = "Close";
            btnClose.UseVisualStyleBackColor = true;

            // ==========================================
            // STATUS LABEL
            // ==========================================

            lblStatus.AutoSize = true;
            lblStatus.Location = new Point(22, 642);
            lblStatus.Name = "lblStatus";
            lblStatus.Text = "Ready to run a transaction.";

            // ==========================================
            // FORM
            // ==========================================

            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(980, 680);

            Controls.Add(lblTitle);
            Controls.Add(lblDescription);
            Controls.Add(grpData);
            Controls.Add(grpLog);
            Controls.Add(chkSimulateFailure);
            Controls.Add(btnExecute);
            Controls.Add(btnClearLog);
            Controls.Add(btnClose);
            Controls.Add(lblStatus);

            Name = "TransactionDemoForm";
            Text = "Transactions - Music Library";
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;

            grpData.ResumeLayout(false);
            grpData.PerformLayout();

            grpLog.ResumeLayout(false);
            grpLog.PerformLayout();

            ((System.ComponentModel.ISupportInitialize)
                nudDuration).EndInit();

            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        // ==========================================
        // CONTROL DECLARATIONS
        // ==========================================

        private Label lblTitle;
        private Label lblDescription;

        private GroupBox grpData;
        private Label lblArtistName;
        private TextBox txtArtistName;
        private Label lblCountry;
        private TextBox txtCountry;
        private Label lblArtistGenre;
        private TextBox txtArtistGenre;
        private Label lblSongTitle;
        private TextBox txtSongTitle;
        private Label lblSongGenre;
        private TextBox txtSongGenre;
        private Label lblDuration;
        private NumericUpDown nudDuration;

        private GroupBox grpLog;
        private TextBox txtLog;

        private CheckBox chkSimulateFailure;
        private Button btnExecute;
        private Button btnClearLog;
        private Button btnClose;
        private Label lblStatus;
    }
}
