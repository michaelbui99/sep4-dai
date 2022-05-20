use [sep4_dwh]
go


--DimSettings additions

-- Declare LastLoadDate variable and give it the value of a query that returns LastLoadUpdate from the LogUpdate table in etl schema
DECLARE @LastLoadDate int
SET @LastLoadDate = (SELECT MAX([LastLoadDate]) from etl.LogUpdate where TableName = 'DimSettings')
-- Declare NewLoadDate variable whitch it takes todays date
DECLARE @NewLoadDate int
SET @NewLoadDate = CONVERT(char(8), GETDATE(), 112) 
--Declare FutureDate variable which is set to a distant future
DECLARE @FutureDate int
set @FutureDate = 99990101

-- Add validFrom and validTo to the DimCustomer table with the values of NewLoadDate and FutureDate
INSERT INTO edw.DimSettings(
       [SettingsId]
      ,[HumidityThreshold]
      ,[TargetTemperature]
      ,[TemperatureMargin]
      ,[CO2Threshold]
	  ,[validFrom]
	  ,[validTo]
  
) SELECT 
	   [SettingsId]
      ,[HumidityThreshold]
      ,[TargetTemperature]
      ,[TemperatureMargin]
      ,[CO2Threshold]
	  ,@NewLoadDate
	  ,@FutureDate
FROM 
stage.DimSettings
WHERE [SettingsId] in
(select [SettingsId] from stage.DimSettings
except
select SettingsId from edw.DimSettings where validTo=99990101) -- The exept statment here is to avoid adding duplicate



--DimSettings updates

SELECT  
       [SettingsId] --Get all records in stage schema 
      ,[HumidityThreshold]
      ,[TargetTemperature]
      ,[TemperatureMargin]
      ,[CO2Threshold]
INTO #tmp
FROM stage.DimSettings EXCEPT SELECT --Except Settings that already exist in edw schema
[SettingsId] 
      ,[HumidityThreshold]
      ,[TargetTemperature]
      ,[TemperatureMargin]
      ,[CO2Threshold]

FROM edw.DimSettings where validTo=99990101
EXCEPT SELECT --Except new records
[SettingsId] 
      ,[HumidityThreshold]
      ,[TargetTemperature]
      ,[TemperatureMargin]
      ,[CO2Threshold]
FROM stage.DimSettings WHERE SettingsId in (
SELECT SettingsId from stage.DimSettings except select SettingsId from edw.DimSettings where validTo=99990101
) 

--Insert from temp table to DimSettings after the above filtering and checks 
INSERT INTO edw.DimSettings(
       [SettingsId] 
      ,[HumidityThreshold]
      ,[TargetTemperature]
      ,[TemperatureMargin]
      ,[CO2Threshold]
      ,validFrom
      ,validTo
) select 
       [SettingsId] 
      ,[HumidityThreshold]
      ,[TargetTemperature]
      ,[TemperatureMargin]
      ,[CO2Threshold]
      ,@NewLoadDate
      ,@FutureDate
FROM #tmp

--Set valid to after the changes according to type 2 to yesterday
UPDATE edw.DimSettings
SET validTo =  @NewLoadDate-1
WHERE SettingsId in (
SELECT SettingsId from #tmp) and edw.DimSettings.validFrom < @NewLoadDate

DROP table if exists #tmp


--DimSettings Deletions
 UPDATE [edw].[DimSettings]
 SET [ValidTo] = @NewLoadDate-1
 where [SettingsId] in (
 SELECT [SettingsId]
 from [edw].[DimSettings] 
 WHERE [SettingsId] in (SELECT [SettingsId] FROM [edw].[DimSettings] except select [SettingsId]
 FROM [stage].[DimSettings])) and [ValidTo] =99990101




INSERT INTO [etl].[LogUpdate]
(
[TableName],[LastLoadDate]
)
	VALUES ('DimSettings',@NewLoadDate)
