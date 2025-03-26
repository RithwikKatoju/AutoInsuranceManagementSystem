using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AutoInsuranceManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class ChangePolicyIdInClaimTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Claims_Policies_PolicyId1",
                table: "Claims");

            migrationBuilder.DropIndex(
                name: "IX_Claims_PolicyId1",
                table: "Claims");

            migrationBuilder.DropColumn(
                name: "PolicyId1",
                table: "Claims");

            migrationBuilder.AddColumn<string>(
                name: "PolicyNumber",
                table: "Claims",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PolicyNumber",
                table: "Claims");

            migrationBuilder.AddColumn<Guid>(
                name: "PolicyId1",
                table: "Claims",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Claims_PolicyId1",
                table: "Claims",
                column: "PolicyId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Claims_Policies_PolicyId1",
                table: "Claims",
                column: "PolicyId1",
                principalTable: "Policies",
                principalColumn: "PolicyId");
        }
    }
}
