using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class TopicModelFixes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "proposer_id",
                table: "topic",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "supervisor_id",
                table: "topic",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "ix_topic_proposer_id",
                table: "topic",
                column: "proposer_id");

            migrationBuilder.CreateIndex(
                name: "ix_topic_supervisor_id",
                table: "topic",
                column: "supervisor_id");

            migrationBuilder.AddForeignKey(
                name: "fk_topic_user_proposer_id",
                table: "topic",
                column: "proposer_id",
                principalTable: "user",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_topic_user_supervisor_id",
                table: "topic",
                column: "supervisor_id",
                principalTable: "user",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_topic_user_proposer_id",
                table: "topic");

            migrationBuilder.DropForeignKey(
                name: "fk_topic_user_supervisor_id",
                table: "topic");

            migrationBuilder.DropIndex(
                name: "ix_topic_proposer_id",
                table: "topic");

            migrationBuilder.DropIndex(
                name: "ix_topic_supervisor_id",
                table: "topic");

            migrationBuilder.DropColumn(
                name: "proposer_id",
                table: "topic");

            migrationBuilder.DropColumn(
                name: "supervisor_id",
                table: "topic");
        }
    }
}
