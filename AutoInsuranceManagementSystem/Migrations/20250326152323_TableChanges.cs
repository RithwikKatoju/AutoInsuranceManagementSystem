using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AutoInsuranceManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class TableChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payments_Policies_PolicyId1",
                table: "Payments");

            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_Policies_PolicyId1",
                table: "Tickets");

            migrationBuilder.DropIndex(
                name: "IX_Tickets_PolicyId1",
                table: "Tickets");

            migrationBuilder.DropIndex(
                name: "IX_Payments_PolicyId1",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "PolicyId1",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "PolicyId1",
                table: "Payments");

            migrationBuilder.AddColumn<string>(
                name: "PolicyNumber",
                table: "Tickets",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PolicyNumber",
                table: "Payments",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PolicyNumber",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "PolicyNumber",
                table: "Payments");

            migrationBuilder.AddColumn<Guid>(
                name: "PolicyId1",
                table: "Tickets",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "PolicyId1",
                table: "Payments",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_PolicyId1",
                table: "Tickets",
                column: "PolicyId1");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_PolicyId1",
                table: "Payments",
                column: "PolicyId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_Policies_PolicyId1",
                table: "Payments",
                column: "PolicyId1",
                principalTable: "Policies",
                principalColumn: "PolicyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_Policies_PolicyId1",
                table: "Tickets",
                column: "PolicyId1",
                principalTable: "Policies",
                principalColumn: "PolicyId");
        }
    }
}
