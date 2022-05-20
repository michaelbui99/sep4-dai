use [sep4_dwh]
go

IF EXISTS (SELECT * from sys.tables where name like '#tmp%') DROP TABLE #tmp


--DimSettings additions -----------------------------------------------
DROP TABLE IF EXISTS #tmp
-- Declare LastLoadDate variable and give it the value of a query that returns LastLoadUpdate from the LogUpdate table in etl schema
DECLARE @LastLoadDate int
SET @LastLoadDate = (SELECT MAX([LastLoadDate])
                     from etl.LogUpdate
                     where TableName = 'DimSettings')
-- Declare NewLoadDate variable whitch it takes todays date
DECLARE @NewLoadDate int
SET @NewLoadDate = CONVERT(char(8), GETDATE(), 112)
--Declare FutureDate variable which is set to a distant future
DECLARE @FutureDate int
set @FutureDate = 99990101

-- Add validFrom and validTo to the DimSettings table with the values of NewLoadDate and FutureDate
INSERT INTO edw.DimSettings( [SettingsId]
                           , [HumidityThreshold]
                           , [TargetTemperature]
                           , [TemperatureMargin]
                           , [CO2Threshold]
                           , [validFrom]
                           , [validTo])
SELECT [SettingsId]
     , [HumidityThreshold]
     , [TargetTemperature]
     , [TemperatureMargin]
     , [CO2Threshold]
     , @NewLoadDate
     , @FutureDate
FROM stage.DimSettings
WHERE [SettingsId] in
      (select [SettingsId]
       from stage.DimSettings
           except
       select SettingsId
       from edw.DimSettings
       where validTo = 99990101)
-- The exept statment here is to avoid adding duplicate


--DimSettings updates

SELECT [SettingsId] --Get all records in stage schema
     , [HumidityThreshold]
     , [TargetTemperature]
     , [TemperatureMargin]
     , [CO2Threshold]
INTO #tmp
FROM stage.DimSettings EXCEPT
SELECT --Except Settings that already exist in edw schema
    [SettingsId]
     , [HumidityThreshold]
     , [TargetTemperature]
     , [TemperatureMargin]
     , [CO2Threshold]

FROM edw.DimSettings
where validTo = 99990101
    EXCEPT
SELECT --Except new records
    [SettingsId]
     , [HumidityThreshold]
     , [TargetTemperature]
     , [TemperatureMargin]
     , [CO2Threshold]
FROM stage.DimSettings
WHERE SettingsId in (
    SELECT SettingsId from stage.DimSettings except select SettingsId from edw.DimSettings where validTo = 99990101
)

--Insert from temp table to DimSettings after the above filtering and checks 
INSERT INTO edw.DimSettings( [SettingsId]
                           , [HumidityThreshold]
                           , [TargetTemperature]
                           , [TemperatureMargin]
                           , [CO2Threshold]
                           , validFrom
                           , validTo)
select [SettingsId]
     , [HumidityThreshold]
     , [TargetTemperature]
     , [TemperatureMargin]
     , [CO2Threshold]
     , @NewLoadDate
     , @FutureDate
FROM #tmp

