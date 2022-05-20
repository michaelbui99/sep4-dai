
use [sep4_dwh_test]
go
/****** Load to stage ClimateDevice  ******/
insert into [edw].[DimClimateDevice] 
([ClimateDeviceId]
)
SELECT [ClimateDeviceId]
  FROM sep4_dwh_test.stage.[DimClimateDevice]


  /******  Load to stage Room  ******/
INSERT INTO [edw].[DimRoom]
      ([RoomId],
	   [RoomName])
SELECT [RoomId]
      ,[RoomName]
  FROM sep4_dwh_test.stage.[DimRoom]
      


  /****** Load to stage Settings  ******/

Insert into [edw].[DimSettings]
      ([SettingsId]
      ,[Co2Threshold]
      ,[HumidityThreshold]
      ,[TargetTemperature]
      ,[TemperatureMargin])
SELECT [SettingsId]
      ,[Co2Threshold]
      ,[HumidityThreshold]
      ,[TargetTemperature]
      ,[TemperatureMargin]
  FROM sep4_dwh_test.stage.[DimSettings]


  /****** Load to stage Measurement  ******/

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
 FROM sep4_dwh_test.stage.FactMeasurement fm join sep4_dwh_test.edw.DimClimateDevice dcd on fm.ClimateDeviceId = dcd.ClimateDeviceId
 join sep4_dwh_test.edw.DimSettings ds on fm.SettingsId = ds.SettingsId
 join sep4_dwh_test.edw.DimRoom dr on fm.RoomId = dr.RoomId
