--DATA cleansing
USE sep4_dwh
GO

-- Cleanse device id
UPDATE stage.DimClimateDevice
SET ClimateDeviceId = 'UNKNOWN DEVICE' WHERE ClimateDeviceId IS NULL


-- Cleanse room name
UPDATE stage.DimRoom
SET RoomName = 'UNKNOWN' WHERE RoomName IS NULL








