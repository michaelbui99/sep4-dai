using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebAPI.Migrations
{
    public partial class _0_InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Settings",
                columns: table => new
                {
                    SettingId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Co2Threshold = table.Column<int>(type: "int", nullable: false),
                    HumidityThreshold = table.Column<int>(type: "int", nullable: false),
                    TargetTemperature = table.Column<float>(type: "real", nullable: false),
                    TemperatureMargin = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Settings", x => x.SettingId);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "Rooms",
                columns: table => new
                {
                    RoomId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoomName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SettingsSettingId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rooms", x => x.RoomId);
                    table.ForeignKey(
                        name: "FK_Rooms_Settings_SettingsSettingId",
                        column: x => x.SettingsSettingId,
                        principalTable: "Settings",
                        principalColumn: "SettingId");
                });

            migrationBuilder.CreateTable(
                name: "ClimateDevice",
                columns: table => new
                {
                    ClimateDeviceId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SettingsSettingId = table.Column<int>(type: "int", nullable: true),
                    RoomId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClimateDevice", x => x.ClimateDeviceId);
                    table.ForeignKey(
                        name: "FK_ClimateDevice_Rooms_RoomId",
                        column: x => x.RoomId,
                        principalTable: "Rooms",
                        principalColumn: "RoomId");
                    table.ForeignKey(
                        name: "FK_ClimateDevice_Settings_SettingsSettingId",
                        column: x => x.SettingsSettingId,
                        principalTable: "Settings",
                        principalColumn: "SettingId");
                });

            migrationBuilder.CreateTable(
                name: "Actuator",
                columns: table => new
                {
                    ActuatorId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClimateDeviceId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Actuator", x => x.ActuatorId);
                    table.ForeignKey(
                        name: "FK_Actuator_ClimateDevice_ClimateDeviceId",
                        column: x => x.ClimateDeviceId,
                        principalTable: "ClimateDevice",
                        principalColumn: "ClimateDeviceId");
                });

            migrationBuilder.CreateTable(
                name: "Measurements",
                columns: table => new
                {
                    MeasurementId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Temperature = table.Column<float>(type: "real", nullable: false),
                    Humidity = table.Column<int>(type: "int", nullable: false),
                    Co2 = table.Column<int>(type: "int", nullable: false),
                    ClimateDeviceId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Measurements", x => x.MeasurementId);
                    table.ForeignKey(
                        name: "FK_Measurements_ClimateDevice_ClimateDeviceId",
                        column: x => x.ClimateDeviceId,
                        principalTable: "ClimateDevice",
                        principalColumn: "ClimateDeviceId");
                });

            migrationBuilder.CreateTable(
                name: "Sensor",
                columns: table => new
                {
                    SensorId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClimateDeviceId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sensor", x => x.SensorId);
                    table.ForeignKey(
                        name: "FK_Sensor_ClimateDevice_ClimateDeviceId",
                        column: x => x.ClimateDeviceId,
                        principalTable: "ClimateDevice",
                        principalColumn: "ClimateDeviceId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Actuator_ClimateDeviceId",
                table: "Actuator",
                column: "ClimateDeviceId");

            migrationBuilder.CreateIndex(
                name: "IX_ClimateDevice_RoomId",
                table: "ClimateDevice",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_ClimateDevice_SettingsSettingId",
                table: "ClimateDevice",
                column: "SettingsSettingId");

            migrationBuilder.CreateIndex(
                name: "IX_Measurements_ClimateDeviceId",
                table: "Measurements",
                column: "ClimateDeviceId");

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_SettingsSettingId",
                table: "Rooms",
                column: "SettingsSettingId");

            migrationBuilder.CreateIndex(
                name: "IX_Sensor_ClimateDeviceId",
                table: "Sensor",
                column: "ClimateDeviceId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Actuator");

            migrationBuilder.DropTable(
                name: "Measurements");

            migrationBuilder.DropTable(
                name: "Sensor");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "ClimateDevice");

            migrationBuilder.DropTable(
                name: "Rooms");

            migrationBuilder.DropTable(
                name: "Settings");
        }
    }
}
