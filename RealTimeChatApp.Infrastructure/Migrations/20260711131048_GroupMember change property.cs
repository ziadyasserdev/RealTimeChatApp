using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RealTimeChatApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class GroupMemberchangeproperty : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastReadMessageId",
                table: "GroupMembers");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "LastReadMessageId",
                table: "GroupMembers",
                type: "uniqueidentifier",
                nullable: true);
        }
    }
}
