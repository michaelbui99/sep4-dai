use [sep4_dwh]
go

/*
	Creates ETL schema 
*/
CREATE SCHEMA etl
go

/*
	Creates the table that will keep track of when Source was last load 
*/
CREATE TABLE etl.LogUpdate(
	TableName nvarchar(50) NULL,
	LastLoadDate int NULL
) on [PRIMARY]
go


INSERT INTO etl.LogUpdate(TableName, LastLoadDate) VALUES ('DimClimateDevice', 20220519)
INSERT INTO etl.LogUpdate(TableName, LastLoadDate) VALUES ('DimRoom', 20220519)
INSERT INTO etl.LogUpdate(TableName, LastLoadDate) VALUES ('DimSettings', 20220519)
INSERT INTO etl.LogUpdate(TableName, LastLoadDate) VALUES ('FactMeasurement', 20220519)
go

/*
	Altering tables, such that we can track changes
*/
ALTER TABLE edw.DimClimateDevice
ADD validFrom int, validTo int
ALTER TABLE edw.DimRoom
ADD validFrom int, validTo int
ALTER TABLE edw.DimSettings
ADD validFrom int, validTo int
go

UPDATE edw.DimClimateDevice
SET validFrom = 20220519, validTo = 99990101
UPDATE edw.DimRoom
SET validFrom = 20220519, validTo = 99990101
UPDATE edw.DimSettings
SET validFrom = 20220519, validTo = 99990101
UPDATE edw.DimPrice
SET validFrom = 20220519, validTo = 99990101
go




