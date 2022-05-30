using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebAPI.Migrations
{
    public partial class _02_AddIndexes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Settings_SettingId",
                table: "Settings",
                column: "SettingId");

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_RoomName",
                table: "Rooms",
                column: "RoomName");

            migrationBuilder.CreateIndex(
                name: "IX_Measurements_Timestamp",
                table: "Measurements",
                column: "Timestamp");

            migrationBuilder.CreateIndex(
                name: "IX_Devices_ClimateDeviceId",
                table: "Devices",
                column: "ClimateDeviceId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Settings_SettingId",
                table: "Settings");

            migrationBuilder.DropIndex(
                name: "IX_Rooms_RoomName",
                table: "Rooms");

            migrationBuilder.DropIndex(
                name: "IX_Measurements_Timestamp",
                table: "Measurements");

            migrationBuilder.DropIndex(
                name: "IX_Devices_ClimateDeviceId",
                table: "Devices");
        }
    }
}
