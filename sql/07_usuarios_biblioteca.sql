-- 07_usuarios_biblioteca.sql
-- Ampliación SIN borrar las tablas artistas y canciones ya existentes.
-- Ejecutar después de 01_esquema.sql.
USE biblioteca_musical;

CREATE TABLE IF NOT EXISTS usuarios (
    id_usuario INT AUTO_INCREMENT PRIMARY KEY,
    usuario_mysql VARCHAR(80) NOT NULL,
    nombre VARCHAR(100) NOT NULL,
    correo VARCHAR(150) NOT NULL,
    fecha_registro DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    activo BOOLEAN NOT NULL DEFAULT TRUE,
    CONSTRAINT uq_usuario_mysql UNIQUE (usuario_mysql),
    CONSTRAINT uq_usuario_correo UNIQUE (correo)
) ENGINE = InnoDB;

CREATE TABLE IF NOT EXISTS biblioteca_canciones (
    id_biblioteca INT AUTO_INCREMENT PRIMARY KEY,
    id_usuario INT NOT NULL,
    id_cancion INT NOT NULL,
    fecha_agregado DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    favorito BOOLEAN NOT NULL DEFAULT FALSE,
    calificacion TINYINT NULL,
    CONSTRAINT uq_biblioteca_usuario_cancion UNIQUE (id_usuario, id_cancion),
    CONSTRAINT chk_biblioteca_calificacion
        CHECK (calificacion IS NULL OR calificacion BETWEEN 1 AND 5),
    CONSTRAINT fk_biblioteca_usuario
        FOREIGN KEY (id_usuario) REFERENCES usuarios(id_usuario)
        ON UPDATE CASCADE ON DELETE CASCADE,
    CONSTRAINT fk_biblioteca_cancion
        FOREIGN KEY (id_cancion) REFERENCES canciones(id_cancion)
        ON UPDATE CASCADE ON DELETE CASCADE
) ENGINE = InnoDB;
