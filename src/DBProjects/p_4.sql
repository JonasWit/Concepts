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