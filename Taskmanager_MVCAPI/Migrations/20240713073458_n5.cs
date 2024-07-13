using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TaskmanagerMVCAPI.Migrations
{
    /// <inheritdoc />
    public partial class n5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "DueDate",
                table: "NewTasks",
                type: "datetime2",
                nullable: true,
                defaultValueSql: "DATEADD(HOUR, 24, GETDATE())",
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.InsertData(
                table: "Batches",
                columns: new[] { "BatchId", "BatchName" },
                values: new object[,]
                {
                    { 1, "Batch1" },
                    { 2, "Batch2" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Batches",
                keyColumn: "BatchId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Batches",
                keyColumn: "BatchId",
                keyValue: 2);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DueDate",
                table: "NewTasks",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValueSql: "DATEADD(HOUR, 24, GETDATE())");
        }
    }
}
