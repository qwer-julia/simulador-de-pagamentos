using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BenefitsManager.Migrations
{
    /// <inheritdoc />
    public partial class SeedBenefit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Benefits",
                columns: new[] { "Id", "DiscountPercentage", "Name" },
                values: new object[] { 1, 0f, "Desconto Zerado" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Benefits",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
