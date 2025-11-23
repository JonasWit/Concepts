USE master
 
SELECT * FROM sys.database_files
 
RESTORE FILELISTONLY FROM  DISK = N'AdventureWorks2019.bak' -- just lists the names of the files
 
USE master;
GO

ALTER DATABASE AdventureWorks2019
SET SINGLE_USER
WITH ROLLBACK IMMEDIATE;
GO

RESTORE DATABASE AdventureWorks2019 FROM  DISK = N'AdventureWorks2019.bak' 
WITH MOVE N'AdventureWorks2019' TO N'/var/opt/mssql/data/AdventureWorks2019.mdf',  
     MOVE N'AdventureWorks2019_log' TO N'/var/opt/mssql/data/AdventureWorks2019.ldf'

ALTER DATABASE AdventureWorks2019
SET MULTI_USER;
GO