USE TestDatabase
GO


ALTER TABLE Inventory.Furniture DROP CONSTRAINT DF__Furniture__Subcategory
ALTER TABLE Inventory.Furniture DROP COLUMN Subcategory


ALTER TABLE Inventory.Furniture ADD Subcategory VARCHAR(50) NULL DEFAULT('')
ALTER TABLE Inventory.Furniture ADD CONSTRAINT DF__Furniture__Subcategory DEFAULT('') FOR Subcategory

UPDATE Inventory.Furniture SET Subcategory = DEFAULT

ALTER TABLE Inventory.Furniture ALTER COLUMN Subcategory NVARCHAR(150) NOT NULL

SELECT * FROM Inventory.Furniture

GO


CREATE TYPE dbo.String FROM VARCHAR(255)

DECLARE @MyString dbo.String = 'Up to 255 unicode characaters'

SELECT * FROM sys.types