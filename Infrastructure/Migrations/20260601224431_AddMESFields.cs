using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartFactorySystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddMESFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CurrentWorkOrder",
                table: "MachineStatuses",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "LastStartedAt",
                table: "MachineStatuses",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "StandardCycleTimeSeconds",
                table: "MachineStatuses",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.UpdateData(
                table: "MachineStatuses",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CurrentWorkOrder", "LastStartedAt", "StandardCycleTimeSeconds" },
                values: new object[] { "WO-2026-0001", null, 10.5 });

            migrationBuilder.UpdateData(
                table: "MachineStatuses",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CurrentWorkOrder", "LastStartedAt", "StandardCycleTimeSeconds" },
                values: new object[] { "WO-2026-0001", null, 10.5 });

            migrationBuilder.UpdateData(
                table: "MachineStatuses",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CurrentWorkOrder", "LastStartedAt", "StandardCycleTimeSeconds" },
                values: new object[] { "WO-2026-0001", null, 10.5 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CurrentWorkOrder",
                table: "MachineStatuses");

            migrationBuilder.DropColumn(
                name: "LastStartedAt",
                table: "MachineStatuses");

            migrationBuilder.DropColumn(
                name: "StandardCycleTimeSeconds",
                table: "MachineStatuses");
        }
    }
}
