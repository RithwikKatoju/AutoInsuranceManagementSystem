using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AutoInsuranceManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class PolicyIdInClaimTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "PolicyId1",
                table: "Tickets",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_PolicyId1",
                table: "Tickets",
                column: "PolicyId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_Policies_PolicyId1",
                table: "Tickets",
                column: "PolicyId1",
                principalTable: "Policies",
                principalColumn: "PolicyId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_Policies_PolicyId1",
                table: "Tickets");

            migrationBuilder.DropIndex(
                name: "IX_Tickets_PolicyId1",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "PolicyId1",
                table: "Tickets");
        }
    }
}
