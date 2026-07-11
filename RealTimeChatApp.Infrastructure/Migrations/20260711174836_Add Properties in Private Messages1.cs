using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RealTimeChatApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddPropertiesinPrivateMessages1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ReplyToMessageId",
                table: "PrivateMessages",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PrivateMessages_ReplyToMessageId",
                table: "PrivateMessages",
                column: "ReplyToMessageId");

            migrationBuilder.AddForeignKey(
                name: "FK_PrivateMessages_PrivateMessages_ReplyToMessageId",
                table: "PrivateMessages",
                column: "ReplyToMessageId",
                principalTable: "PrivateMessages",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PrivateMessages_PrivateMessages_ReplyToMessageId",
                table: "PrivateMessages");

            migrationBuilder.DropIndex(
                name: "IX_PrivateMessages_ReplyToMessageId",
                table: "PrivateMessages");

            migrationBuilder.DropColumn(
                name: "ReplyToMessageId",
                table: "PrivateMessages");
        }
    }
}
