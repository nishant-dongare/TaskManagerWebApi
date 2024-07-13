using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TaskmanagerMVCAPI.Migrations
{
    /// <inheritdoc />
    public partial class n7 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TaskAssignments_Batches_BatchId",
                table: "TaskAssignments");

            migrationBuilder.DropIndex(
                name: "IX_TaskAssignments_BatchId",
                table: "TaskAssignments");

            migrationBuilder.DropColumn(
                name: "BatchId",
                table: "TaskAssignments");

            migrationBuilder.InsertData(
                table: "Students",
                columns: new[] { "StudentId", "BatchId", "StudentName" },
                values: new object[,]
                {
                    { 1, 1, "s1b1" },
                    { 2, 1, "s2b1" },
                    { 3, 1, "s3b1" },
                    { 4, 1, "s4b1" },
                    { 5, 1, "s5b1" },
                    { 6, 2, "s1b2" },
                    { 7, 2, "s2b2" },
                    { 8, 2, "s3b2" },
                    { 9, 2, "s4b2" },
                    { 10, 2, "s5b2" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Students",
                keyColumn: "StudentId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Students",
                keyColumn: "StudentId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Students",
                keyColumn: "StudentId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Students",
                keyColumn: "StudentId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Students",
                keyColumn: "StudentId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Students",
                keyColumn: "StudentId",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Students",
                keyColumn: "StudentId",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Students",
                keyColumn: "StudentId",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Students",
                keyColumn: "StudentId",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Students",
                keyColumn: "StudentId",
                keyValue: 10);

            migrationBuilder.AddColumn<int>(
                name: "BatchId",
                table: "TaskAssignments",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TaskAssignments_BatchId",
                table: "TaskAssignments",
                column: "BatchId");

            migrationBuilder.AddForeignKey(
                name: "FK_TaskAssignments_Batches_BatchId",
                table: "TaskAssignments",
                column: "BatchId",
                principalTable: "Batches",
                principalColumn: "BatchId");
        }
    }
}
