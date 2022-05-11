IF NOT EXISTS(SELECT * FROM sys.databases WHERE name = 'sep4_dwh')
CREATE DATABASE sep4_dwh
 GO

USE sep4_dwh
go

GO
IF NOT EXISTS (SELECT * FROM sys.schemas WHERE name = N'stage')
EXEC('CREATE SCHEMA stage')
go


IF NOT EXISTS (SELECT * FROM sys.schemas WHERE name = N'edw')
EXEC('CREATE SCHEMA edw')
go


