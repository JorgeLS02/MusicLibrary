
using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Windows.Forms;

namespace MusicLibrary
{
    public partial class MyLibraryForm : Form
    {
        private MySqlConnection? _connection;

        public MyLibraryForm()
        {
            InitializeComponent();
        }

        public MyLibraryForm(MySqlConnection connection) : this()
        {
            _connection = connection;
            LoadLibrary();
        }

        // Load the user's personal library
        private void LoadLibrary()
        {
            if (_connection == null)
            {
                MessageBox.Show(
                    "Database connection is not available.");
                return;
            }

            try
            {
                string query = @"
                    SELECT
                        id_cancion AS SongID,
                        titulo AS Title,
                        artista AS Artist,
                        genero AS Genre,
                        duracion AS Duration,
                        favorito AS Favorite,
                        calificacion AS Rating
                    FROM vista_mi_biblioteca
                    ORDER BY titulo;";

                using var adapter = new MySqlDataAdapter(
                    query, _connection);

                DataTable table = new DataTable();

                adapter.Fill(table);

                dgvMyLibrary.DataSource = table;
                dgvMyLibrary.ClearSelection();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(
                    "Error loading library: " + ex.Message,
                    "Music Library");
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error: " + ex.Message,
                    "Music Library");
            }
        }

        // Remove a song from the library
        private void btnRemoveFromLibrary_Click(
            object sender, EventArgs e)
        {
            if (_connection == null)
            {
                MessageBox.Show(
                    "Database connection is not available.");
                return;
            }

            if (dgvMyLibrary.SelectedRows.Count == 0)
            {
                MessageBox.Show(
                    "Please select a song first.");
                return;
            }

            try
            {
                DataGridViewRow row =
                    dgvMyLibrary.SelectedRows[0];

                int songId = Convert.ToInt32(
                    row.Cells["SongID"].Value);

                string songTitle = Convert.ToString(
                    row.Cells["Title"].Value) ?? "";

                DialogResult confirm = MessageBox.Show(
                    "Remove '" + songTitle +
                    "' from your library?",
                    "Confirm Removal",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (confirm != DialogResult.Yes)
                {
                    return;
                }

                if (_connection.State != ConnectionState.Open)
                {
                    _connection.Open();
                }

                using (var command = new MySqlCommand(
                    "CALL biblioteca_musical.sp_quitar_de_biblioteca(@songId);",
                    _connection))
                {
                    command.CommandType = CommandType.Text;

                    command.Parameters.Add(
                        "@songId",
                        MySqlDbType.Int32
                    ).Value = songId;

                    command.ExecuteNonQuery();
                }

                MessageBox.Show(
                    "Song removed from your library successfully!",
                    "Music Library");

                LoadLibrary();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(
                    "Could not remove the song: " + ex.Message,
                    "Music Library");
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error: " + ex.Message,
                    "Music Library");
            }
        }

        // Toggle the favorite status
        private void btnToggleFavorite_Click(
            object sender, EventArgs e)
        {
            if (_connection == null)
            {
                MessageBox.Show(
                    "Database connection is not available.");
                return;
            }

            if (dgvMyLibrary.SelectedRows.Count == 0)
            {
                MessageBox.Show(
                    "Please select a song first.");
                return;
            }

            try
            {
                DataGridViewRow row =
                    dgvMyLibrary.SelectedRows[0];

                int songId = Convert.ToInt32(
                    row.Cells["SongID"].Value);

                string songTitle = Convert.ToString(
                    row.Cells["Title"].Value) ?? "";

                bool isFavorite = Convert.ToBoolean(
                    row.Cells["Favorite"].Value);

                bool newFavorite = !isFavorite;

                if (_connection.State != ConnectionState.Open)
                {
                    _connection.Open();
                }

                using (var command = new MySqlCommand(
                    "CALL biblioteca_musical.sp_marcar_favorita(@songId, @favorite);",
                    _connection))
                {
                    command.CommandType = CommandType.Text;

                    command.Parameters.Add(
                        "@songId",
                        MySqlDbType.Int32
                    ).Value = songId;

                    command.Parameters.Add(
                        "@favorite",
                        MySqlDbType.Int32
                    ).Value = newFavorite ? 1 : 0;

                    command.ExecuteNonQuery();
                }

                if (newFavorite)
                {
                    MessageBox.Show(
                        "'" + songTitle + "' added to favorites!",
                        "Music Library");
                }
                else
                {
                    MessageBox.Show(
                        "'" + songTitle + "' removed from favorites!",
                        "Music Library");
                }

                LoadLibrary();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(
                    "Could not update favorite: " + ex.Message,
                    "Music Library");
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error: " + ex.Message,
                    "Music Library");
            }
        }

        // Rate the selected song from 1 to 5
        private void btnRateSong_Click(
            object sender, EventArgs e)
        {
            if (_connection == null)
            {
                MessageBox.Show(
                    "Database connection is not available.");
                return;
            }

            if (dgvMyLibrary.SelectedRows.Count == 0)
            {
                MessageBox.Show(
                    "Please select a song first.",
                    "Music Library");
                return;
            }

            try
            {
                DataGridViewRow row =
                    dgvMyLibrary.SelectedRows[0];

                int songId = Convert.ToInt32(
                    row.Cells["SongID"].Value);

                string songTitle = Convert.ToString(
                    row.Cells["Title"].Value) ?? "";

                int rating = Convert.ToInt32(
                    nudRating.Value);

                if (rating < 1 || rating > 5)
                {
                    MessageBox.Show(
                        "Rating must be between 1 and 5.",
                        "Music Library");
                    return;
                }

                if (_connection.State != ConnectionState.Open)
                {
                    _connection.Open();
                }

                using (var command = new MySqlCommand(
                    "CALL biblioteca_musical.sp_calificar_cancion(@songId, @rating);",
                    _connection))
                {
                    command.CommandType = CommandType.Text;

                    command.Parameters.Add(
                        "@songId",
                        MySqlDbType.Int32
                    ).Value = songId;

                    command.Parameters.Add(
                        "@rating",
                        MySqlDbType.Int32
                    ).Value = rating;

                    command.ExecuteNonQuery();
                }

                MessageBox.Show(
                    "'" + songTitle +
                    "' rated successfully!\nRating: " +
                    rating + "/5",
                    "Music Library");

                LoadLibrary();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(
                    "Could not rate the song: " + ex.Message,
                    "Music Library");
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error: " + ex.Message,
                    "Music Library");
            }
        }

        private void MyLibraryForm_Load(
            object sender, EventArgs e)
        {
        }
    }
}
