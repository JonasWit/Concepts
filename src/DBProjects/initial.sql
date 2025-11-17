-- use master

-- -- Kill all connections to the database
-- ALTER DATABASE TestDatabase2 SET SINGLE_USER WITH ROLLBACK IMMEDIATE

-- drop database if exists TestDatabase

-- CREATE DATABASE TestDatabase

-- go

use TestDatabase
GO

DROP SCHEMA if EXISTS Inventory
GO

CREATE SCHEMA Inventory



