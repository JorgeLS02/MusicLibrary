
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;

namespace MusicLibrary
{
    // ==========================================
    // ARTIST OPTION
    // ==========================================

    // Representa un artista que aparecerá
    // en el ComboBox del formulario Reports.
    public sealed class ReportArtistOption
    {
        public int Id { get; }
        public string Name { get; }

        public ReportArtistOption(int id, string name)
        {
            Id = id;
            Name = name;
        }

        // El ComboBox muestra este texto.
        public override string ToString()
        {
            return Name + " (ID " + Id + ")";
        }
    }

    // ==========================================
    // REPORTS REPOSITORY (DAO)
    // ==========================================

    // Esta clase concentra el acceso a MySQL
    // relacionado con los reportes.
    //
    // Los formularios no necesitan conocer
    // las consultas SQL.
    public sealed class ReportsRepository
    {
        // Base de datos utilizada por el proyecto.
        private const string DatabaseName =
            "biblioteca_musical";

        // Reutiliza la conexión del usuario actual.
        private readonly MySqlConnection _connection;

        public ReportsRepository(
            MySqlConnection connection)
        {
            _connection = connection ??
                throw new ArgumentNullException(
                    nameof(connection));
        }

        // ==========================================
        // DATABASE CONNECTION
        // ==========================================

        // Garantiza que MySQL esté disponible
        // y que la base de datos sea la correcta.
        private void EnsureConnection()
        {
            if (_connection.State != ConnectionState.Open)
            {
                _connection.Open();
            }

            if (!string.Equals(
                _connection.Database,
                DatabaseName,
                StringComparison.OrdinalIgnoreCase))
            {
                _connection.ChangeDatabase(DatabaseName);
            }
        }

        // ==========================================
        // ADMIN ROLE
        // ==========================================

