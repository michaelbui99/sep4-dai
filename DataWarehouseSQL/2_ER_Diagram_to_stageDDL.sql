use sep4_dwh
go

CREATE TABLE stage.DimClimateDevice
(
    ClimateDeviceId NVARCHAR(50) NOT NULL
);


ALTER TABLE stage.DimClimateDevice
    ADD CONSTRAINT PK_DimClimateDevice PRIMARY KEY (ClimateDeviceId);


CREATE TABLE stage.DimRoom
(
    RoomId   INT NOT NULL,
    RoomName NVARCHAR(6)
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



