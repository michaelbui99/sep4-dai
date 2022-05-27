--- TEST CASES --------
------ SOURCE VS STAGE TESTS----------
----------- Count of measurements in souce should be the same as the count of measurements in stage ---------
SELECT COUNT(*) from sep4_source_test.dbo.Measurements
SELECT COUNT(*) from sep4_dwh_test.stage.FactMeasurement

----------- Measurements in source are the same as the measurements in stage ---------
SELECT SUM(Temperature) from sep4_source_test.dbo.Measurements
SELECT SUM(Temperature_In_C) from sep4_dwh_test.stage.FactMeasurement

SELECT SUM(Humidity) from sep4_source_test.dbo.Measurements
SELECT SUM(Humidity_In_Percent) from sep4_dwh_test.stage.FactMeasurement

SELECT SUM(CO2) from sep4_source_test.dbo.Measurements
SELECT SUM(CO2_In_PPM) from sep4_dwh_test.stage.FactMeasurement

----------- Count of ClimateDevices in source should be the same as the count of Climate Devices in stage ------
SELECT COUNT(*) from sep4_source_test.dbo.Devices
SELECT COUNT(*) from sep4_dwh_test.stage.DimClimateDevice

----------- Count Settings in source should be the same as the count of settings in stage -----
SELECT COUNT(*) from sep4_source_test.dbo.Settings
SELECT COUNT(*) from sep4_dwh_test.stage.DimSettings


----------- Settings in the source are the same as the settings in stage --------
SELECT SUM(targetTemperature) from sep4_source_test.dbo.Settings
SELECT SUM(targetTemperature) from sep4_dwh_test.stage.DimSettings




----------- Count of Rooms in source should be the same as the count of rooms in stage -------
SELECT COUNT(*) from sep4_source_test.dbo.Rooms
SELECT COUNT(*) from sep4_dwh_test.stage.DimRoom





