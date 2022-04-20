using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EFCore.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AuditTrail",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmailOrUserid = table.Column<string>(nullable: false),
                    Eventdateutc = table.Column<DateTime>(nullable: false),
                    Eventtype = table.Column<string>(maxLength: 1, nullable: false),
                    TableName = table.Column<string>(nullable: false),
                    RecordId = table.Column<long>(nullable: false),
                    OriginalValue = table.Column<string>(nullable: false),
                    NewValue = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditTrail", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Country",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CountryName = table.Column<string>(nullable: false),
                    PhonePrefix = table.Column<string>(maxLength: 3, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Country", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(nullable: false),
                    EmailAddress = table.Column<string>(maxLength: 200, nullable: false),
                    PhoneNumber = table.Column<string>(maxLength: 13, nullable: false),
                    PasswordSalt = table.Column<byte[]>(nullable: true),
                    PasswordHash = table.Column<byte[]>(nullable: true),
                    Status = table.Column<string>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    UserCountryId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Country_UserCountryId",
                        column: x => x.UserCountryId,
                        principalTable: "Country",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ComposedMessages",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Message = table.Column<string>(maxLength: 160, nullable: false),
                    MessageNo = table.Column<string>(nullable: true),
                    UserId = table.Column<long>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComposedMessages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ComposedMessages_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SendMessages",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SenderId = table.Column<long>(nullable: false),
                    MessageId = table.Column<long>(nullable: false),
                    RecipientPhoneNo = table.Column<string>(maxLength: 13, nullable: false),
                    MessageDelivered = table.Column<bool>(nullable: false),
                    TwiloResponse = table.Column<string>(nullable: true),
                    SentDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SendMessages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SendMessages_ComposedMessages_MessageId",
                        column: x => x.MessageId,
                        principalTable: "ComposedMessages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SendMessages_Users_SenderId",
                        column: x => x.SenderId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Country",
                columns: new[] { "Id", "CountryName", "PhonePrefix" },
                values: new object[] { 1, "UK", "+44" });

            migrationBuilder.CreateIndex(
                name: "IX_ComposedMessages_UserId",
                table: "ComposedMessages",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_SendMessages_MessageId",
                table: "SendMessages",
                column: "MessageId");

            migrationBuilder.CreateIndex(
                name: "IX_SendMessages_SenderId",
                table: "SendMessages",
                column: "SenderId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_UserCountryId",
                table: "Users",
                column: "UserCountryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuditTrail");

            migrationBuilder.DropTable(
                name: "SendMessages");

            migrationBuilder.DropTable(
                name: "ComposedMessages");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Country");
        }
    }
}
