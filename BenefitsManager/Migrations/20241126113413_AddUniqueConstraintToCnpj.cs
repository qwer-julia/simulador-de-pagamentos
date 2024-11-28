using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BenefitsManager.Migrations
{
    /// <inheritdoc />
    public partial class AddUniqueConstraintToCnpj : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Taxpayers_Cnpj",
                table: "Taxpayers",
                column: "Cnpj",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Taxpayers_Cnpj",
                table: "Taxpayers");
        }
    }
}
