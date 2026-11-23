# MusicLibrary — Guía de instalación y ejecución

Proyecto académico de Taller de Base de Datos. Aplicación de escritorio C# Windows Forms y base de datos MySQL `biblioteca_musical`, mediante ADO.NET (`MySql.Data`).

> **Importante:** Estas instrucciones sirven para una **instalación nueva o una base de prueba vacía**. NO vuelvas a ejecutar los scripts de creación o carga sobre la base con la que ya estás trabajando. Algunos archivos contienen INSERT con claves fijas, DROP/CREATE de procedimientos y operaciones de demostración que pueden alterar los datos.

## 1. Requisitos

- MySQL 8.0.16 o superior, con servicio activo, y MySQL Workbench.
- Visual Studio con soporte para el proyecto C# Windows Forms y el SDK .NET utilizado por la solución.
- Paquete NuGet `MySql.Data` instalado/restaurado.
- Una cuenta administradora de MySQL para **instalar** los scripts. La aplicación NO debe iniciar sesión con `root`; usa las cuentas de los perfiles configurados.

## 2. Archivos SQL del paquete

| Archivo | Propósito |
| --- | --- |
| `01_esquema.sql` | Base de datos y tablas `artistas`, `canciones` |
| `02_datos.sql` | 15 artistas y 15 canciones iniciales |
| `07_usuarios_biblioteca.sql` | Tablas `usuarios`, `biblioteca_canciones` |
| `08_datos_usuarios_biblioteca.sql` | 15 usuarios y 15 filas de bibliotecas iniciales |
| `10_funcion_trigger.sql` | Función `fn_duracion_mmss`, bitácora y trigger `trg_biblioteca_agregada` |
| `04_procedural.sql` | Procedimientos almacenados, incluida salida `OUT`, y vistas personales |
| `03_seguridad.sql` | Roles `rol_admin`, `rol_usuario`, permisos y cuentas de demostración |
| `05_consultas_iniciales.sql` | Consultas y reportes de lectura para evidencias |
| `06_concurrencia.sql` | Demostraciones COMMIT/ROLLBACK y niveles de aislamiento, **por bloques** |
| `09_pruebas_roles.sql` | Demostración de permisos. Algunas sentencias **modifican** biblioteca |
| `11_prueba_grant_revoke.sql` | Demostración de GRANT, REVOKE y rol temporal; ejecutar con una cuenta administradora de MySQL |

## 3. Instalación de SQL en una base NUEVA

Abre MySQL Workbench con una cuenta autorizada para crear esquemas, rutinas, funciones, triggers, usuarios y roles. Ejecuta cada archivo **completo** en este orden; verifica que no existan errores antes de continuar:

1. `01_esquema.sql`
2. `02_datos.sql`
3. `07_usuarios_biblioteca.sql`
4. `08_datos_usuarios_biblioteca.sql`
5. `10_funcion_trigger.sql`
6. `04_procedural.sql`
7. `03_seguridad.sql`

**Antes del paso 7:** reemplaza las contraseñas de ejemplo `REEMPLAZAR_POR_CLAVE_SEGURA_ADMIN` y `REEMPLAZAR_POR_CLAVE_SEGURA_USUARIO` en una copia local de `03_seguridad.sql` por contraseñas seguras. Nunca publiques claves reales en el repositorio. Las cuentas incluidas están definidas para `@'localhost'`; si tu servidor utiliza otra resolución de host, verifica el host efectivo y ajusta las cuentas/permisos para ese entorno, sin ampliar privilegios innecesariamente.

**Por qué este orden:** las canciones dependen de artistas; las filas de `biblioteca_canciones` dependen de usuarios y canciones; el trigger depende de esa tabla; los roles y sus GRANT de procedimientos, vistas y función requieren que estos objetos ya existan.

La bitácora se crea **después** de cargar las 15 filas iniciales de `biblioteca_canciones`. Por eso esas filas de carga no generan eventos de auditoría; el trigger registra los nuevos INSERT posteriores a su creación.

### Verificación de datos iniciales

En la base recién creada:

```sql
USE biblioteca_musical;
SELECT 'artistas' AS tabla, COUNT(*) AS registros FROM artistas
UNION ALL
SELECT 'canciones', COUNT(*) FROM canciones
UNION ALL
SELECT 'usuarios', COUNT(*) FROM usuarios
UNION ALL
SELECT 'biblioteca_canciones', COUNT(*) FROM biblioteca_canciones;
```

El resultado de una instalación inicial limpia debe ser **15 registros en cada una**, 60 en total. Si agregaste registros durante las pruebas, los totales serán superiores.

## 4. Ejecutar MusicLibrary en Visual Studio

1. Abre `app/MusicLibrary.slnx` en Visual Studio (es un archivo de solución `.slnx`).
2. Verifica que se restaure el paquete NuGet `MySql.Data`.
3. Asegúrate de que `appsettings.json` exista y esté configurado con tu servidor y base de datos. Valores típicos de una instalación local:

```json
{
  "MySql": {
    "Server": "127.0.0.1",
    "Port": 3306,
    "Database": "biblioteca_musical"
  }
}
```

4. Configura el archivo para que se copie al directorio de salida si el proyecto no lo hace ya. La clase `DatabaseConnection` lo busca desde `AppContext.BaseDirectory`.
5. Conserva la corrección de conexión `CheckParameters = false` en `DatabaseConnection.cs`, utilizada para el procedimiento almacenado con `IN` y `OUT`.
6. Usa **Build > Rebuild Solution**, después **F5**.
7. Inicia sesión con `musica_admin` o `musica_u01` y la contraseña configurada para cada cuenta. No uses `root` desde la aplicación.

## 5. Demostración recomendada

- **Usuario normal (`musica_u01`):** buscar canciones por texto, duración y género; agregar una canción a My Library; comprobar que Admin Panel esté oculto; ejecutar Test Permissions para demostrar el error 1142 manejado por C#.
- **Administrador (`musica_admin`):** abrir Admin Panel; CRUD de artistas y canciones; reportes incluyendo `sp_contar_canciones_artista` con parámetro OUT; Test Permissions con lectura permitida.
- **Transacción desde la aplicación:** mostrar COMMIT y ROLLBACK de operaciones sobre artistas y canciones, verificando el efecto en las dos tablas.
- **Trigger:** agregar una canción y consultar `bitacora_biblioteca` desde una cuenta autorizada para demostrar la entrada automática.
- **Concurrencia:** usar `06_concurrencia.sql` **por etapas y con dos sesiones abiertas**; nunca ejecutar el archivo completo seguido.

## 6. Archivos SQL adicionales: ejecutar con cautela

- `05_consultas_iniciales.sql`: consultas de lectura para demostrar filtros y reportes; las secciones sobre vistas deben ejecutarse con el usuario correspondiente.
- `06_concurrencia.sql`: incluye `INSERT`, `UPDATE`, `COMMIT`, `ROLLBACK` y cambios de nivel de aislamiento. Las pruebas requieren sesiones A/B, respetar pausas, y pueden modificar datos.
- `09_pruebas_roles.sql`: aunque es para pruebas, contiene `CALL` que agrega, califica y quita canciones. Ejecuta solo las operaciones que corresponda probar y evita repetirlas sobre la base con datos importantes.

## 7. Estructura sugerida para entregar

```text
MusicLibrary_Entrega/
├── sql/
│   ├── 01_esquema.sql
│   ├── 02_datos.sql
│   ├── 03_seguridad.sql
│   ├── 04_procedural.sql
│   ├── 05_consultas_iniciales.sql
│   ├── 06_concurrencia.sql
│   ├── 07_usuarios_biblioteca.sql
│   ├── 08_datos_usuarios_biblioteca.sql
│   ├── 09_pruebas_roles.sql
│   ├── 10_funcion_trigger.sql
│   └── 11_prueba_grant_revoke.sql
├── app/
│   ├── MusicLibrary.slnx
│   └── MusicLibrary/ (código C#, diseñadores, .csproj, .resx y appsettings.json)
├── docs/
│   ├── Reporte_MusicLibrary.pdf
│   ├── Diccionario_de_Datos_MusicLibrary.pdf
│   ├── Diagrama_ER_MusicLibrary.png
│   └── MusicLibrary_Modelo.mwb
└── README.md
```

**No incluyas** contraseñas reales ni carpetas temporales como `bin/` y `obj/` si no son necesarias para abrir el proyecto.

## 8. Pendientes de revisión antes de entregar

- Comprobar que el proyecto final de Visual Studio está completo y compila tras descomprimirlo en otro directorio.
- Validar instalación completa en un entorno de prueba, sin tocar la base actual (este README se preparó mediante revisión estática de los scripts).
- El script `11_prueba_grant_revoke.sql` documenta `GRANT` y `REVOKE`. Ejecútalo solamente si necesitas repetir la demostración; el script crea y elimina un rol temporal.
- Revisar el alcance del CRUD exigido para cada tabla y documentar la gestión de perfiles/bibliotecas.
- Verificar que el reporte de `docs/` contiene el diagrama, diccionario de datos, matriz de permisos, comparación JDBC/ADO.NET, capturas de las pruebas y conclusión individual.
- Subir estas carpetas a un repositorio GitHub y conservar evidencia del commit.
- La guía de estudio para la exposición es un documento adicional; se preparará por separado.

---

**Estado de verificación:** orden y contenido de los once scripts revisados; ejecución de instalación limpia y compilación de la solución final **pendientes de prueba en el entorno del usuario**.

## 9. Publicación en GitHub

Sube el **contenido extraído** de este paquete (`app/`, `sql/`, `docs/`, `README.md` y `.gitignore`), no el archivo `.zip` o `.rar` como único elemento. Evita publicar `.vs/`, `bin/`, `obj/`, contraseñas reales, cachés o archivos `.user`. El repositorio permite consultar el código y su historial de commits.