        // Verifica que el usuario conectado
        // tenga activo el rol administrador.
        public bool HasAdminRole()
        {
            EnsureConnection();

            // Activa los roles concedidos.
            using (var command = new MySqlCommand(
                "SET ROLE ALL;",
                _connection))
            {
                command.ExecuteNonQuery();
            }

            // Consulta los roles activos.
            using var roleCommand = new MySqlCommand(
                "SELECT CURRENT_ROLE();",
                _connection);

            string rolesText =
                Convert.ToString(
                    roleCommand.ExecuteScalar()) ?? "";

            string[] roles = rolesText.Split(
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
        // GET ARTISTS
        // ==========================================

        // Obtiene todos los artistas disponibles
        // para el reporte del procedimiento OUT.
        public List<ReportArtistOption> GetArtists()
        {
            EnsureConnection();

            List<ReportArtistOption> artists =
                new List<ReportArtistOption>();

            string query = @"
                SELECT
                    id_artista,
                    nombre
                FROM artistas
                ORDER BY nombre;";

            using var command = new MySqlCommand(
                query,
                _connection);

            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                int artistId =
                    Convert.ToInt32(
                        reader["id_artista"]);

                string artistName =
                    Convert.ToString(
                        reader["nombre"]) ?? "";

                artists.Add(
                    new ReportArtistOption(
                        artistId,
                        artistName));
            }

            return artists;
        }

        // ==========================================
        // GET REPORT
        // ==========================================

        // Ejecuta cualquiera de los primeros
        // siete reportes y devuelve un DataTable.
        public DataTable GetReport(
            int reportIndex,
            int minimumSongs)
        {
            EnsureConnection();

            // Obtiene la consulta SQL del reporte.
            string query = GetReportQuery(reportIndex);

            using var command = new MySqlCommand(
                query,
                _connection);

            // Artist Statistics utiliza HAVING.
            if (reportIndex == 0)
            {
                command.Parameters.Add(
                    "@minimumSongs",
                    MySqlDbType.Int32
                ).Value = minimumSongs;
            }

            // MySqlDataAdapter ejecuta la consulta
            // y obtiene los registros de MySQL.
            using var adapter =
                new MySqlDataAdapter(command);

            DataTable table = new DataTable();

            adapter.Fill(table);

            // Devuelve los registros al formulario.
            return table;
        }

        // ==========================================
        // REPORT SQL QUERIES
        // ==========================================

        // Todas las consultas de reportes
        // están ahora dentro de esta clase DAO.
        private string GetReportQuery(int index)
        {
            switch (index)
            {
                // ==================================
                // REPORT 1
                // GROUP BY + HAVING
                // ==================================

                case 0:
                    return @"
                        SELECT
                            a.nombre AS Artist,
                            COUNT(c.id_cancion) AS TotalSongs,
                            ROUND(
                                AVG(c.duracion), 2
                            ) AS AverageDuration,
                            SUM(c.duracion) AS TotalSeconds,
                            CAST(
                                SUM(c.duracion) / 60
                                AS DECIMAL(10,2)
                            ) AS TotalMinutes,
                            YEAR(
                                MAX(c.fecha_lanzamiento)
                            ) AS LatestReleaseYear
                        FROM artistas AS a
                        INNER JOIN canciones AS c
                            ON a.id_artista = c.id_artista
                        GROUP BY
                            a.id_artista,
                            a.nombre
                        HAVING COUNT(c.id_cancion)
                            >= @minimumSongs
                        ORDER BY
                            TotalSongs DESC,
                            Artist ASC;";

                // ==================================
                // REPORT 2
                // LEFT JOIN
                // ==================================

                case 1:
                    return @"
                        SELECT
                            a.id_artista AS ArtistID,
                            a.nombre AS Artist,
                            a.pais AS Country,
                            c.titulo AS Song,
                            c.genero AS Genre,
                            c.duracion AS Duration
                        FROM artistas AS a
                        LEFT JOIN canciones AS c
                            ON a.id_artista = c.id_artista
                        ORDER BY
                            a.nombre,
                            c.titulo;";

                // ==================================
                // REPORT 3
                // SUBQUERY
                // ==================================

                case 2:
                    return @"
                        SELECT
                            c.titulo AS Title,
                            a.nombre AS Artist,
                            c.duracion AS Duration,
                            (
                                SELECT ROUND(
                                    AVG(duracion), 2
                                )
                                FROM canciones
                            ) AS CatalogAverage
                        FROM canciones AS c
                        INNER JOIN artistas AS a
                            ON c.id_artista = a.id_artista
                        WHERE c.duracion > (
                            SELECT AVG(duracion)
                            FROM canciones
                        )
                        ORDER BY
                            c.duracion DESC;";

                // ==================================
                // REPORT 4
                // CORRELATED SUBQUERY
                // ==================================

                case 3:
                    return @"
                        SELECT
                            c.titulo AS Title,
                            a.nombre AS Artist,
                            c.duracion AS Duration
                        FROM canciones AS c
                        INNER JOIN artistas AS a
                            ON c.id_artista = a.id_artista
                        WHERE c.duracion > (
                            SELECT AVG(c2.duracion)
                            FROM canciones AS c2
                            WHERE c2.id_artista =
                                c.id_artista
                        )
                        ORDER BY
                            a.nombre,
                            c.titulo;";

                // ==================================
                // REPORT 5
                // UNION ALL
                // ==================================

                case 4:
                    return @"
                        SELECT
                            'Artist' AS RecordType,
                            nombre AS Name,
                            genero AS Genre
                        FROM artistas

                        UNION ALL

                        SELECT
                            'Song' AS RecordType,
                            titulo AS Name,
                            genero AS Genre
                        FROM canciones

                        ORDER BY
                            RecordType,
                            Name;";

                // ==================================
                // REPORT 6
                // UNION
                // ==================================

                case 5:
                    return @"
                        SELECT
                            genero AS Genre
                        FROM artistas

                        UNION

                        SELECT
                            genero AS Genre
                        FROM canciones

                        ORDER BY Genre;";

                // ==================================
                // REPORT 7
                // SQL FUNCTION
                // ==================================

                case 6:
                    return @"
                        SELECT
                            c.titulo AS Title,
                            a.nombre AS Artist,
                            c.duracion AS DurationSeconds,
                            fn_duracion_mmss(
                                c.duracion
                            ) AS FormattedDuration
                        FROM canciones AS c
                        INNER JOIN artistas AS a
                            ON c.id_artista = a.id_artista
                        ORDER BY
                            c.titulo;";

                default:
                    throw new InvalidOperationException(
                        "Unknown report type.");
            }
        }

        // ==========================================
        // STORED PROCEDURE WITH OUT
        // ==========================================

        // Ejecuta sp_contar_canciones_artista.
        //
        // Recibe el ID del artista (IN).
        // Devuelve su total de canciones (OUT).
        public int GetArtistSongCount(int artistId)
        {
            EnsureConnection();

            // Nombre completo del procedimiento.
            // Conservamos la corrección anterior.
            using var command = new MySqlCommand(
                "biblioteca_musical.sp_contar_canciones_artista",
                _connection);

            command.CommandType =
                CommandType.StoredProcedure;

            // ==================================
            // INPUT PARAMETER
            // ==================================

            MySqlParameter artistParameter =
                command.Parameters.Add(
                    "@p_id_artista",
                    MySqlDbType.Int32);

            artistParameter.Direction =
                ParameterDirection.Input;

            artistParameter.Value = artistId;

            // ==================================
            // OUTPUT PARAMETER
            // ==================================

            MySqlParameter totalParameter =
                command.Parameters.Add(
                    "@p_total",
                    MySqlDbType.Int32);

            totalParameter.Direction =
                ParameterDirection.Output;

            // ==================================
            // EXECUTE
            // ==================================

            // Ejecución del procedimiento real.
            // DatabaseConnection conserva
            // CheckParameters = false.
            command.ExecuteNonQuery();

            // Comprueba que OUT tenga un resultado.
            if (totalParameter.Value == null ||
                totalParameter.Value == DBNull.Value)
            {
                throw new InvalidOperationException(
                    "The procedure did not return a total.");
            }

            // Devuelve la cantidad a ReportsForm.
            return Convert.ToInt32(
                totalParameter.Value);
        }
    }
}