--Set valid to after the changes according to type 2 to yesterday
UPDATE edw.DimSettings
SET validTo = @NewLoadDate - 1
WHERE SettingsId in (
    SELECT SettingsId
    from #tmp)
  and edw.DimSettings.validFrom < @NewLoadDate

DROP table #tmp


--DimSettings Deletions
UPDATE [edw].[DimSettings]
SET [ValidTo] = @NewLoadDate - 1
where [SettingsId] in (
    SELECT [SettingsId]
    from [edw].[DimSettings]
    WHERE [SettingsId] in (SELECT [SettingsId]
                           FROM [edw].[DimSettings] except
                           select [SettingsId]
                           FROM [stage].[DimSettings]))
  and [ValidTo] = 99990101


INSERT INTO [etl].[LogUpdate]
([TableName], [LastLoadDate])
VALUES ('DimSettings', @NewLoadDate)




--DimRoom additions-----------------------------------------------------------
SET @LastLoadDate = (SELECT MAX([LastLoadDate])
                     from etl.LogUpdate
                     where TableName = 'DimRoom')
-- Add validFrom and validTo to the DimRoom table with the values of NewLoadDate and FutureDate
INSERT INTO edw.DimRoom( [RoomId]
                       , [RoomName]
                       , [validFrom]
                       , [validTo])
SELECT [RoomId]
     , [RoomName]
     , @NewLoadDate
     , @FutureDate
FROM stage.DimRoom
WHERE [RoomId] in
      (select [RoomId]
       from stage.DimRoom
           except
       select RoomId
       from edw.DimRoom
       where validTo = 99990101)
-- The exept statment here is to avoid adding duplicate


--DimRoom updates

IF EXISTS (SELECT * from sys.tables where name like '#tmp%') DROP TABLE #tmp
SELECT [RoomId] --Get all records in stage schema
     , [RoomName]
INTO #tmp
FROM stage.DimRoom EXCEPT
SELECT --Except Room that already exist in edw schema
    [RoomId]
     , [RoomName]

FROM edw.DimRoom
where validTo = 99990101
    EXCEPT
SELECT --Except new records
    [RoomId]
     , [RoomName]
FROM stage.DimRoom
WHERE RoomId in (
    SELECT RoomId from stage.DimRoom except select RoomId from edw.DimRoom where validTo = 99990101
)

--Insert from temp table to DimRoom after the above filtering and checks
INSERT INTO edw.DimRoom( [RoomId]
                       , [RoomName]
                       , validFrom
                       , validTo)
select [RoomId]
     , [RoomName]
     , @NewLoadDate
     , @FutureDate
FROM #tmp

--Set valid to after the changes according to type 2 to yesterday
UPDATE edw.DimRoom
SET validTo = @NewLoadDate - 1
WHERE RoomId in (
    SELECT RoomId
    from #tmp)
  and edw.DimRoom.validFrom < @NewLoadDate

DROP table  #tmp


--DimRoom Deletions
UPDATE [edw].[DimRoom]
SET [ValidTo] = @NewLoadDate - 1
where [RoomId] in (
    SELECT [RoomId]
    from [edw].[DimRoom]
    WHERE [RoomId] in (SELECT [RoomId]
                       FROM [edw].[DimRoom] except
                       select [RoomId]
                       FROM [stage].[DimRoom]))
  and [ValidTo] = 99990101


INSERT INTO [etl].[LogUpdate]
([TableName], [LastLoadDate])
VALUES ('DimRoom', @NewLoadDate)

                     

--DimClimateDevice additions ---------------------------------------------------------
SET @LastLoadDate = (SELECT MAX([LastLoadDate])
                     from etl.LogUpdate
                     where TableName = 'DimClimateDevice')
-- Add validFrom and validTo to the DimClimateDevice table with the values of NewLoadDate and FutureDate
INSERT INTO edw.DimClimateDevice(
       [ClimateDeviceId]
	  ,[validFrom]
	  ,[validTo]

) SELECT
	   [ClimateDeviceId]
	  ,@NewLoadDate
	  ,@FutureDate
FROM
stage.DimClimateDevice
WHERE [ClimateDeviceId] in
(select [ClimateDeviceId] from stage.DimClimateDevice
except
select ClimateDeviceId from edw.DimClimateDevice where validTo=99990101) -- The exept statment here is to avoid adding duplicate



--DimClimateDevice updates

IF EXISTS (SELECT * from sys.tables where name like '#tmp%') DROP TABLE #tmp
SELECT
       [ClimateDeviceId] --Get all records in stage schema

INTO #tmp
FROM stage.DimClimateDevice EXCEPT SELECT --Except Settings that already exist in edw schema
[ClimateDeviceId]

FROM edw.DimClimateDevice where validTo=99990101
EXCEPT SELECT --Except new records
[ClimateDeviceId]

FROM stage.DimClimateDevice WHERE ClimateDeviceId in (
SELECT ClimateDeviceId from stage.DimClimateDevice except select ClimateDeviceId from edw.DimClimateDevice where validTo=99990101
)

--Insert from temp table to DimClimateDevice after the above filtering and checks
INSERT INTO edw.DimClimateDevice(
       [ClimateDeviceId]
      ,validFrom
      ,validTo
) select
       [ClimateDeviceId]
      ,@NewLoadDate
      ,@FutureDate
FROM #tmp

--Set valid to after the changes according to type 2 to yesterday
UPDATE edw.DimClimateDevice
SET validTo =  @NewLoadDate-1
WHERE ClimateDeviceId in (
SELECT ClimateDeviceId from #tmp) and edw.DimClimateDevice.validFrom < @NewLoadDate

DROP table  #tmp


--DimClimateDevice Deletions
 UPDATE [edw].[DimClimateDevice]
 SET [ValidTo] = @NewLoadDate-1
 where [ClimateDeviceId] in (
 SELECT [ClimateDeviceId]
 from [edw].[DimClimateDevice]
 WHERE [ClimateDeviceId] in (SELECT [ClimateDeviceId] FROM [edw].[DimClimateDevice] except select [ClimateDeviceId]
 FROM [stage].[DimClimateDevice])) and [ValidTo] =99990101



INSERT INTO [etl].[LogUpdate]
(
[TableName],[LastLoadDate]
)
	VALUES ('DimClimateDevice',@NewLoadDate)



-- FactMeasurement load with newest version of dimensions -------------------------------------------------
DECLARE @UpdatedTable varchar
SET @UpdatedTable = 'FactMeasurement'
SET @LastLoadDate = (SELECT MAX([LastLoadDate])
                     from etl.LogUpdate
                     where TableName = @UpdatedTable)

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

