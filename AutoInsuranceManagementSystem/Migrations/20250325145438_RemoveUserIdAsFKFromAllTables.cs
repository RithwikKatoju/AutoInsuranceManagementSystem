using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AutoInsuranceManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class RemoveUserIdAsFKFromAllTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_AspNetUsers_UserIdId",
                table: "Tickets");

            migrationBuilder.DropIndex(
                name: "IX_Tickets_UserIdId",
                table: "Tickets");

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
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "UserIdId",
                table: "Policies");

            migrationBuilder.DropColumn(
                name: "UserIdId",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "UserIdId",
                table: "Claims");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Tickets",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Policies",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Payments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Claims",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Policies");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Claims");

            migrationBuilder.AddColumn<string>(
                name: "UserIdId",
                table: "Tickets",
                type: "nvarchar(450)",
                nullable: true);

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
                name: "IX_Tickets_UserIdId",
                table: "Tickets",
                column: "UserIdId");

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

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_AspNetUsers_UserIdId",
                table: "Tickets",
                column: "UserIdId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
