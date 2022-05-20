use sep4_dwh
go

DECLARE @UpdatedTable varchar
SET @UpdatedTable = 'FactMeasurement'

DECLARE @LastLoadDate int
SET @LastLoadDate = (SELECT MAX([LastLoadDate]) from etl.LogUpdate where TableName = @UpdatedTable)
-- Declare NewLoadDate variable whitch it takes todays date
DECLARE @NewLoadDate int
SET @NewLoadDate = CONVERT(char(8), GETDATE(), 112) 

truncate table [edw].[FactMeasurement]
insert into [edw].[FactMeasurement]
      ([R_ID]
	  ,[S_ID]
	  ,[C_ID]
	  ,[CO2_In_PPM]
	  ,[Temperature_In_C]
	  ,[Humidity_In_Percent]
	  ,MT_ID
	  ,MD_ID
      )
SELECT dr.[R_ID]
	  ,ds.[S_ID]
	  ,dcd.[C_ID]
	  ,[CO2_In_PPM]
	  ,[Temperature_In_C]
	  ,[Humidity_In_Percent]
	  ,(SELECT TimeKey from edw.DimTime dt WHERE dt.TimeAltKey like convert(char(8), [Measurement_Time], 108))
	  ,(SELECT D_ID from edw.DimDate dd WHERE dd.D_ID like convert(int, convert(varchar, [Measurement_Date], 112), 112)) 
 FROM sep4_dwh.stage.FactMeasurement fm join sep4_dwh.edw.DimClimateDevice dcd on fm.ClimateDeviceId = dcd.ClimateDeviceId
 join sep4_dwh.edw.DimSettings ds on fm.SettingsId = ds.SettingsId
 join sep4_dwh.edw.DimRoom dr on fm.RoomId = dr.RoomId
WHERE dcd.validTo='99990101'
AND ds.validTo='99990101'
AND dr.validTo='99990101'



INSERT INTO etl.LogUpdate(TableName, LastLoadDate) VALUES (@UpdatedTable, @NewLoadDate)
go

