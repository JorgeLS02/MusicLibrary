-- 06_concurrencia.sql
-- Proyecto Biblioteca Musical | Evidencias de transacciones y concurrencia.
-- ADVERTENCIA: NO ejecutar todo el archivo de una sola vez.
-- Ejecutar SOLO EL BLOQUE indicado, en la conexión correspondiente,
-- respetando el orden de las etapas. Mantener ambas conexiones ABIERTAS.
-- Conexión A: musica_admin (Administrador Biblioteca)
-- Conexión B: root (Local instance MySQL80)
-- Los ejemplos de concurrencia utilizan id_cancion = 1, que viene en 02_datos.sql.
-- Si falta esta canción, primero cargar 01_esquema.sql y 02_datos.sql en BD nueva.

-- ============================================================
-- PARTE I: TRANSACCIONES COMMIT / ROLLBACK
-- ============================================================

-- I-A. ROLLBACK. Ejecutar TODO ESTE BLOQUE desde musica_admin.
-- Son dos inserciones en dos tablas distintas. ROLLBACK revierte ambas.
USE biblioteca_musical;
START TRANSACTION;
INSERT INTO artistas (nombre, pais, genero, fecha_registro)
VALUES ('Artista Demo Rollback', 'Mexico', 'Rock', CURDATE());
SET @id_artista_rollback = LAST_INSERT_ID();
INSERT INTO canciones (titulo, id_artista, duracion, genero, fecha_lanzamiento)
VALUES ('Cancion Demo Rollback', @id_artista_rollback, 210, 'Rock', CURDATE());
SELECT * FROM canciones WHERE id_artista = @id_artista_rollback;
ROLLBACK;
SELECT * FROM artistas WHERE id_artista = @id_artista_rollback;
SELECT * FROM canciones WHERE id_artista = @id_artista_rollback;
-- Las dos últimas consultas deben devolver 0 filas.

-- I-B. COMMIT. Ejecutar TODO ESTE BLOQUE una sola vez desde musica_admin.
-- Mantiene dos inserciones en dos tablas distintas.
USE biblioteca_musical;
START TRANSACTION;
INSERT INTO artistas (nombre, pais, genero, fecha_registro)
VALUES ('Artista Demo Commit SQL', 'Mexico', 'Pop', CURDATE());
SET @id_artista_commit_demo = LAST_INSERT_ID();
INSERT INTO canciones (titulo, id_artista, duracion, genero, fecha_lanzamiento)
VALUES ('Cancion Demo Commit SQL', @id_artista_commit_demo, 240, 'Pop', CURDATE());
COMMIT;
SELECT * FROM artistas WHERE id_artista = @id_artista_commit_demo;
SELECT * FROM canciones WHERE id_artista = @id_artista_commit_demo;
-- Las dos últimas consultas deben devolver 1 fila cada una.
-- No repetir I-B sin limpiar/renombrar los registros: hay restricciones UNIQUE.

-- ============================================================
-- PARTE II: READ COMMITTED - LECTURA NO REPETIBLE
-- Seguir las etapas II-0 a II-3 en este orden, alternando conexiones.
-- No ejecutar todos estos bloques juntos.
-- ============================================================

-- II-0. Conexión B (root): preparar la duración inicial de la canción 1.
USE biblioteca_musical;
UPDATE canciones SET duracion = 215 WHERE id_cancion = 1;
SELECT id_cancion, titulo, duracion FROM canciones WHERE id_cancion = 1;

-- II-1. Conexión A (musica_admin): INICIAR y dejar abierta la transacción.
USE biblioteca_musical;
SET SESSION TRANSACTION ISOLATION LEVEL READ COMMITTED;
START TRANSACTION;
SELECT id_cancion, titulo, duracion FROM canciones WHERE id_cancion = 1;
-- Debe mostrar 215. Todavía NO hacer COMMIT.

-- II-2. Conexión B (root): modificar y confirmar el cambio.
USE biblioteca_musical;
UPDATE canciones SET duracion = 225 WHERE id_cancion = 1;
SELECT id_cancion, titulo, duracion FROM canciones WHERE id_cancion = 1;
-- Debe mostrar 225. En autocommit=1, UPDATE se confirma automáticamente.

-- II-3. Conexión A (musica_admin): consultar OTRA VEZ sin reiniciar transacción.
SELECT id_cancion, titulo, duracion FROM canciones WHERE id_cancion = 1;
-- Debe mostrar 225: cambió de 215 a 225 dentro de la misma transacción.
COMMIT;

-- ============================================================
-- PARTE III: SERIALIZABLE - BLOQUEO DE ESCRITURA
-- Seguir las etapas III-1 a III-4 en este orden.
-- No ejecutar todas juntas ni cerrar las conexiones antes de terminar.
-- ============================================================

-- III-1. Conexión A (musica_admin): iniciar transacción SERIALIZABLE.
USE biblioteca_musical;
SET SESSION TRANSACTION ISOLATION LEVEL SERIALIZABLE;
START TRANSACTION;
SELECT id_cancion, titulo, duracion FROM canciones WHERE id_cancion = 1;
-- Debe mostrar 225. NO ejecutar COMMIT todavía.

-- III-2. Conexión B (root): probar una escritura conflictiva.
USE biblioteca_musical;
SET SESSION innodb_lock_wait_timeout = 5;
UPDATE canciones SET duracion = 235 WHERE id_cancion = 1;
-- ERROR esperado: 1205, Lock wait timeout exceeded.
-- ¡Es una prueba exitosa de bloqueo, no un fallo del proyecto!

-- III-3. Conexión A (musica_admin): liberar el bloqueo.
COMMIT;

-- III-4. Conexión B (root): repetir después de liberar.
UPDATE canciones SET duracion = 235 WHERE id_cancion = 1;
SELECT id_cancion, titulo, duracion FROM canciones WHERE id_cancion = 1;
-- Debe mostrar 235.

-- ============================================================
-- PARTE IV: LIMPIEZA DE LA PRUEBA
-- ============================================================

-- IV-1. Conexión B (root): devolver canción 1 a su duración original.
UPDATE canciones SET duracion = 215 WHERE id_cancion = 1;

-- IV-2. Conexión A (musica_admin): restaurar aislamiento predeterminado usual.
SET SESSION TRANSACTION ISOLATION LEVEL REPEATABLE READ;

-- NOTA PARA LA APP C#:
-- El PDF exige una transacción de ADO.NET que toque al menos 2 tablas y
-- maneje rollback ante errores. Esta parte debe implementarse desde C#;
-- el script solo documenta y permite demostrar el comportamiento SQL.
