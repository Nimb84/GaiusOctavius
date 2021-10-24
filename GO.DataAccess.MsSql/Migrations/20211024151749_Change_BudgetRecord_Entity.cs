using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GO.DataAccess.MsSql.Migrations
{
    public partial class Change_BudgetRecord_Entity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "BudgetRecord");

            migrationBuilder.UpdateData(
                table: "Budget",
                keyColumn: "Id",
                keyValue: new Guid("936da01f-9abd-4d9d-80c7-02af85c822a8"),
                column: "CreatedDate",
                value: new DateTimeOffset(new DateTime(2021, 10, 24, 15, 17, 46, 968, DateTimeKind.Unspecified).AddTicks(6066), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: new Guid("936da01f-9abd-4d9d-80c7-02af85c822a8"),
                column: "CreatedDate",
                value: new DateTimeOffset(new DateTime(2021, 10, 24, 15, 17, 46, 963, DateTimeKind.Unspecified).AddTicks(7909), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "UserConnection",
                keyColumn: "Id",
                keyValue: new Guid("936da01f-9abd-4d9d-80c7-02af85c822a8"),
                column: "CreatedDate",
                value: new DateTimeOffset(new DateTime(2021, 10, 24, 15, 17, 46, 967, DateTimeKind.Unspecified).AddTicks(3932), new TimeSpan(0, 0, 0, 0, 0)));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "BudgetRecord",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Budget",
                keyColumn: "Id",
                keyValue: new Guid("936da01f-9abd-4d9d-80c7-02af85c822a8"),
                column: "CreatedDate",
                value: new DateTimeOffset(new DateTime(2021, 10, 14, 22, 11, 29, 713, DateTimeKind.Unspecified).AddTicks(5663), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: new Guid("936da01f-9abd-4d9d-80c7-02af85c822a8"),
                column: "CreatedDate",
                value: new DateTimeOffset(new DateTime(2021, 10, 14, 22, 11, 29, 709, DateTimeKind.Unspecified).AddTicks(2007), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "UserConnection",
                keyColumn: "Id",
                keyValue: new Guid("936da01f-9abd-4d9d-80c7-02af85c822a8"),
                column: "CreatedDate",
                value: new DateTimeOffset(new DateTime(2021, 10, 14, 22, 11, 29, 712, DateTimeKind.Unspecified).AddTicks(5765), new TimeSpan(0, 0, 0, 0, 0)));
        }
    }
}
