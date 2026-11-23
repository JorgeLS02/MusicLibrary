
using System;
using System.Windows.Forms;

namespace MusicLibrary
{
    // Esta clase partial complementa AdminForm.cs.
    //
    // Nos permite agregar funciones nuevas
    // sin modificar el código que ya tenemos
    // para administrar artistas y canciones.
    public partial class AdminForm
    {
        // ==========================================
        // REPORTS BUTTON
        // ==========================================

        // Este método se ejecuta cuando presionamos
        // Reports desde Artists o Songs.
        private void btnReports_Click(
            object? sender,
            EventArgs e)
        {
            // Comprobamos que exista la conexión a MySQL.
            if (_connection == null)
            {
                MessageBox.Show(
                    "Database connection is not available.",
                    "Music Library");

                return;
            }

            try
            {
                // Garantiza que MySQL esté conectado.
                EnsureConnection();

                // Verifica que siga teniendo rol_admin.
                if (!HasAdminRole())
                {
                    MessageBox.Show(
                        "Access denied. Administrator role required.",
                        "Admin Panel",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    return;
                }

                // Abre la ventana de reportes con la
                // misma conexión del administrador.
                using (ReportsForm reportsForm =
                    new ReportsForm(_connection))
                {
                    reportsForm.ShowDialog(this);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Could not open Reports:\n" +
                    ex.Message,
                    "Music Library",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        // ==========================================
        // TRANSACTIONS BUTTON
        // ==========================================

        // Este método se ejecuta cuando presionamos
        // Transactions desde Artists o Songs.
        private void btnTransactions_Click(
            object? sender,
            EventArgs e)
        {
            // Comprobamos la conexión a MySQL.
            if (_connection == null)
            {
                MessageBox.Show(
                    "Database connection is not available.",
                    "Music Library");

                return;
            }

            try
            {
                // Verifica que la conexión esté abierta.
                EnsureConnection();

                // Solo permitimos la demostración
                // a usuarios administradores.
                if (!HasAdminRole())
                {
                    MessageBox.Show(
                        "Access denied. Administrator role required.",
                        "Admin Panel",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    return;
                }

                // ==================================
                // OPEN TRANSACTION DEMO
                // ==================================

                // Creamos TransactionDemoForm.
                //
                // Le enviamos la misma conexión MySQL.
                // No se solicita nuevamente la contraseña.
                using (TransactionDemoForm transactionForm =
                    new TransactionDemoForm(_connection))
                {
                    // Abre la ventana sobre Admin Panel.
                    transactionForm.ShowDialog(this);
                }

                // ==================================
                // REFRESH DATA
                // ==================================

                // Cuando se cierra Transactions,
                // actualizamos las tablas de Artists
                // y Songs.
                //
                // Si se ejecutó COMMIT, aparecerán
                // el nuevo artista y su canción.
                //
                // Si se ejecutó ROLLBACK, no aparecerán.
                RefreshArtistData();
            }
            catch (Exception ex)
            {
                // Captura errores al abrir la ventana
                // o actualizar los datos.
                MessageBox.Show(
                    "Could not open Transactions:\n" +
                    ex.Message,
                    "Music Library",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }
    }
}
