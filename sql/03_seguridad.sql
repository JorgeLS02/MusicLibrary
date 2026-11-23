-- 03_seguridad.sql
-- Ejecutar con una cuenta que pueda crear usuarios y conceder privilegios.
-- IMPORTANTE: sustituir las claves marcadas antes de ejecutar; no guardarlas en Git.
-- Ejecutar después de 04_procedural.sql y 10_funcion_trigger.sql.
-- Si la BD ya existe, NO necesitas volver a crear cuentas ni cambiar contraseñas.
-- CREATE USER IF NOT EXISTS NO modifica contraseñas existentes.
USE biblioteca_musical;

CREATE ROLE IF NOT EXISTS 'rol_admin', 'rol_usuario';

-- El administrador puede administrar el catálogo, perfiles y bibliotecas.
GRANT SELECT, INSERT, UPDATE, DELETE
ON biblioteca_musical.* TO 'rol_admin';

-- El usuario común solo consulta el catálogo general.
GRANT SELECT ON biblioteca_musical.artistas TO 'rol_usuario';
GRANT SELECT ON biblioteca_musical.canciones TO 'rol_usuario';

-- Sus datos y biblioteca se ofrecen únicamente por vistas filtradas y procedures.
GRANT SELECT ON biblioteca_musical.vista_mi_biblioteca TO 'rol_usuario';
GRANT SELECT ON biblioteca_musical.vista_mi_perfil TO 'rol_usuario';
GRANT EXECUTE ON PROCEDURE biblioteca_musical.sp_agregar_a_biblioteca TO 'rol_usuario';
GRANT EXECUTE ON PROCEDURE biblioteca_musical.sp_quitar_de_biblioteca TO 'rol_usuario';
GRANT EXECUTE ON PROCEDURE biblioteca_musical.sp_marcar_favorita TO 'rol_usuario';
GRANT EXECUTE ON PROCEDURE biblioteca_musical.sp_calificar_cancion TO 'rol_usuario';


-- Permisos para los objetos añadidos después de la primera versión.
-- Requieren haber creado el procedimiento OUT y la función antes.
GRANT EXECUTE ON PROCEDURE biblioteca_musical.sp_contar_canciones_artista
TO 'rol_admin';
GRANT EXECUTE ON FUNCTION biblioteca_musical.fn_duracion_mmss
TO 'rol_admin';
GRANT EXECUTE ON FUNCTION biblioteca_musical.fn_duracion_mmss
TO 'rol_usuario';

-- Cuentas de DEMOSTRACIÓN para C#. Cambia las contraseñas antes de ejecutar.
CREATE USER IF NOT EXISTS 'musica_admin'@'localhost'
IDENTIFIED BY 'REEMPLAZAR_POR_CLAVE_SEGURA_ADMIN';
CREATE USER IF NOT EXISTS 'musica_u01'@'localhost'
IDENTIFIED BY 'REEMPLAZAR_POR_CLAVE_SEGURA_USUARIO';

GRANT 'rol_admin' TO 'musica_admin'@'localhost';
GRANT 'rol_usuario' TO 'musica_u01'@'localhost';
SET DEFAULT ROLE 'rol_admin' TO 'musica_admin'@'localhost';
SET DEFAULT ROLE 'rol_usuario' TO 'musica_u01'@'localhost';

-- Evidencia de permisos. Conecta con cada cuenta para comprobarlos.
SHOW GRANTS FOR 'musica_admin'@'localhost';
SHOW GRANTS FOR 'musica_u01'@'localhost';
