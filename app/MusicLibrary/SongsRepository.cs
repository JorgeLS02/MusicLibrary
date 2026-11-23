
using MySql.Data.MySqlClient;
using System;
using System.Data;

namespace MusicLibrary
{
    // ==========================================
    // SONGS REPOSITORY (DAO)
    // ==========================================

    // Centraliza las consultas SQL para las canciones.
    // No utiliza controles de Windows Forms y no muestra mensajes.
    // Reutiliza la conexión del usuario administrador sin cerrarla.
    public sealed class SongsRepository
    {
        private readonly MySqlConnection _connection;

        public SongsRepository(MySqlConnection connection)
        {
            _connection = connection ??
                throw new ArgumentNullException(nameof(connection));
        }

        // Solo abre la conexión si no está abierta.
        private void EnsureConnection()
        {
            if (_connection.State != ConnectionState.Open)
            {
                _connection.Open();
            }
        }

        // ==========================================
        // ARTISTS FOR SONGS COMBOBOX
        // ==========================================

        // Carga las opciones de artistas para cmbSongArtist.
        // Conserva los nombres ArtistID y Name del formulario.
        public DataTable GetArtistOptions()
        {
            EnsureConnection();

            string query = @"
                SELECT
                    id_artista AS ArtistID,
                    nombre AS Name
                FROM artistas
                ORDER BY nombre;";

            using var command = new MySqlCommand(query, _connection);
            using var adapter = new MySqlDataAdapter(command);

            DataTable table = new DataTable();
            adapter.Fill(table);
            return table;
        }

        // ==========================================
        // READ SONGS
        // ==========================================

        // Devuelve todas las canciones con su artista.
        // Los alias coinciden con las columnas de dgvAdminSongs.
        public DataTable GetSongs()
        {
            EnsureConnection();

            string query = @"
                SELECT
                    c.id_cancion AS SongID,
                    c.titulo AS Title,
                    c.id_artista AS ArtistID,
                    a.nombre AS Artist,
                    c.genero AS Genre,
                    c.duracion AS Duration,
                    c.fecha_lanzamiento AS ReleaseDate
                FROM canciones AS c
                INNER JOIN artistas AS a
                    ON c.id_artista = a.id_artista
                ORDER BY c.titulo;";

            using var command = new MySqlCommand(query, _connection);
            using var adapter = new MySqlDataAdapter(command);

            DataTable table = new DataTable();
            adapter.Fill(table);
            return table;
        }

        // ==========================================
        // SQL PARAMETERS
        // ==========================================

        // Todos los datos que introduce el usuario son parámetros.
        // No concatenamos entradas del usuario en sentencias SQL.
        private static void AddSongParameters(
            MySqlCommand command,
            string title,
            int artistId,
            int durationSeconds,
            string genre,
            DateTime releaseDate)
        {
            command.Parameters.Add(
                "@title", MySqlDbType.VarChar).Value = title;

            command.Parameters.Add(
                "@artistId", MySqlDbType.Int32).Value = artistId;

            command.Parameters.Add(
                "@duration", MySqlDbType.Int32).Value = durationSeconds;

            command.Parameters.Add(
                "@genre", MySqlDbType.VarChar).Value = genre;

            command.Parameters.Add(
                "@releaseDate", MySqlDbType.Date).Value =
                releaseDate.Date;
        }

        // ==========================================
        // CREATE SONG
        // ==========================================

        public int AddSong(
            string title,
            int artistId,
            int durationSeconds,
            string genre,
            DateTime releaseDate)
        {
            EnsureConnection();

            string query = @"
                INSERT INTO canciones
                    (titulo, id_artista, duracion, genero, fecha_lanzamiento)
                VALUES
                    (@title, @artistId, @duration, @genre, @releaseDate);";

            using var command = new MySqlCommand(query, _connection);

            AddSongParameters(
                command, title, artistId, durationSeconds,
                genre, releaseDate);

            return command.ExecuteNonQuery();
        }

        // ==========================================
        // UPDATE SONG
        // ==========================================

        public int UpdateSong(
            int songId,
            string title,
            int artistId,
            int durationSeconds,
            string genre,
            DateTime releaseDate)
        {
            EnsureConnection();

            string query = @"
                UPDATE canciones
                SET
                    titulo = @title,
                    id_artista = @artistId,
                    duracion = @duration,
                    genero = @genre,
                    fecha_lanzamiento = @releaseDate
                WHERE id_cancion = @songId;";

            using var command = new MySqlCommand(query, _connection);

            AddSongParameters(
                command, title, artistId, durationSeconds,
                genre, releaseDate);

            command.Parameters.Add(
                "@songId", MySqlDbType.Int32).Value = songId;

            return command.ExecuteNonQuery();
        }

        // ==========================================
        // CHECK REFERENCES BEFORE DELETE
        // ==========================================

        // Evita eliminar canciones almacenadas por algún usuario.
        // La clave foránea de MySQL también protege esa relación.
        public long CountLibrariesForSong(int songId)
        {
            EnsureConnection();

            string query = @"
                SELECT COUNT(*)
                FROM biblioteca_canciones
                WHERE id_cancion = @songId;";

            using var command = new MySqlCommand(query, _connection);

            command.Parameters.Add(
                "@songId", MySqlDbType.Int32).Value = songId;

            return Convert.ToInt64(command.ExecuteScalar());
        }

        // ==========================================
        // DELETE SONG
        // ==========================================

        public int DeleteSong(int songId)
        {
            EnsureConnection();

            string query = @"
                DELETE FROM canciones
                WHERE id_cancion = @songId;";

            using var command = new MySqlCommand(query, _connection);

            command.Parameters.Add(
                "@songId", MySqlDbType.Int32).Value = songId;

            return command.ExecuteNonQuery();
        }
    }
}
