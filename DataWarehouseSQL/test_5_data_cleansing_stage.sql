--DATA cleansing
USE sep4_dwh_test
GO

--Set Temp to -999 on a null value to alert the user for an invalid reading
UPDATE [stage].[FactMeasurement]
SET Temperature_In_C = -999
WHERE Temperature_In_C IS NULL

--Set CO2 level to -1 on a null value to alert the user for an invalid reading
UPDATE [stage].[FactMeasurement]
SET CO2_In_PPM = -1
WHERE CO2_In_PPM IS NULL

--Set Humidity level to -1 on a null value to alert the user for an invalid reading
UPDATE [stage].[FactMeasurement]
SET Humidity_In_Percent = -1
WHERE Humidity_In_Percent IS NULL


