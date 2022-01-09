using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "area_of_interest",
                columns: table => new
                {
                    area_of_interest_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_area_of_interest", x => x.area_of_interest_id);
                });

            migrationBuilder.CreateTable(
                name: "field_of_study",
                columns: table => new
                {
                    field_of_study_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    lecture_language = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    study_form = table.Column<string>(type: "text", nullable: false),
                    hours_for_thesis = table.Column<int>(type: "integer", nullable: false),
                    degree = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_field_of_study", x => x.field_of_study_id);
                });

            migrationBuilder.CreateTable(
                name: "role",
                columns: table => new
                {
                    role_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_role", x => x.role_id);
                });

            migrationBuilder.CreateTable(
                name: "user",
                columns: table => new
                {
                    user_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    first_name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    last_name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    email = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    discriminator = table.Column<string>(type: "text", nullable: false),
                    index_number = table.Column<int>(type: "integer", nullable: true),
                    pensum = table.Column<int>(type: "integer", nullable: true),
                    position = table.Column<string>(type: "text", nullable: true),
                    has_consent_to_extend_pensum = table.Column<bool>(type: "boolean", nullable: true),
                    department = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    academic_degree = table.Column<string>(type: "text", nullable: true),
                    is_positive_faculty_opinion = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user", x => x.user_id);
                });

            migrationBuilder.CreateTable(
                name: "area_of_interest_user",
                columns: table => new
                {
                    area_of_interest_id = table.Column<long>(type: "bigint", nullable: false),
                    user_id = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_area_of_interest_user", x => new { x.area_of_interest_id, x.user_id });
                    table.ForeignKey(
                        name: "fk_area_of_interest_user_area_of_interest_area_of_interest_id",
                        column: x => x.area_of_interest_id,
                        principalTable: "area_of_interest",
                        principalColumn: "area_of_interest_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_area_of_interest_user_user_user_id",
                        column: x => x.user_id,
                        principalTable: "user",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "role_user",
                columns: table => new
                {
                    role_id = table.Column<long>(type: "bigint", nullable: false),
                    user_id = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_role_user", x => new {x.role_id, x.user_id });
                    table.ForeignKey(
                        name: "fk_role_user_role_role_id",
                        column: x => x.role_id,
                        principalTable: "role",
                        principalColumn: "role_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_role_user_user_user_id",
                        column: x => x.user_id,
                        principalTable: "user",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "student_field_of_study",
                columns: table => new
                {
                    student_id = table.Column<long>(type: "bigint", nullable: false),
                    field_of_study_id = table.Column<long>(type: "bigint", nullable: false),
                    semester = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    specialization = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    planned_year_of_defence = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_student_field_of_study", x => new { x.student_id, x.field_of_study_id });
                    table.ForeignKey(
                        name: "fk_student_field_of_study_field_of_study_field_of_study_id",
                        column: x => x.field_of_study_id,
                        principalTable: "field_of_study",
                        principalColumn: "field_of_study_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_student_field_of_study_user_student_id",
                        column: x => x.student_id,
                        principalTable: "user",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "topic",
                columns: table => new
                {
                    topic_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    proposer_id = table.Column<long>(type: "bigint", nullable: false),
                    supervisor_id = table.Column<long>(type: "bigint", nullable: false),
                    name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    english_name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    is_accepted = table.Column<bool>(type: "boolean", nullable: true),
                    is_free = table.Column<bool>(type: "boolean", nullable: false),
                    max_realization_number = table.Column<int>(type: "integer", nullable: false),
                    year_of_defence = table.Column<string>(type: "text", nullable: false),
                    is_proposed_by_student = table.Column<bool>(type: "boolean", nullable: false),
                    field_of_study_id = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_topic", x => x.topic_id);
                    table.ForeignKey(
                        name: "fk_topic_field_of_study_field_of_study_id",
                        column: x => x.field_of_study_id,
                        principalTable: "field_of_study",
                        principalColumn: "field_of_study_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_topic_user_proposer_id",
                        column: x => x.proposer_id,
                        principalTable: "user",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_topic_user_supervisor_id",
                        column: x => x.supervisor_id,
                        principalTable: "user",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "application",
                columns: table => new
                {
                    application_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    submitter_id = table.Column<long>(type: "bigint", nullable: false),
                    topic_id = table.Column<long>(type: "bigint", nullable: true),
                    timestamp = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    message = table.Column<string>(type: "character varying(4000)", maxLength: 4000, nullable: false),
                    status = table.Column<string>(type: "text", nullable: false),
                    is_topic_proposal = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_application", x => x.application_id);
                    table.ForeignKey(
                        name: "fk_application_topic_topic_id",
                        column: x => x.topic_id,
                        principalTable: "topic",
                        principalColumn: "topic_id");
                    table.ForeignKey(
                        name: "fk_application_user_submitter_id",
                        column: x => x.submitter_id,
                        principalTable: "user",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "thesis",
                columns: table => new
                {
                    thesis_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    topic_id = table.Column<long>(type: "bigint", nullable: false),
                    status = table.Column<string>(type: "text", nullable: false),
                    content = table.Column<byte[]>(type: "bytea", nullable: true),
                    file_format = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    language = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    has_consent_to_change_language = table.Column<bool>(type: "boolean", nullable: true),
                    realizer_student_id = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_thesis", x => x.thesis_id);
                    table.ForeignKey(
                        name: "fk_thesis_topic_topic_id",
                        column: x => x.topic_id,
                        principalTable: "topic",
                        principalColumn: "topic_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_thesis_user_realizer_student_id",
                        column: x => x.realizer_student_id,
                        principalTable: "user",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "declaration",
                columns: table => new
                {
                    declaration_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    objective_of_work = table.Column<string>(type: "character varying(4000)", maxLength: 4000, nullable: false),
                    operating_range = table.Column<string>(type: "character varying(4000)", maxLength: 4000, nullable: false),
                    language = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    date = table.Column<DateOnly>(type: "date", nullable: false),
                    thesis_id = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_declaration", x => x.declaration_id);
                    table.ForeignKey(
                        name: "fk_declaration_thesis_thesis_id",
                        column: x => x.thesis_id,
                        principalTable: "thesis",
                        principalColumn: "thesis_id");
                });

            migrationBuilder.CreateTable(
                name: "review",
                columns: table => new
                {
                    review_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    grade = table.Column<string>(type: "text", nullable: true),
                    is_published = table.Column<bool>(type: "boolean", nullable: false),
                    publish_timestamp = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    thesis_id = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_review", x => x.review_id);
                    table.ForeignKey(
                        name: "fk_review_thesis_thesis_id",
                        column: x => x.thesis_id,
                        principalTable: "thesis",
                        principalColumn: "thesis_id");
                });

            migrationBuilder.CreateTable(
                name: "review_module",
                columns: table => new
                {
                    review_module_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    value = table.Column<string>(type: "text", nullable: false),
                    type = table.Column<string>(type: "text", nullable: false),
                    review_id = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_review_module", x => x.review_module_id);
                    table.ForeignKey(
                        name: "fk_review_module_review_review_id",
                        column: x => x.review_id,
                        principalTable: "review",
                        principalColumn: "review_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_application_submitter_id",
                table: "application",
                column: "submitter_id");

            migrationBuilder.CreateIndex(
                name: "ix_application_topic_id",
                table: "application",
                column: "topic_id");

            migrationBuilder.CreateIndex(
                name: "ix_area_of_interest_user_user_id",
                table: "area_of_interest_user",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_declaration_thesis_id",
                table: "declaration",
                column: "thesis_id");

            migrationBuilder.CreateIndex(
                name: "ix_review_thesis_id",
                table: "review",
                column: "thesis_id");

            migrationBuilder.CreateIndex(
                name: "ix_review_module_review_id",
                table: "review_module",
                column: "review_id");

            migrationBuilder.CreateIndex(
                name: "ix_role_user_user_id",
                table: "role_user",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_student_field_of_study_field_of_study_id",
                table: "student_field_of_study",
                column: "field_of_study_id");

            migrationBuilder.CreateIndex(
                name: "ix_thesis_realizer_student_id",
                table: "thesis",
                column: "realizer_student_id");

            migrationBuilder.CreateIndex(
                name: "ix_thesis_topic_id",
                table: "thesis",
                column: "topic_id");

            migrationBuilder.CreateIndex(
                name: "ix_topic_field_of_study_id",
                table: "topic",
                column: "field_of_study_id");

            migrationBuilder.CreateIndex(
                name: "ix_topic_proposer_id",
                table: "topic",
                column: "proposer_id");

            migrationBuilder.CreateIndex(
                name: "ix_topic_supervisor_id",
                table: "topic",
                column: "supervisor_id");

            migrationBuilder.CreateIndex(
                name: "ix_user_email",
                table: "user",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_user_index_number",
                table: "user",
                column: "index_number",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "application");

            migrationBuilder.DropTable(
                name: "area_of_interest_user");

            migrationBuilder.DropTable(
                name: "declaration");

            migrationBuilder.DropTable(
                name: "review_module");

            migrationBuilder.DropTable(
                name: "role_user");

            migrationBuilder.DropTable(
                name: "student_field_of_study");

            migrationBuilder.DropTable(
                name: "area_of_interest");

            migrationBuilder.DropTable(
                name: "review");

            migrationBuilder.DropTable(
                name: "role");

            migrationBuilder.DropTable(
                name: "thesis");

            migrationBuilder.DropTable(
                name: "topic");

            migrationBuilder.DropTable(
                name: "field_of_study");

            migrationBuilder.DropTable(
                name: "user");
        }
    }
}
