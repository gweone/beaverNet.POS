using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace beaverNet.POS.WebApp.Data.Migrations
{
    public partial class MedicalRecord : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DateofBirthday",
                table: "Customer",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.CreateTable(
                name: "MedicalRecord",
                columns: table => new
                {
                    MedicalRecordId = table.Column<Guid>(nullable: false),
                    RecordDate = table.Column<DateTimeOffset>(nullable: false),
                    PyshicalCheck = table.Column<string>(nullable: true),
                    Diagnosis = table.Column<string>(nullable: true),
                    Therapy = table.Column<string>(nullable: true),
                    CustomerId = table.Column<Guid>(nullable: false),
                    SalesOrderId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedicalRecord", x => x.MedicalRecordId);
                    table.ForeignKey(
                        name: "FK_MedicalRecord_Customer_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customer",
                        principalColumn: "CustomerId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MedicalRecord_SalesOrder_SalesOrderId",
                        column: x => x.SalesOrderId,
                        principalTable: "SalesOrder",
                        principalColumn: "SalesOrderId",
                        onDelete: ReferentialAction.Cascade);
                });

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MedicalRecord");

            migrationBuilder.DropColumn(
                name: "DateofBirthday",
                table: "Customer");

        }
    }
}
