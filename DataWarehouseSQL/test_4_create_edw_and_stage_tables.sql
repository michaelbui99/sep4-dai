use sep4_dwh_test
go

-- check to see if table exists in sys.tables - ignore DROP TABLE if it does not
IF EXISTS(SELECT * FROM sys.tables WHERE SCHEMA_NAME(schema_id) LIKE 'edw' AND name like 'DimClimateDevice')  
   DROP TABLE [edw].[DimClimateDevice];

   IF EXISTS(SELECT * FROM sys.tables WHERE SCHEMA_NAME(schema_id) LIKE 'edw' AND name like 'DimRoom')  
   DROP TABLE [edw].[DimRoom];

   IF EXISTS(SELECT * FROM sys.tables WHERE SCHEMA_NAME(schema_id) LIKE 'edw' AND name like 'DimSettings')  
   DROP TABLE [edw].[DimSettings];

   IF EXISTS(SELECT * FROM sys.tables WHERE SCHEMA_NAME(schema_id) LIKE 'edw' AND name like 'FactMeasurement')  
   DROP TABLE [edw].[FactMeasurement];

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
    CO2_In_PPM          INT,
    Temperature_In_C    FLOAT,
    Humidity_In_Percent INT,
    MT_ID INT REFERENCES edw.DimTime(TimeKey) NOT NULL,
    MD_ID INT REFERENCES edw.DimDate(D_ID) NOT NULL ,
	PRIMARY KEY (R_Id, S_Id, C_Id, MT_ID, MD_ID)
);


CREATE TABLE stage.DimClimateDevice
(
    ClimateDeviceId NVARCHAR(50) NOT NULL
);


ALTER TABLE stage.DimClimateDevice
    ADD CONSTRAINT PK_DimClimateDevice PRIMARY KEY (ClimateDeviceId);


CREATE TABLE stage.DimRoom
(
    RoomId   INT NOT NULL,
    RoomName NVARCHAR(7)
);

ALTER TABLE stage.DimRoom
    ADD CONSTRAINT PK_DimRoom PRIMARY KEY (RoomId);


CREATE TABLE stage.DimSettings
(
    SettingsId        INT NOT NULL,
    HumidityThreshold INT,
    TargetTemperature FLOAT,
    TemperatureMargin INT,
    CO2Threshold      INT
);

ALTER TABLE stage.DimSettings
    ADD CONSTRAINT PK_DimSettings PRIMARY KEY (SettingsId);


CREATE TABLE stage.FactMeasurement
(
    RoomId              INT NOT NULL,
    SettingsId          INT NOT NULL,
    ClimateDeviceId     nvarchar(50) NOT NULL,
    CO2_In_PPM          INT,
    Temperature_In_C    FLOAT,
    Humidity_In_Percent INT,
    Measurement_Time TIME NOT NULL,
    Measurement_Date DATE NOT NULL 
);

ALTER TABLE stage.FactMeasurement
    ADD CONSTRAINT PK_FactMeasurement PRIMARY KEY (RoomId, SettingsId, ClimateDeviceId, Measurement_Time, Measurement_Date);


--ALTER TABLE Stage.FactMeasurement
--    ADD CONSTRAINT FK_FactMeasurement_0 FOREIGN KEY (RoomId) REFERENCES DimRoom (RoomId);
--ALTER TABLE Stage.FactMeasurement
--    ADD CONSTRAINT FK_FactMeasurement_1 FOREIGN KEY (SettingsId) REFERENCES DimSettings (SettingsId);
--ALTER TABLE Stage.FactMeasurement
--    ADD CONSTRAINT FK_FactMeasurement_2 FOREIGN KEY (ClimateDeviceId) REFERENCES DimClimateDevice (ClimateDeviceId);



