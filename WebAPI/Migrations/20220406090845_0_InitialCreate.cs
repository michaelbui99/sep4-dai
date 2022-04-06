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
                        .Annotation("SqlServer:Identity", "1, 1")
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
                name: "ClimateDevices",
                columns: table => new
                {
                    ClimateDeviceId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SettingsSettingId = table.Column<int>(type: "int", nullable: true),
                    RoomId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClimateDevices", x => x.ClimateDeviceId);
                    table.ForeignKey(
                        name: "FK_ClimateDevices_Rooms_RoomId",
                        column: x => x.RoomId,
                        principalTable: "Rooms",
                        principalColumn: "RoomId");
                    table.ForeignKey(
                        name: "FK_ClimateDevices_Settings_SettingsSettingId",
                        column: x => x.SettingsSettingId,
                        principalTable: "Settings",
                        principalColumn: "SettingId");
                });

            migrationBuilder.CreateTable(
                name: "Actuators",
                columns: table => new
                {
                    ActuatorId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClimateDeviceId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Actuators", x => x.ActuatorId);
                    table.ForeignKey(
                        name: "FK_Actuators_ClimateDevices_ClimateDeviceId",
                        column: x => x.ClimateDeviceId,
                        principalTable: "ClimateDevices",
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
                    ClimateDeviceId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Measurements", x => x.MeasurementId);
                    table.ForeignKey(
                        name: "FK_Measurements_ClimateDevices_ClimateDeviceId",
                        column: x => x.ClimateDeviceId,
                        principalTable: "ClimateDevices",
                        principalColumn: "ClimateDeviceId");
                });

            migrationBuilder.CreateTable(
                name: "Sensors",
                columns: table => new
                {
                    SensorId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClimateDeviceId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sensors", x => x.SensorId);
                    table.ForeignKey(
                        name: "FK_Sensors_ClimateDevices_ClimateDeviceId",
                        column: x => x.ClimateDeviceId,
                        principalTable: "ClimateDevices",
                        principalColumn: "ClimateDeviceId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Actuators_ClimateDeviceId",
                table: "Actuators",
                column: "ClimateDeviceId");

            migrationBuilder.CreateIndex(
                name: "IX_ClimateDevices_RoomId",
                table: "ClimateDevices",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_ClimateDevices_SettingsSettingId",
                table: "ClimateDevices",
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
                name: "IX_Sensors_ClimateDeviceId",
                table: "Sensors",
                column: "ClimateDeviceId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Actuators");

            migrationBuilder.DropTable(
                name: "Measurements");

            migrationBuilder.DropTable(
                name: "Sensors");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "ClimateDevices");

            migrationBuilder.DropTable(
                name: "Rooms");

            migrationBuilder.DropTable(
                name: "Settings");
        }
    }
}
