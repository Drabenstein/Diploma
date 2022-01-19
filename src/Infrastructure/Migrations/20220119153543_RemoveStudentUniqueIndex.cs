using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class RemoveStudentUniqueIndex : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_application_topic_topic_id",
                table: "application");

            migrationBuilder.DropIndex(
                name: "ix_user_index_number",
                table: "user");

            migrationBuilder.AlterColumn<long>(
                name: "topic_id",
                table: "application",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "ix_user_index_number",
                table: "user",
                column: "index_number");

            migrationBuilder.AddForeignKey(
                name: "fk_application_topic_topic_id",
                table: "application",
                column: "topic_id",
                principalTable: "topic",
                principalColumn: "topic_id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_application_topic_topic_id",
                table: "application");

            migrationBuilder.DropIndex(
                name: "ix_user_index_number",
                table: "user");

            migrationBuilder.AlterColumn<long>(
                name: "topic_id",
                table: "application",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.CreateIndex(
                name: "ix_user_index_number",
                table: "user",
                column: "index_number",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "fk_application_topic_topic_id",
                table: "application",
                column: "topic_id",
                principalTable: "topic",
                principalColumn: "topic_id");
        }
    }
}
