CREATE TABLE Stage.DimClimateDevice
(
    ClimateDeviceId INT
);


ALTER TABLE Stage.DimClimateDevice
    ADD CONSTRAINT PK_DimClimateDevice PRIMARY KEY (ClimateDeviceId);


CREATE TABLE Stage.DimRoom
(
    RoomId   INT,
    RoomName NVARCHAR(6)
);

ALTER TABLE Stage.DimRoom
    ADD CONSTRAINT PK_DimRoom PRIMARY KEY (RoomId);


CREATE TABLE Stage.DimSettings
(
    SettingsId        INT,
    HumidityThreshold INT,
    TargetTemperature FLOAT,
    TemperatureMargin INT,
    CO2Threshold      INT
);

ALTER TABLE Stage.DimSettings
    ADD CONSTRAINT PK_DimSettings PRIMARY KEY (SettingsId);


CREATE TABLE Stage.FactMeasurement
(
    RoomId              INT NOT NULL,
    SettingsId          INT NOT NULL,
    ClimateDeviceId     INT NOT NULL,
    CO2_In_PPM          INT,
    Temperature_In_C    FLOAT,
    Humidity_In_Percent INT
);

ALTER TABLE Stage.FactMeasurement
    ADD CONSTRAINT PK_FactMeasurement PRIMARY KEY (RoomId, SettingsId, ClimateDeviceId);


ALTER TABLE Stage.FactMeasurement
    ADD CONSTRAINT FK_FactMeasurement_0 FOREIGN KEY (RoomId) REFERENCES DimRoom (RoomId);
ALTER TABLE Stage.FactMeasurement
    ADD CONSTRAINT FK_FactMeasurement_1 FOREIGN KEY (SettingsId) REFERENCES DimSettings (SettingsId);
ALTER TABLE Stage.FactMeasurement
    ADD CONSTRAINT FK_FactMeasurement_2 FOREIGN KEY (ClimateDeviceId) REFERENCES DimClimateDevice (ClimateDeviceId);



