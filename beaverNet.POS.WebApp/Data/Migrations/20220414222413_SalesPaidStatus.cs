using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace beaverNet.POS.WebApp.Data.Migrations
{
    public partial class SalesPaidStatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "PaidStatus",
                table: "SalesOrder",
                nullable: false,
                defaultValue: false);

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PaidStatus",
                table: "SalesOrder");
        }
    }
}
