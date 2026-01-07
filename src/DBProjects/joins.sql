USE AdventureWorks2019
GO

select * from HumanResources.EmployeeDepartmentHistory

select * from Person.Person WHERE PersonType = 'EM'

-- inner join

select [EH].[BusinessEntityID],
[EH].[DepartmentID],
[EH].[ShiftID],
[EH].[StartDate],
[EH].[EndDate],
[EH].[ModifiedDate],
[PS].[BusinessEntityID],
[PS].[PersonType],
[PS].[NameStyle],
[PS].[Title],
[PS].[FirstName],
[PS].[MiddleName],
[PS].[LastName],
[PS].[Suffix],
[PS].[EmailPromotion],
[PS].[AdditionalContactInfo],
[PS].[Demographics],
[PS].[rowguid],
[PS].[ModifiedDate] from HumanResources.EmployeeDepartmentHistory as EH
    INNER JOIN Person.Person as PS
        on eh.BusinessEntityID = ps.BusinessEntityID


-- left join

SELECT [EmployeeDepartmentHistory].[BusinessEntityID],
    [EmployeeDepartmentHistory].[DepartmentID],
    [EmployeeDepartmentHistory].[ShiftID],
    [EmployeeDepartmentHistory].[StartDate],
    [EmployeeDepartmentHistory].[EndDate],
    [Person].[FirstName],
    [Person].[MiddleName],
    [Person].[LastName]
FROM HumanResources.EmployeeDepartmentHistory AS EmployeeDepartmentHistory
    LEFT JOIN Person.Person AS Person
        ON Person.BusinessEntityID = EmployeeDepartmentHistory.BusinessEntityID
-- FROM Person.Person AS Person
--     LEFT JOIN HumanResources.EmployeeDepartmentHistory AS EmployeeDepartmentHistory
--         ON Person.BusinessEntityID = EmployeeDepartmentHistory.BusinessEntityID