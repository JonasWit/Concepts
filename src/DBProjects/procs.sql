
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [HumanResources].[spDeparement_Get]
  @GroupName [dbo].[Name] = NULL
  , @DepName [dbo].[Name] = NULL
  , @PageNumber INT = 1
  , @RowsToReturn INT = 7

/*
  EXEC HumanResources.spDeparement_Get 
    @GroupName = 'R n D Group'
    , @DepName = 'R n D'

  EXEC HumanResources.spDeparement_Get
    @PageNumber = 1
    , @RowsToReturn = 4


  EXEC HumanResources.spDeparement_Get
*/

AS
  BEGIN
    DECLARE @Offset INT = @RowsToReturn * (@PageNumber - 1)

    SELECT [DepartmentID],
    [Name],
    [GroupName],
    [ModifiedDate] FROM HumanResources.Department
      WHERE Department.GroupName = ISNULL(@GroupName, Department.GroupName) 
      AND Department.[Name] = ISNULL(@DepName, Department.[Name]) 
    ORDER BY Department.GroupName, Department.[Name]
    OFFSET  @Offset ROWS FETCH NEXT @RowsToReturn ROWS ONLY
    
  END
GO



USE AdventureWorks2019
GO

SELECT 
[Rate],
[ModifiedDate], ROW_NUMBER() OVER (PARTITION BY EmployeePayHistory.BusinessEntityID 
                          ORDER BY ModifiedDate DESC) AS IsMostRecent
 FROM HumanResources.EmployeePayHistory
WHERE BusinessEntityID = 4

SELECT MostRecentPayRate.Rate, * FROM Person.Person
  CROSS Apply(
    SELECT EmployeePayHistory.Rate, ROW_NUMBER() OVER (PARTITION BY EmployeePayHistory.BusinessEntityID 
                              ORDER BY ModifiedDate DESC) AS IsMostRecent, EmployeePayHistory.BusinessEntityID
    FROM HumanResources.EmployeePayHistory
    WHERE EmployeePayHistory.BusinessEntityID = Person.BusinessEntityID 
  ) MostRecentPayRate
   WHERE MostRecentPayRate.IsMostRecent = 1

SELECT [MostRecentPayRate].[Rate],
[MostRecentPayRate].[IsMostRecent],
[MostRecentPayRate].[BusinessEntityID],
[Person].[BusinessEntityID],
[Person].[PersonType],
[Person].[NameStyle],
[Person].[Title],
[Person].[FirstName],
[Person].[MiddleName],
[Person].[LastName],
[Person].[Suffix],
[Person].[EmailPromotion],
[Person].[AdditionalContactInfo],
[Person].[Demographics],
[Person].[rowguid],
[Person].[ModifiedDate] FROM (
  SELECT EmployeePayHistory.Rate, ROW_NUMBER() OVER (PARTITION BY EmployeePayHistory.BusinessEntityID 
      ORDER BY ModifiedDate DESC) AS IsMostRecent, EmployeePayHistory.BusinessEntityID
      FROM HumanResources.EmployeePayHistory
    ) MostRecentPayRate
      LEFT JOIN Person.Person
        ON MostRecentPayRate.BusinessEntityID = Person.BusinessEntityID
      WHERE IsMostRecent = 1




GO



ALTER PROCEDURE HumanResources.spEmpPayRate_Get
AS
BEGIN

  WITH MostRecentPayRate AS (
    SELECT 
      EmployeePayHistory.Rate, 
      ROW_NUMBER() OVER (PARTITION BY EmployeePayHistory.BusinessEntityID 
        ORDER BY ModifiedDate DESC) AS IsMostRecent, 
      EmployeePayHistory.BusinessEntityID
    FROM HumanResources.EmployeePayHistory
  )

  SELECT [MostRecentPayRate].[Rate],
  [MostRecentPayRate].[IsMostRecent],
  [MostRecentPayRate].[BusinessEntityID],
  [Person].[BusinessEntityID],
  [Person].[PersonType],
  [Person].[NameStyle],
  [Person].[Title],
  [Person].[FirstName],
  [Person].[MiddleName],
  [Person].[LastName],
  [Person].[Suffix],
  [Person].[EmailPromotion],
  [Person].[AdditionalContactInfo],
  [Person].[Demographics],
  [Person].[rowguid],
  [Person].[ModifiedDate] FROM MostRecentPayRate
        LEFT JOIN Person.Person
          ON MostRecentPayRate.BusinessEntityID = Person.BusinessEntityID
        WHERE IsMostRecent = 1
END

EXEC HumanResources.spEmpPayRate_Get
