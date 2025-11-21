USE master
 
SELECT * FROM sys.database_files
 
RESTORE FILELISTONLY FROM  DISK = N'AdventureWorks2019.bak' 
 
RESTORE DATABASE AdventureWorks2019 FROM  DISK = N'AdventureWorks2019.bak' 
WITH MOVE N'AdventureWorks2019' TO N'/var/opt/mssql/data/AdventureWorks2019.mdf',  
     MOVE N'AdventureWorks2019_log' TO N'/var/opt/mssql/data/AdventureWorks2019.ldf'
