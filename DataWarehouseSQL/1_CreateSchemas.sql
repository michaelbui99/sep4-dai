use sep4_source 
go

IF NOT EXISTS (SELECT * FROM sys.schemas WHERE name = N'stage')
EXEC('CREATE SCHEMA stage')
go


IF NOT EXISTS (SELECT * FROM sys.schemas WHERE name = N'edw')
EXEC('CREATE SCHEMA edw')
go


