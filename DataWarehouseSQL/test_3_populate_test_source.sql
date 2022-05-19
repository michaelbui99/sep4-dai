use sep4_source_test
go

-- Insert subset of measurements from source to test source
SET IDENTITY_INSERT sep4_source_test.dbo.Measurements ON
INSERT INTO dbo.Measurements
([MeasurementId]
      ,[Timestamp]
      ,[Temperature]
      ,[Humidity]
      ,[Co2]
      ,[ClimateDeviceId])
SELECT [MeasurementId]
      ,[Timestamp]
      ,[Temperature]
      ,[Humidity]
      ,[Co2]
      ,[ClimateDeviceId]
FROM sep4_source.dbo.Measurements 
WHERE MeasurementId % 10 = 2
SET IDENTITY_INSERT sep4_source_test.dbo.Measurements OFF


INSERT INTO dbo.Devices(
[ClimateDeviceId]
      ,[SettingsSettingId]
      ,[RoomId])
SELECT [ClimateDeviceId]
      ,[SettingsSettingId]
      ,[RoomId]
FROM sep4_source.dbo.Devices 
WHERE ClimateDeviceId IN (
	SELECT DISTINCT ClimateDeviceId FROM dbo.Measurements
)

SET IDENTITY_INSERT sep4_source_test.dbo.Rooms ON
INSERT INTO dbo.Rooms(
[RoomId]
      ,[RoomName]
      ,[SettingsSettingId])
SELECT [RoomId]
      ,[RoomName]
      ,[SettingsSettingId]
FROM sep4_source.dbo.Rooms
WHERE RoomId IN (
	SELECT DISTINCT RoomId FROM dbo.Devices
)
SET IDENTITY_INSERT sep4_source_test.dbo.Rooms OFF


SET IDENTITY_INSERT sep4_source_test.dbo.Settings ON
INSERT INTO dbo.Settings(
[SettingId]
      ,[Co2Threshold]
      ,[HumidityThreshold]
      ,[TargetTemperature]
      ,[TemperatureMargin])
SELECT [SettingId]
      ,[Co2Threshold]
      ,[HumidityThreshold]
      ,[TargetTemperature]
      ,[TemperatureMargin]
FROM sep4_source.dbo.Settings
WHERE SettingId IN (
	SELECT DISTINCT SettingsSettingId from dbo.Devices
)
SET IDENTITY_INSERT sep4_source_test.dbo.Settings OFF
