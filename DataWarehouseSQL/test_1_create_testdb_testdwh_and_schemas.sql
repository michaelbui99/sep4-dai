-- Create Test Source 
use master 
go
IF NOT EXISTS(SELECT * FROM sys.databases WHERE name = 'sep4_source_test')
CREATE DATABASE sep4_source_test
 GO


-- Create Test Data Warehouse
IF NOT EXISTS(SELECT * FROM sys.databases WHERE name = 'sep4_dwh_test')
CREATE DATABASE sep4_dwh_test
 GO

USE sep4_dwh_test
go

GO
IF NOT EXISTS (SELECT * FROM sys.schemas WHERE name = N'stage')
EXEC('CREATE SCHEMA stage')
go


IF NOT EXISTS (SELECT * FROM sys.schemas WHERE name = N'edw')
EXEC('CREATE SCHEMA edw')
go