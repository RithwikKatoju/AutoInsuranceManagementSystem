using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AutoInsuranceManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class AddColoumnToTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Claims_AspNetUsers_AdjusterIdId",
                table: "Claims");

            migrationBuilder.RenameColumn(
                name: "AdjusterIdId",
                table: "Claims",
                newName: "AgentIdId");

            migrationBuilder.RenameIndex(
                name: "IX_Claims_AdjusterIdId",
                table: "Claims",
                newName: "IX_Claims_AgentIdId");

            migrationBuilder.AddColumn<string>(
                name: "AgentIdId",
                table: "Tickets",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Feedback",
                table: "Tickets",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AgentIdId",
                table: "Payments",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_AgentIdId",
                table: "Tickets",
                column: "AgentIdId");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_AgentIdId",
                table: "Payments",
                column: "AgentIdId");

            migrationBuilder.AddForeignKey(
                name: "FK_Claims_AspNetUsers_AgentIdId",
                table: "Claims",
                column: "AgentIdId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_AspNetUsers_AgentIdId",
                table: "Payments",
                column: "AgentIdId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_AspNetUsers_AgentIdId",
                table: "Tickets",
                column: "AgentIdId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Claims_AspNetUsers_AgentIdId",
                table: "Claims");

            migrationBuilder.DropForeignKey(
                name: "FK_Payments_AspNetUsers_AgentIdId",
                table: "Payments");

            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_AspNetUsers_AgentIdId",
                table: "Tickets");

            migrationBuilder.DropIndex(
                name: "IX_Tickets_AgentIdId",
                table: "Tickets");

            migrationBuilder.DropIndex(
                name: "IX_Payments_AgentIdId",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "AgentIdId",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "Feedback",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "AgentIdId",
                table: "Payments");

            migrationBuilder.RenameColumn(
                name: "AgentIdId",
                table: "Claims",
                newName: "AdjusterIdId");

            migrationBuilder.RenameIndex(
                name: "IX_Claims_AgentIdId",
                table: "Claims",
                newName: "IX_Claims_AdjusterIdId");

            migrationBuilder.AddForeignKey(
                name: "FK_Claims_AspNetUsers_AdjusterIdId",
                table: "Claims",
                column: "AdjusterIdId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
