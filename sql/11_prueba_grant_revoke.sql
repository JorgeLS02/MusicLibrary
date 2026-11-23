
-- 11_prueba_grant_revoke.sql
-- Proyecto: MusicLibrary
-- Demostracion de GRANT y REVOKE.
-- Ejecutar con root sobre una instalacion de prueba.
-- No modifica datos de las tablas.

USE biblioteca_musical;

-- 1. Crear un rol temporal.
CREATE ROLE IF NOT EXISTS
'rol_demo_revoke_musiclibrary';

-- 2. GRANT: conceder permiso SELECT.
GRANT SELECT
ON biblioteca_musical.artistas
TO 'rol_demo_revoke_musiclibrary';

-- 3. Comprobar el permiso concedido.
SHOW GRANTS FOR 'rol_demo_revoke_musiclibrary';

-- 4. REVOKE: retirar el permiso SELECT.
REVOKE SELECT
ON biblioteca_musical.artistas
FROM 'rol_demo_revoke_musiclibrary';

-- 5. Comprobar que el permiso desaparecio.
SHOW GRANTS FOR 'rol_demo_revoke_musiclibrary';

-- 6. Eliminar el rol temporal.
DROP ROLE IF EXISTS
'rol_demo_revoke_musiclibrary';


