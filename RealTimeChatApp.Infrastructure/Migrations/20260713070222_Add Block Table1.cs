using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RealTimeChatApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddBlockTable1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ForwardedFromUserId",
                table: "PrivateMessages",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsForwarded",
                table: "PrivateMessages",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "ForwardedFromUserId",
                table: "GroupMessages",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsForwarded",
                table: "GroupMessages",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_PrivateMessages_ForwardedFromUserId",
                table: "PrivateMessages",
                column: "ForwardedFromUserId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupMessages_ForwardedFromUserId",
                table: "GroupMessages",
                column: "ForwardedFromUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_GroupMessages_AspNetUsers_ForwardedFromUserId",
                table: "GroupMessages",
                column: "ForwardedFromUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PrivateMessages_AspNetUsers_ForwardedFromUserId",
                table: "PrivateMessages",
                column: "ForwardedFromUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GroupMessages_AspNetUsers_ForwardedFromUserId",
                table: "GroupMessages");

            migrationBuilder.DropForeignKey(
                name: "FK_PrivateMessages_AspNetUsers_ForwardedFromUserId",
                table: "PrivateMessages");

            migrationBuilder.DropIndex(
                name: "IX_PrivateMessages_ForwardedFromUserId",
                table: "PrivateMessages");

            migrationBuilder.DropIndex(
                name: "IX_GroupMessages_ForwardedFromUserId",
                table: "GroupMessages");

            migrationBuilder.DropColumn(
                name: "ForwardedFromUserId",
                table: "PrivateMessages");

            migrationBuilder.DropColumn(
                name: "IsForwarded",
                table: "PrivateMessages");

            migrationBuilder.DropColumn(
                name: "ForwardedFromUserId",
                table: "GroupMessages");

            migrationBuilder.DropColumn(
                name: "IsForwarded",
                table: "GroupMessages");
        }
    }
}
