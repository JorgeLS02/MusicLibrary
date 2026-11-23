-- 02_datos.sql
-- Ejecutar después de 01_esquema.sql y solo una vez.
USE biblioteca_musical;

INSERT INTO artistas (id_artista, nombre, pais, genero, fecha_registro) VALUES
(1, 'Aurora Norte', 'México', 'Pop', '2026-11-01'),
(2, 'Ritmo Solar', 'México', 'Electrónica', '2026-11-01'),
(3, 'Luna Roja', 'Argentina', 'Rock', '2026-11-02'),
(4, 'Mar Azul', 'Chile', 'Indie', '2026-11-02'),
(5, 'Viento Sur', 'Colombia', 'Pop', '2026-11-03'),
(6, 'Eco Urbano', 'México', 'Hip hop', '2026-11-03'),
(7, 'Nube Gris', 'España', 'Alternativo', '2026-11-04'),
(8, 'Cielo Abierto', 'Perú', 'Folk', '2026-11-04'),
(9, 'Sol de Medianoche', 'México', 'Rock', '2026-11-05'),
(10, 'Horizonte Cero', 'Uruguay', 'Electrónica', '2026-11-05'),
(11, 'Brisa Clara', 'Costa Rica', 'Pop', '2026-11-06'),
(12, 'Estación Prisma', 'México', 'Indie', '2026-11-06'),
(13, 'Marea Alta', 'España', 'Rock', '2026-11-07'),
(14, 'Jardín de Papel', 'Ecuador', 'Folk', '2026-11-07'),
(15, 'Neón Violeta', 'México', 'Electrónica', '2026-11-08');

-- Duración expresada en segundos.
INSERT INTO canciones (id_cancion, titulo, id_artista, duracion, genero, fecha_lanzamiento) VALUES
(1, 'Primer Amanecer', 1, 215, 'Pop', '2024-02-16'),
(2, 'Luces en la Ciudad', 1, 198, 'Pop', '2025-06-21'),
(3, 'Pulso Digital', 2, 242, 'Electrónica', '2024-03-09'),
(4, 'Órbita', 2, 231, 'Electrónica', '2025-07-18'),
(5, 'Fuego Lento', 3, 260, 'Rock', '2023-10-04'),
(6, 'Kilómetros', 3, 225, 'Rock', '2024-11-11'),
(7, 'Arena y Sal', 4, 203, 'Indie', '2025-01-25'),
(8, 'Vuelve a Empezar', 5, 210, 'Pop', '2024-05-30'),
(9, 'Esquinas', 6, 188, 'Hip hop', '2025-02-14'),
(10, 'Sin Señal', 7, 234, 'Alternativo', '2023-09-08'),
(11, 'Camino de Tierra', 8, 247, 'Folk', '2024-08-22'),
(12, 'Ruta 90', 9, 273, 'Rock', '2025-04-12'),
(13, 'Bajo Cero', 10, 219, 'Electrónica', '2024-12-02'),
(14, 'Papel y Tinta', 11, 206, 'Pop', '2025-03-01'),
(15, 'Reflejos', 12, 228, 'Indie', '2025-05-16');
