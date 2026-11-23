
using MySql.Data.MySqlClient;
using System;
using System.IO;
using System.Text.Json;

namespace MusicLibrary
{
    public static class DatabaseConnection
    {
        // ==========================================
        // CREATE DATABASE CONNECTION
        // ==========================================

        // Crea una conexión a MySQL utilizando
        // las credenciales del usuario que inició sesión.
        public static MySqlConnection CreateConnection(
            string username, string password)
        {
            // Busca el archivo de configuración.
            string path = Path.Combine(
                AppContext.BaseDirectory,
                "appsettings.json");

            // Lee las configuraciones de MySQL.
            using var document = JsonDocument.Parse(
                File.ReadAllText(path));

            var settings = document.RootElement
                .GetProperty("MySql");

            // Construye la cadena de conexión.
            var builder = new MySqlConnectionStringBuilder
            {
                // Dirección del servidor MySQL.
                Server = settings.GetProperty("Server")
                    .GetString(),

                // Puerto del servidor.
                Port = settings.GetProperty("Port")
                    .GetUInt32(),

                // Nombre de la base de datos.
                Database = settings.GetProperty("Database")
                    .GetString(),

                // Usuario que inicia sesión.
                UserID = username,

                // Contraseña proporcionada en Login.
                Password = password,

                // ==================================
                // STORED PROCEDURE FIX
                // ==================================

                // Evita que MySql.Data intente
                // consultar automáticamente los
                // metadatos de los parámetros
                // de los procedimientos almacenados.
                //
                // Esto permite utilizar procedimientos
                // con parámetros IN y OUT incluso
                // cuando la cuenta no puede consultar
                // sus metadatos.
                //
                // Los parámetros deben declararse
                // con los tipos y en el orden correcto.
                CheckParameters = false
            };

            // Devuelve la conexión configurada.
            return new MySqlConnection(
                builder.ConnectionString);
        }
    }
}
