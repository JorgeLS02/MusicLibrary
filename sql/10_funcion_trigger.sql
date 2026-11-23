-- 10_funcion_trigger.sql
-- Complemento del proyecto individual: 1 función y 1 trigger de auditoría.
-- Ejecutar después de 07_usuarios_biblioteca.sql.
USE biblioteca_musical;

DROP FUNCTION IF EXISTS fn_duracion_mmss;
DELIMITER $$
CREATE FUNCTION fn_duracion_mmss(p_segundos INT)
RETURNS VARCHAR(16)
DETERMINISTIC
NO SQL
BEGIN
    IF p_segundos IS NULL OR p_segundos < 0 THEN
        RETURN NULL;
    END IF;
    RETURN CONCAT(FLOOR(p_segundos / 60), ':',
                  LPAD(MOD(p_segundos, 60), 2, '0'));
END$$
DELIMITER ;

-- La tabla de auditoría no cuenta entre las tablas de negocio mínimas.
CREATE TABLE IF NOT EXISTS bitacora_biblioteca (
    id_evento INT AUTO_INCREMENT PRIMARY KEY,
    id_usuario INT NOT NULL,
    id_cancion INT NOT NULL,
    operacion VARCHAR(30) NOT NULL,
    cuenta_mysql VARCHAR(80) NOT NULL,
    fecha_evento DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP
) ENGINE=InnoDB;

DROP TRIGGER IF EXISTS trg_biblioteca_agregada;
DELIMITER $$
CREATE TRIGGER trg_biblioteca_agregada
AFTER INSERT ON biblioteca_canciones
FOR EACH ROW
BEGIN
    INSERT INTO bitacora_biblioteca
        (id_usuario, id_cancion, operacion, cuenta_mysql)
    VALUES
        (NEW.id_usuario, NEW.id_cancion, 'AGREGAR_CANCION',
         SUBSTRING_INDEX(USER(), '@', 1));
END$$
DELIMITER ;

-- Prueba de la función (ejecutar con cuenta que pueda consultar canciones):
SELECT titulo, fn_duracion_mmss(duracion) AS duracion_mmss
FROM canciones;
-- Para probar el trigger: conectarse como musica_u01 y ejecutar
-- CALL sp_agregar_a_biblioteca(4);  (solo si todavía no la agregó)
-- Después, con cuenta administradora: SELECT * FROM bitacora_biblioteca;
