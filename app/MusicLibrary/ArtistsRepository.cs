using MySql.Data.MySqlClient;
using System;
using System.Data;

namespace MusicLibrary
{
    // ==========================================
    // ARTISTS REPOSITORY (DAO)
    // ==========================================

    // Esta clase contiene las consultas SQL de Artists.
    // No tiene controles visuales ni muestra MessageBox.
    // Reutiliza la conexión de AdminForm, sin cerrarla.
    public sealed class ArtistsRepository
    {
        private readonly MySqlConnection _connection;

        public ArtistsRepository(MySqlConnection connection)
        {
            _connection = connection ??
                throw new ArgumentNullException(nameof(connection));
        }

        // Abre la conexión compartida únicamente si es necesario.
        private void EnsureConnection()
        {
            if (_connection.State != ConnectionState.Open)
            {
                _connection.Open();
            }
        }

        // ==========================================
        // READ ARTISTS
        // ==========================================

        // Devuelve los artistas con los mismos nombres de
        // columnas que utiliza dgvArtists en AdminForm.
        public DataTable GetArtists()
        {
            EnsureConnection();

            string query = @"
                SELECT
                    id_artista AS ArtistID,
                    nombre AS Name,
                    pais AS Country,
                    genero AS Genre,
                    fecha_registro AS RegistrationDate
                FROM artistas
                ORDER BY nombre;";

            using var command = new MySqlCommand(query, _connection);
            using var adapter = new MySqlDataAdapter(command);

            DataTable table = new DataTable();
            adapter.Fill(table);
            return table;
        }

        // ==========================================
        // SAFE PARAMETERS
        // ==========================================

        // Parametriza todos los valores para evitar
        // concatenar entradas del usuario dentro del SQL.
        private static void AddArtistParameters(
            MySqlCommand command,
            string name,
            string country,
            string genre,
            DateTime registrationDate)
        {
            command.Parameters.Add(
                "@name", MySqlDbType.VarChar).Value = name;
            command.Parameters.Add(
                "@country", MySqlDbType.VarChar).Value = country;
            command.Parameters.Add(
                "@genre", MySqlDbType.VarChar).Value = genre;
            command.Parameters.Add(
                "@date", MySqlDbType.Date).Value =
                registrationDate.Date;
        }

        // ==========================================
        // CREATE ARTIST
        // ==========================================

        public int AddArtist(
            string name,
            string country,
            string genre,
            DateTime registrationDate)
        {
            EnsureConnection();

            string query = @"
                INSERT INTO artistas
                    (nombre, pais, genero, fecha_registro)
                VALUES
                    (@name, @country, @genre, @date);";

            using var command = new MySqlCommand(query, _connection);
            AddArtistParameters(
                command, name, country, genre, registrationDate);

            return command.ExecuteNonQuery();
        }

        // ==========================================
        // UPDATE ARTIST
        // ==========================================

        public int UpdateArtist(
            int artistId,
            string name,
            string country,
            string genre,
            DateTime registrationDate)
        {
            EnsureConnection();

            string query = @"
                UPDATE artistas
                SET
                    nombre = @name,
                    pais = @country,
                    genero = @genre,
                    fecha_registro = @date
                WHERE id_artista = @id;";

            using var command = new MySqlCommand(query, _connection);
            AddArtistParameters(
                command, name, country, genre, registrationDate);
            command.Parameters.Add(
                "@id", MySqlDbType.Int32).Value = artistId;

            return command.ExecuteNonQuery();
        }

        // ==========================================
        // CHECK RELATED SONGS
        // ==========================================

        // Protege al artista contra borrado cuando tiene
        // canciones asociadas. MySQL también aplica la FK.
        public long CountSongsByArtist(int artistId)
        {
            EnsureConnection();

            string query = @"
                SELECT COUNT(*)
                FROM canciones
                WHERE id_artista = @id;";

            using var command = new MySqlCommand(query, _connection);
            command.Parameters.Add(
                "@id", MySqlDbType.Int32).Value = artistId;

            return Convert.ToInt64(command.ExecuteScalar());
        }

        // ==========================================
        // DELETE ARTIST
        // ==========================================

        public int DeleteArtist(int artistId)
        {
            EnsureConnection();

            string query = @"
                DELETE FROM artistas
                WHERE id_artista = @id;";

            using var command = new MySqlCommand(query, _connection);
            command.Parameters.Add(
                "@id", MySqlDbType.Int32).Value = artistId;

            return command.ExecuteNonQuery();
        }
    }
}
