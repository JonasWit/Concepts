USE AdventureWorks2019
GO

SELECT  [DepartmentID],
[Name],
[GroupName],
[ModifiedDate] 
FROM HumanResources.Department

GO

SELECT * FROM HumanResources.Department

DELETE from HumanResources.Department WHERE DepartmentID = 7

SELECT * FROM HumanResources.EmployeeDepartmentHistory 

DELETE FROM HumanResources.EmployeeDepartmentHistory WHERE DepartmentID = 7

UPDATE HumanResources.Department SET [GroupName] = 'R n D Group'
    WHERE GroupName = 'Research and Development'


USE AdventureWorks2019
GO

SELECT * INTO HumanResources.DepartmentCopy2 FROM HumanResources.Department

SELECT * FROM HumanResources.Department ORDER BY [Name] 

INSERT INTO  HumanResources.DepartmentCopy2(  [Name],
[GroupName],
[ModifiedDate] ) VALUES ('rgroup', 'rname', '2025-09-09')

TRUNCATE TABLE HumanResources.DepartmentCopy2
DELETE from HumanResources.DepartmentCopy2 WHERE 1=1


SELECT [Department].[DepartmentID],
[Department].[Name],
[Department].[GroupName],
[Department].[ModifiedDate] 
from HumanResources.Department AS Department

SELECT GroupName,  COUNT(*) 
    from HumanResources.Department 
    GROUP BY GroupName
    ORDER BY COUNT(*)