using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GO.DataAccess.MsSql.Migrations
{
    public partial class AddUserAndBudgetTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Budget",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsArchived = table.Column<bool>(type: "bit", nullable: false),
                    Payday = table.Column<byte>(type: "tinyint", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Budget", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Scopes = table.Column<byte>(type: "tinyint", nullable: false),
                    IsLocked = table.Column<bool>(type: "bit", nullable: false),
                    IsArchived = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BudgetRecord",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CategoryType = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<long>(type: "bigint", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    BudgetId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BudgetRecord", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BudgetRecord_Budget_BudgetId",
                        column: x => x.BudgetId,
                        principalTable: "Budget",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BudgetsUsers",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BudgetId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsDisabled = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BudgetsUsers", x => new { x.UserId, x.BudgetId });
                    table.ForeignKey(
                        name: "FK_BudgetsUsers_Budget_BudgetId",
                        column: x => x.BudgetId,
                        principalTable: "Budget",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BudgetsUsers_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserConnection",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NickName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ConnectionId = table.Column<long>(type: "bigint", maxLength: 50, nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    CurrentScope = table.Column<byte>(type: "tinyint", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserConnection", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserConnection_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Budget",
                columns: new[] { "Id", "CreatedBy", "CreatedDate", "IsArchived", "Payday", "UpdatedBy", "UpdatedDate" },
                values: new object[] { new Guid("936da01f-9abd-4d9d-80c7-02af85c822a8"), new Guid("936da01f-9abd-4d9d-80c7-02af85c822a8"), new DateTimeOffset(new DateTime(2021, 10, 14, 22, 11, 29, 713, DateTimeKind.Unspecified).AddTicks(5663), new TimeSpan(0, 0, 0, 0, 0)), false, (byte)0, null, null });

            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "Id", "CreatedBy", "CreatedDate", "FirstName", "IsArchived", "IsLocked", "LastName", "Scopes", "UpdatedBy", "UpdatedDate" },
                values: new object[] { new Guid("936da01f-9abd-4d9d-80c7-02af85c822a8"), new Guid("936da01f-9abd-4d9d-80c7-02af85c822a8"), new DateTimeOffset(new DateTime(2021, 10, 14, 22, 11, 29, 709, DateTimeKind.Unspecified).AddTicks(2007), new TimeSpan(0, 0, 0, 0, 0)), "Dmytro", false, false, "😇", (byte)14, null, null });

            migrationBuilder.InsertData(
                table: "BudgetsUsers",
                columns: new[] { "BudgetId", "UserId", "IsDisabled" },
                values: new object[] { new Guid("936da01f-9abd-4d9d-80c7-02af85c822a8"), new Guid("936da01f-9abd-4d9d-80c7-02af85c822a8"), false });

            migrationBuilder.InsertData(
                table: "UserConnection",
                columns: new[] { "Id", "ConnectionId", "CreatedBy", "CreatedDate", "CurrentScope", "NickName", "Type", "UpdatedBy", "UpdatedDate", "UserId" },
                values: new object[] { new Guid("936da01f-9abd-4d9d-80c7-02af85c822a8"), 428296956L, new Guid("936da01f-9abd-4d9d-80c7-02af85c822a8"), new DateTimeOffset(new DateTime(2021, 10, 14, 22, 11, 29, 712, DateTimeKind.Unspecified).AddTicks(5765), new TimeSpan(0, 0, 0, 0, 0)), (byte)8, "Nimb84", 1, null, null, new Guid("936da01f-9abd-4d9d-80c7-02af85c822a8") });

            migrationBuilder.CreateIndex(
                name: "IX_BudgetRecord_BudgetId",
                table: "BudgetRecord",
                column: "BudgetId");

            migrationBuilder.CreateIndex(
                name: "IX_BudgetsUsers_BudgetId",
                table: "BudgetsUsers",
                column: "BudgetId");

            migrationBuilder.CreateIndex(
                name: "IX_UserConnection_ConnectionId",
                table: "UserConnection",
                column: "ConnectionId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserConnection_UserId",
                table: "UserConnection",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BudgetRecord");

            migrationBuilder.DropTable(
                name: "BudgetsUsers");

            migrationBuilder.DropTable(
                name: "UserConnection");

            migrationBuilder.DropTable(
                name: "Budget");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
