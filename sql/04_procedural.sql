-- 04_procedural.sql
-- Ejecutar como administrador de la BD DESPUÉS de 07 y 08.
-- Para que cada usuario solo gestione SU biblioteca, los procedimientos
-- identifican la cuenta MySQL de la sesión con USER().
-- SQL SECURITY DEFINER usa permisos del creador del procedimiento;
-- USER() identifica al cliente conectado, no al DEFINER.
USE biblioteca_musical;

DROP PROCEDURE IF EXISTS sp_agregar_a_biblioteca;
DROP PROCEDURE IF EXISTS sp_quitar_de_biblioteca;
DROP PROCEDURE IF EXISTS sp_marcar_favorita;
DROP PROCEDURE IF EXISTS sp_calificar_cancion;
DROP PROCEDURE IF EXISTS sp_contar_canciones_artista;

DELIMITER $$

CREATE PROCEDURE sp_agregar_a_biblioteca(IN p_id_cancion INT)
SQL SECURITY DEFINER
BEGIN
    DECLARE v_id_usuario INT DEFAULT NULL;
    SELECT id_usuario INTO v_id_usuario
    FROM usuarios
    WHERE usuario_mysql = SUBSTRING_INDEX(USER(), '@', 1) AND activo = TRUE
    LIMIT 1;

    IF v_id_usuario IS NULL THEN
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Usuario sin perfil activo';
    END IF;

    IF NOT EXISTS (SELECT 1 FROM canciones WHERE id_cancion = p_id_cancion) THEN
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'La cancion no existe';
    END IF;

    INSERT INTO biblioteca_canciones (id_usuario, id_cancion)
    VALUES (v_id_usuario, p_id_cancion);
END$$

CREATE PROCEDURE sp_quitar_de_biblioteca(IN p_id_cancion INT)
SQL SECURITY DEFINER
BEGIN
    DECLARE v_id_usuario INT DEFAULT NULL;
    SELECT id_usuario INTO v_id_usuario
    FROM usuarios
    WHERE usuario_mysql = SUBSTRING_INDEX(USER(), '@', 1) AND activo = TRUE
    LIMIT 1;

    IF v_id_usuario IS NULL THEN
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Usuario sin perfil activo';
    END IF;

    DELETE FROM biblioteca_canciones
    WHERE id_usuario = v_id_usuario AND id_cancion = p_id_cancion;
END$$

CREATE PROCEDURE sp_marcar_favorita(
    IN p_id_cancion INT,
    IN p_favorita BOOLEAN
)
SQL SECURITY DEFINER
BEGIN
    DECLARE v_id_usuario INT DEFAULT NULL;
    SELECT id_usuario INTO v_id_usuario
    FROM usuarios
    WHERE usuario_mysql = SUBSTRING_INDEX(USER(), '@', 1) AND activo = TRUE
    LIMIT 1;

    IF v_id_usuario IS NULL THEN
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Usuario sin perfil activo';
    END IF;

    UPDATE biblioteca_canciones
    SET favorito = p_favorita
    WHERE id_usuario = v_id_usuario AND id_cancion = p_id_cancion;
END$$

CREATE PROCEDURE sp_calificar_cancion(
    IN p_id_cancion INT,
    IN p_calificacion TINYINT
)
SQL SECURITY DEFINER
BEGIN
    DECLARE v_id_usuario INT DEFAULT NULL;
    SELECT id_usuario INTO v_id_usuario
    FROM usuarios
    WHERE usuario_mysql = SUBSTRING_INDEX(USER(), '@', 1) AND activo = TRUE
    LIMIT 1;

    IF v_id_usuario IS NULL THEN
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Usuario sin perfil activo';
    END IF;

    IF p_calificacion IS NOT NULL AND NOT (p_calificacion BETWEEN 1 AND 5) THEN
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Calificacion fuera de rango';
    END IF;

    UPDATE biblioteca_canciones
    SET calificacion = p_calificacion
    WHERE id_usuario = v_id_usuario AND id_cancion = p_id_cancion;
END$$


-- Procedimiento de reporte con parámetro de salida OUT y manejo de errores.
CREATE PROCEDURE sp_contar_canciones_artista(
    IN p_id_artista INT,
    OUT p_total INT
)
SQL SECURITY DEFINER
BEGIN
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        SET p_total = -1;
        RESIGNAL;
    END;

    IF p_id_artista IS NULL OR NOT EXISTS (
        SELECT 1 FROM artistas WHERE id_artista = p_id_artista
    ) THEN
        SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT = 'El artista no existe';
    END IF;

    SELECT COUNT(*) INTO p_total
    FROM canciones
    WHERE id_artista = p_id_artista;
END$$

DELIMITER ;

-- Vista: cada persona solo consulta su propio contenido.
CREATE OR REPLACE SQL SECURITY DEFINER VIEW vista_mi_biblioteca AS
SELECT bc.id_biblioteca,
       c.id_cancion, c.titulo, a.nombre AS artista,
       c.genero, c.duracion,
       bc.fecha_agregado, bc.favorito, bc.calificacion
FROM biblioteca_canciones AS bc
INNER JOIN usuarios AS u ON u.id_usuario = bc.id_usuario
INNER JOIN canciones AS c ON c.id_cancion = bc.id_cancion
INNER JOIN artistas AS a ON a.id_artista = c.id_artista
WHERE u.usuario_mysql = SUBSTRING_INDEX(USER(), '@', 1)
  AND u.activo = TRUE;

-- Vista: cada usuario puede ver solo los datos de su propio perfil.
CREATE OR REPLACE SQL SECURITY DEFINER VIEW vista_mi_perfil AS
SELECT id_usuario, usuario_mysql, nombre, correo, fecha_registro
FROM usuarios
WHERE usuario_mysql = SUBSTRING_INDEX(USER(), '@', 1)
  AND activo = TRUE;

-- Para probar el procedimiento OUT (desde root, o rol_admin con EXECUTE):
-- CALL sp_contar_canciones_artista(1, @total);
-- SELECT @total AS total_canciones;
-- Error esperado para artista inexistente (ejecutar por separado):
-- CALL sp_contar_canciones_artista(99999, @total);
