USE TestDatabase
GO

DROP TABLE IF EXISTS  Inventory.Furniture

CREATE TABLE Inventory.Furniture (
    FurnitureId INT IDENTITY(1,1),
    FurnitureType VARCHAR(50) NULL,
    FurnitureName VARCHAR(50) NOT NULL,
    Price DECIMAL(18,4),
    Quantity INT DEFAULT(0) NOT NULL,
    Release DATE,
    CreateDate DATETIME,
    UpdateDate DATETIME
)

GO

INSERT INTO Inventory.Furniture (
    [FurnitureType],
    [FurnitureName],
    [Price],
    [Quantity],
    [Release],
    [CreateDate],
    [UpdateDate]
) VALUES (
    'Couch',
    'The Super One',
    1299.99,
    34,
    '2033-12-11',
    '2033-12-11 20:00:00',
    '2033-12-11 20:00:00'  
)

SELECT [Furniture1].[FurnitureId],
[Furniture1].[FurnitureType],
[Furniture1].[FurnitureName],
[Furniture1].[Price],
[Furniture1].[Quantity],
[Furniture1].[Release],
[Furniture1].[CreateDate],
[Furniture1].[UpdateDate] 
  FROM [TestDatabase].[Inventory].[Furniture] AS Furniture1

GO

UPDATE [Inventory].[Furniture] SET Quantity = 43
SELECT * from Inventory.Furniture

GO

