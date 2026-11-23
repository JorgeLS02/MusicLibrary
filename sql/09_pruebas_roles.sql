-- 09_pruebas_roles.sql
-- EJECUTAR con la conexión de MySQL «musica_u01», no con root ni administrador.
-- No contiene sentencias peligrosas: prueba solo su propia biblioteca.
USE biblioteca_musical;

-- 1. Consultar catálogo general (permitido para rol_usuario).
SELECT id_cancion, titulo, genero FROM canciones ORDER BY titulo;

-- 2. Ver las canciones del usuario autenticado.
SELECT * FROM vista_mi_biblioteca ORDER BY fecha_agregado DESC;

-- 3. Agregar canción 4 a su biblioteca (ejecutar una sola vez).
CALL sp_agregar_a_biblioteca(4);

-- 4. Marcarla como favorita y calificarla.
CALL sp_marcar_favorita(4, TRUE);
CALL sp_calificar_cancion(4, 5);
SELECT * FROM vista_mi_biblioteca WHERE id_cancion = 4;

-- 5. Quitarla de su biblioteca (NO elimina la canción del catálogo).
CALL sp_quitar_de_biblioteca(4);
SELECT * FROM vista_mi_biblioteca WHERE id_cancion = 4;

-- 6. PRUEBA DE PERMISOS: ejecutar la sentencia siguiente por separado.
-- Debe producir ERROR 1142: no puedes insertar artistas desde rol_usuario.
-- INSERT INTO artistas (nombre, pais, genero, fecha_registro)
-- VALUES ('Sin permiso', 'México', 'Pop', CURRENT_DATE());
