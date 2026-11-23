-- 05_consultas.sql
-- Proyecto Biblioteca Musical | Consultas, filtros, JOIN y reportes.
-- Estas consultas son de solo lectura: se pueden volver a ejecutar.
-- Ejecutar en MySQL Workbench con musica_admin o root, salvo las vistas personales.

USE biblioteca_musical;

-- 1. Listados básicos.
SELECT id_artista, nombre, pais, genero FROM artistas ORDER BY nombre;
SELECT id_cancion, titulo, id_artista, duracion, genero
FROM canciones ORDER BY titulo;

-- 2. INNER JOIN: mostrar el artista al lado de cada canción.
SELECT c.id_cancion, c.titulo, a.nombre AS artista,
       c.genero, c.duracion, c.fecha_lanzamiento
FROM canciones AS c
INNER JOIN artistas AS a ON c.id_artista = a.id_artista
ORDER BY c.titulo;

-- 3. LEFT JOIN: incluir artistas que no tienen canciones.
-- Para los artistas sin canciones, c.titulo tendrá NULL.
SELECT a.id_artista, a.nombre AS artista, c.titulo AS cancion
FROM artistas AS a
LEFT JOIN canciones AS c ON a.id_artista = c.id_artista
ORDER BY a.id_artista, c.titulo;

-- 4. WHERE + LIKE: buscar por parte del título.
SELECT c.id_cancion, c.titulo, a.nombre AS artista, c.genero
FROM canciones AS c
INNER JOIN artistas AS a ON c.id_artista = a.id_artista
WHERE c.titulo LIKE '%Luce%';

-- 5. WHERE + BETWEEN: canciones entre 200 y 250 segundos (inclusive).
SELECT c.id_cancion, c.titulo, a.nombre AS artista, c.duracion
FROM canciones AS c
INNER JOIN artistas AS a ON c.id_artista = a.id_artista
WHERE c.duracion BETWEEN 200 AND 250
ORDER BY c.duracion ASC;

-- 6. WHERE + IN: filtrar una lista de géneros.
SELECT c.id_cancion, c.titulo, a.nombre AS artista, c.genero
FROM canciones AS c
INNER JOIN artistas AS a ON c.id_artista = a.id_artista
WHERE c.genero IN ('Pop', 'Rock', 'Electrónica')
ORDER BY c.genero, c.titulo;

-- 7. Subconsulta independiente en WHERE:
-- canciones que duran más que el promedio GENERAL del catálogo.
SELECT titulo, duracion
FROM canciones
WHERE duracion > (SELECT AVG(duracion) FROM canciones)
ORDER BY duracion DESC;

-- 8. Subconsulta correlacionada en WHERE:
-- canciones que superan el promedio de SU PROPIO artista.
SELECT c.titulo, a.nombre AS artista, c.duracion
FROM canciones AS c
INNER JOIN artistas AS a ON c.id_artista = a.id_artista
WHERE c.duracion > (
    SELECT AVG(c2.duracion)
    FROM canciones AS c2
    WHERE c2.id_artista = c.id_artista
)
ORDER BY a.nombre, c.titulo;

-- 9. Reporte con GROUP BY, HAVING, SUM, COUNT, AVG, CAST, YEAR y ORDER BY.
SELECT a.nombre AS artista,
       COUNT(c.id_cancion) AS total_canciones,
       ROUND(AVG(c.duracion), 2) AS duracion_promedio,
       SUM(c.duracion) AS duracion_total,
       CAST(SUM(c.duracion) / 60 AS DECIMAL(10,2)) AS total_minutos,
       YEAR(MAX(c.fecha_lanzamiento)) AS ultimo_anio
FROM artistas AS a
INNER JOIN canciones AS c ON a.id_artista = c.id_artista
GROUP BY a.id_artista, a.nombre
HAVING COUNT(c.id_cancion) >= 1
ORDER BY total_canciones DESC, artista ASC;

-- 10. UNION ALL: catálogo combinado de artistas y canciones.
SELECT 'Artista' AS tipo, nombre, genero
FROM artistas
UNION ALL
SELECT 'Cancion' AS tipo, titulo AS nombre, genero
FROM canciones
ORDER BY tipo, nombre;

-- 11. UNION: sin filas idénticas duplicadas.
SELECT genero FROM artistas
UNION
SELECT genero FROM canciones
ORDER BY genero;

-- 12. Función SQL: convertir segundos a minutos:segundos.
-- fn_duracion_mmss debe estar creada (archivo 10_funcion_trigger.sql).
SELECT titulo, duracion, fn_duracion_mmss(duracion) AS duracion_mmss
FROM canciones
ORDER BY titulo;

-- 13. Stored procedure con parámetro OUT, para estadísticas de artista.
-- Requiere sp_contar_canciones_artista (archivo 04_procedural.sql).
CALL sp_contar_canciones_artista(1, @total);
SELECT @total AS total_canciones_artista_1;

-- 14. Vista personal: ejecutar ESTA sección desde la conexión musica_u01.
-- Desde root, estas vistas se ven vacías porque no hay perfil root asociado.
-- SELECT * FROM vista_mi_perfil;
-- SELECT * FROM vista_mi_biblioteca;

-- 15. Consulta opcional de auditoría: ejecutar desde root.
-- SELECT * FROM bitacora_biblioteca ORDER BY id_evento DESC;
