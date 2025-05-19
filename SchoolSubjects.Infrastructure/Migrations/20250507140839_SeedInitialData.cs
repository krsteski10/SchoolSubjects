using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SchoolSubjects.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedInitialData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Subjects",
                columns: new[] { "Id", "Description", "Name", "NumberOfWeeklyClasses" },
                values: new object[,]
                {
                    { 1, "Mathematics focuses on numbers and problem-solving.", "Math", 5 },
                    { 2, "English covers literature, writing, and grammar.", "English", 4 },
                    { 3, "Art encourages creativity through drawing and painting.", "Art", 2 }
                });

            migrationBuilder.InsertData(
                table: "Literature",
                columns: new[] { "Id", "SubjectId", "Title" },
                values: new object[,]
                {
                    { 1, 1, "Mathematics for Beginners" },
                    { 2, 1, "Advanced Algebra" },
                    { 3, 2, "Shakespeare's Sonnets" },
                    { 4, 2, "Modern English Grammar" },
                    { 5, 3, "The Art Book" },
                    { 6, 3, "Drawing on the Right Side of the Brain" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Literature",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Literature",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Literature",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Literature",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Literature",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Literature",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
