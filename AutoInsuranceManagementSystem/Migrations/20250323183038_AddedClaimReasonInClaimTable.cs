using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AutoInsuranceManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class AddedClaimReasonInClaimTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ClaimReason",
                table: "Claims",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClaimReason",
                table: "Claims");
        }
    }
}
