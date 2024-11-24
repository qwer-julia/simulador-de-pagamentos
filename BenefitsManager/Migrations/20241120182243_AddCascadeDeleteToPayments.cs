using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BenefitsManager.Migrations
{
    /// <inheritdoc />
    public partial class AddCascadeDeleteToPayments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payments_Taxpayers_TaxpayerId",
                table: "Payments");

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_Taxpayers_TaxpayerId",
                table: "Payments",
                column: "TaxpayerId",
                principalTable: "Taxpayers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
