using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SmartFactorySystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ExpandMachineStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MachineStatuses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    MachineId = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    MachineName = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Status = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    StopReason = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true),
                    PlannedProductionCount = table.Column<int>(type: "INTEGER", nullable: false),
                    ActualProductionCount = table.Column<int>(type: "INTEGER", nullable: false),
                    DefectCount = table.Column<int>(type: "INTEGER", nullable: false),
                    LastUpdated = table.Column<DateTime>(type: "TEXT", nullable: false),
                    LastStatusChangedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MachineStatuses", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "MachineStatuses",
                columns: new[] { "Id", "ActualProductionCount", "DefectCount", "LastStatusChangedAt", "LastUpdated", "MachineId", "MachineName", "PlannedProductionCount", "Status", "StopReason" },
                values: new object[,]
                {
                    { 1, 450, 2, new DateTime(2026, 6, 1, 8, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), "LINE-A-PRSS", "Aライン 高速プレス機", 1000, "Running", null },
                    { 2, 120, 0, new DateTime(2026, 6, 1, 10, 30, 0, 0, DateTimeKind.Utc), new DateTime(2026, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), "LINE-B-WELD", "Bライン 溶接ロボットアーム", 500, "PlannedStop", "型替え作業中（段取り）" },
                    { 3, 1800, 45, new DateTime(2026, 6, 1, 11, 15, 0, 0, DateTimeKind.Utc), new DateTime(2026, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), "LINE-C-PCKG", "Cライン 自動梱包機", 2000, "Error", "搬送ベルト コンボイ詰まり" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MachineStatuses");
        }
    }
}
