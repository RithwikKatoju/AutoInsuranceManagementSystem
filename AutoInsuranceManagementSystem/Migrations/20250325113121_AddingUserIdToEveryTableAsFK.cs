using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AutoInsuranceManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class AddingUserIdToEveryTableAsFK : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "CoverageType",
                table: "Policies",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "CoverageAmount",
                table: "Policies",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserIdId",
                table: "Policies",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserIdId",
                table: "Payments",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserIdId",
                table: "Claims",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Policies_UserIdId",
                table: "Policies",
                column: "UserIdId");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_UserIdId",
                table: "Payments",
                column: "UserIdId");

            migrationBuilder.CreateIndex(
                name: "IX_Claims_UserIdId",
                table: "Claims",
                column: "UserIdId");

            migrationBuilder.AddForeignKey(
                name: "FK_Claims_AspNetUsers_UserIdId",
                table: "Claims",
                column: "UserIdId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_AspNetUsers_UserIdId",
                table: "Payments",
                column: "UserIdId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Policies_AspNetUsers_UserIdId",
                table: "Policies",
                column: "UserIdId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Claims_AspNetUsers_UserIdId",
                table: "Claims");

            migrationBuilder.DropForeignKey(
                name: "FK_Payments_AspNetUsers_UserIdId",
                table: "Payments");

            migrationBuilder.DropForeignKey(
                name: "FK_Policies_AspNetUsers_UserIdId",
                table: "Policies");

            migrationBuilder.DropIndex(
                name: "IX_Policies_UserIdId",
                table: "Policies");

            migrationBuilder.DropIndex(
                name: "IX_Payments_UserIdId",
                table: "Payments");

            migrationBuilder.DropIndex(
                name: "IX_Claims_UserIdId",
                table: "Claims");

            migrationBuilder.DropColumn(
                name: "UserIdId",
                table: "Policies");

            migrationBuilder.DropColumn(
                name: "UserIdId",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "UserIdId",
                table: "Claims");

            migrationBuilder.AlterColumn<int>(
                name: "CoverageType",
                table: "Policies",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<decimal>(
                name: "CoverageAmount",
                table: "Policies",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");
        }
    }
}
