-- 01_esquema.sql
-- Proyecto: Biblioteca Musical | Taller de Base de Datos
-- MySQL 8.0.16 o superior

CREATE DATABASE IF NOT EXISTS biblioteca_musical
    CHARACTER SET utf8mb4
    COLLATE utf8mb4_unicode_ci;

USE biblioteca_musical;

CREATE TABLE IF NOT EXISTS artistas (
    id_artista INT AUTO_INCREMENT PRIMARY KEY,
    nombre VARCHAR(100) NOT NULL,
    pais VARCHAR(60) NOT NULL,
    genero VARCHAR(50) NOT NULL DEFAULT 'Sin especificar',
    fecha_registro DATE NOT NULL,
    CONSTRAINT uq_artista_nombre_pais UNIQUE (nombre, pais)
) ENGINE = InnoDB;

CREATE TABLE IF NOT EXISTS canciones (
    id_cancion INT AUTO_INCREMENT PRIMARY KEY,
    titulo VARCHAR(100) NOT NULL,
    id_artista INT NOT NULL,
    duracion INT NOT NULL,
    genero VARCHAR(50) NOT NULL DEFAULT 'Sin especificar',
    fecha_lanzamiento DATE NOT NULL,
    CONSTRAINT uq_cancion_artista_titulo UNIQUE (id_artista, titulo),
    CONSTRAINT chk_duracion CHECK (duracion BETWEEN 1 AND 36000),
    CONSTRAINT fk_cancion_artista FOREIGN KEY (id_artista)
        REFERENCES artistas(id_artista)
        ON UPDATE CASCADE
        ON DELETE RESTRICT
) ENGINE = InnoDB;
