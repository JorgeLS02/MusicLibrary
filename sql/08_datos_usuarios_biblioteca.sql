-- 08_datos_usuarios_biblioteca.sql
-- Ejecutar UNA SOLA VEZ después de cargar 02_datos.sql y crear las 2 tablas nuevas.
-- Los perfiles son ficticios; solo musica_u01 tendrá una cuenta MySQL de demostración.
USE biblioteca_musical;

INSERT INTO usuarios (id_usuario, usuario_mysql, nombre, correo, fecha_registro) VALUES
(1, 'musica_u01', 'Ana López', 'ana01@ejemplo.test', '2026-11-10 09:00:00'),
(2, 'musica_u02', 'Luis Pérez', 'luis02@ejemplo.test', '2026-11-10 09:10:00'),
(3, 'musica_u03', 'Sofía Ruiz', 'sofia03@ejemplo.test', '2026-11-10 09:20:00'),
(4, 'musica_u04', 'Pablo Torres', 'pablo04@ejemplo.test', '2026-11-10 09:30:00'),
(5, 'musica_u05', 'María Soto', 'maria05@ejemplo.test', '2026-11-10 09:40:00'),
(6, 'musica_u06', 'Diego Flores', 'diego06@ejemplo.test', '2026-11-10 09:50:00'),
(7, 'musica_u07', 'Elena Rojas', 'elena07@ejemplo.test', '2026-11-10 10:00:00'),
(8, 'musica_u08', 'Carlos Vega', 'carlos08@ejemplo.test', '2026-11-10 10:10:00'),
(9, 'musica_u09', 'Laura Méndez', 'laura09@ejemplo.test', '2026-11-10 10:20:00'),
(10, 'musica_u10', 'Raúl Molina', 'raul10@ejemplo.test', '2026-11-10 10:30:00'),
(11, 'musica_u11', 'Julia Navarro', 'julia11@ejemplo.test', '2026-11-10 10:40:00'),
(12, 'musica_u12', 'Pedro Salas', 'pedro12@ejemplo.test', '2026-11-10 10:50:00'),
(13, 'musica_u13', 'Eva Campos', 'eva13@ejemplo.test', '2026-11-10 11:00:00'),
(14, 'musica_u14', 'Iván Silva', 'ivan14@ejemplo.test', '2026-11-10 11:10:00'),
(15, 'musica_u15', 'Rosa Luna', 'rosa15@ejemplo.test', '2026-11-10 11:20:00');

-- 15 registros de biblioteca, con favoritos y calificaciones de ejemplo.
INSERT INTO biblioteca_canciones
(id_usuario, id_cancion, fecha_agregado, favorito, calificacion) VALUES
(1, 1, '2026-11-11 09:00:00', TRUE, 5),
(1, 2, '2026-11-11 09:10:00', FALSE, 4),
(1, 3, '2026-11-11 09:20:00', TRUE, 5),
(2, 4, '2026-11-11 09:30:00', FALSE, 3),
(2, 5, '2026-11-11 09:40:00', TRUE, 5),
(3, 6, '2026-11-11 09:50:00', TRUE, 4),
(3, 7, '2026-11-11 10:00:00', FALSE, 4),
(4, 8, '2026-11-11 10:10:00', TRUE, 5),
(4, 9, '2026-11-11 10:20:00', FALSE, 3),
(5, 10, '2026-11-11 10:30:00', FALSE, NULL),
(6, 11, '2026-11-11 10:40:00', TRUE, 5),
(7, 12, '2026-11-11 10:50:00', FALSE, 4),
(8, 13, '2026-11-11 11:00:00', TRUE, 5),
(9, 14, '2026-11-11 11:10:00', FALSE, 3),
(10, 15, '2026-11-11 11:20:00', TRUE, 4);
