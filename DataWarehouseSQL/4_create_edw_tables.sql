use sep4_dwh
go

-- check to see if table exists in sys.tables - ignore DROP TABLE if it does not
  IF EXISTS(SELECT * FROM sys.tables WHERE SCHEMA_NAME(schema_id) LIKE 'edw' AND name like 'FactMeasurement')  
   DROP TABLE [edw].[FactMeasurement];

IF EXISTS(SELECT * FROM sys.tables WHERE SCHEMA_NAME(schema_id) LIKE 'edw' AND name like 'DimClimateDevice')  
   DROP TABLE [edw].[DimClimateDevice];

   IF EXISTS(SELECT * FROM sys.tables WHERE SCHEMA_NAME(schema_id) LIKE 'edw' AND name like 'DimRoom')  
   DROP TABLE [edw].[DimRoom];

   IF EXISTS(SELECT * FROM sys.tables WHERE SCHEMA_NAME(schema_id) LIKE 'edw' AND name like 'DimSettings')  
   DROP TABLE [edw].[DimSettings];

 
CREATE TABLE edw.DimClimateDevice
(
	C_Id INT IDENTITY PRIMARY KEY NOT NULL,
    ClimateDeviceId NVARCHAR(50) NOT NULL
);


CREATE TABLE edw.DimRoom
(
	R_Id INT IDENTITY PRIMARY KEY NOT NULL,
	RoomId   INT NOT NULL,
    RoomName NVARCHAR(7)
);


CREATE TABLE edw.DimSettings
(
	S_ID INT IDENTITY PRIMARY KEY NOT NULL,
    SettingsId        INT NOT NULL,
    HumidityThreshold INT,
    TargetTemperature FLOAT,
    TemperatureMargin INT,
    CO2Threshold      INT
);


CREATE TABLE edw.FactMeasurement
(
    R_Id              INT REFERENCES edw.DimRoom(R_Id) NOT NULL,
    S_Id          INT REFERENCES edw.DimSettings(S_Id) NOT NULL,
    C_Id    INT REFERENCES edw.DimClimateDevice(C_Id) NOT NULL,
	MD_ID INT REFERENCES edw.DimDate(D_ID) NOT NULL, 
	MT_ID INT REFERENCES edw.DimTime(TimeKey),
    CO2_In_PPM          INT,
    Temperature_In_C    FLOAT,
    Humidity_In_Percent INT,
	PRIMARY KEY (R_Id, S_Id, C_Id, MD_ID, MT_ID)
);
