﻿// <auto-generated />
using System;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infrastructure.Migrations
{
    [DbContext(typeof(DiplomaDbContext))]
    partial class DiplomaDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("AreaOfInterestUser", b =>
                {
                    b.Property<long>("AreasOfInterestId")
                        .HasColumnType("bigint")
                        .HasColumnName("areas_of_interest_id");

                    b.Property<long>("UsersId")
                        .HasColumnType("bigint")
                        .HasColumnName("users_id");

                    b.HasKey("AreasOfInterestId", "UsersId")
                        .HasName("pk_area_of_interest_user");

                    b.HasIndex("UsersId")
                        .HasDatabaseName("ix_area_of_interest_user_users_id");

                    b.ToTable("area_of_interest_user");
                });

            modelBuilder.Entity("Core.Models.Reviews.Review", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("review_id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<string>("Grade")
                        .HasColumnType("text")
                        .HasColumnName("grade");

                    b.Property<bool>("IsPublished")
                        .HasColumnType("boolean")
                        .HasColumnName("is_published");

                    b.Property<DateTime?>("PublishTimestamp")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("publish_timestamp");

                    b.Property<long>("ReviewerId")
                        .HasColumnType("bigint")
                        .HasColumnName("reviewer_id");

                    b.Property<long?>("ThesisId")
                        .HasColumnType("bigint")
                        .HasColumnName("thesis_id");

                    b.HasKey("Id")
                        .HasName("pk_review");

                    b.HasIndex("ReviewerId")
                        .HasDatabaseName("ix_review_reviewer_id");

                    b.HasIndex("ThesisId")
                        .HasDatabaseName("ix_review_thesis_id");

                    b.ToTable("review", (string)null);
                });

            modelBuilder.Entity("Core.Models.Reviews.ReviewModule", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("review_module_id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.Property<long?>("ReviewId")
                        .HasColumnType("bigint")
                        .HasColumnName("review_id");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("type");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("value");

                    b.HasKey("Id")
                        .HasName("pk_review_module");

                    b.HasIndex("ReviewId")
                        .HasDatabaseName("ix_review_module_review_id");

                    b.ToTable("review_module", (string)null);
                });

            modelBuilder.Entity("Core.Models.Theses.Declaration", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("declaration_id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<DateOnly>("Date")
                        .HasColumnType("date")
                        .HasColumnName("date");

                    b.Property<string>("Language")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("language");

                    b.Property<string>("ObjectiveOfWork")
                        .IsRequired()
                        .HasMaxLength(4000)
                        .HasColumnType("character varying(4000)")
                        .HasColumnName("objective_of_work");

                    b.Property<string>("OperatingRange")
                        .IsRequired()
                        .HasMaxLength(4000)
                        .HasColumnType("character varying(4000)")
                        .HasColumnName("operating_range");

                    b.Property<long?>("ThesisId")
                        .HasColumnType("bigint")
                        .HasColumnName("thesis_id");

                    b.HasKey("Id")
                        .HasName("pk_declaration");

                    b.HasIndex("ThesisId")
                        .HasDatabaseName("ix_declaration_thesis_id");

                    b.ToTable("declaration", (string)null);
                });

            modelBuilder.Entity("Core.Models.Theses.Thesis", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("thesis_id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<string>("CloudBucket")
                        .HasColumnType("text")
                        .HasColumnName("cloud_bucket");

                    b.Property<string>("CloudKey")
                        .HasColumnType("text")
                        .HasColumnName("cloud_key");

                    b.Property<byte[]>("Content")
                        .HasColumnType("bytea")
                        .HasColumnName("content");

                    b.Property<string>("FileFormat")
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("file_format");

                    b.Property<bool?>("HasConsentToChangeLanguage")
                        .HasColumnType("boolean")
                        .HasColumnName("has_consent_to_change_language");

                    b.Property<string>("Language")
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("language");

                    b.Property<long>("RealizerStudentId")
                        .HasColumnType("bigint")
                        .HasColumnName("realizer_student_id");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("status");

                    b.Property<long>("TopicId")
                        .HasColumnType("bigint")
                        .HasColumnName("topic_id");

                    b.HasKey("Id")
                        .HasName("pk_thesis");

                    b.HasIndex("RealizerStudentId")
                        .HasDatabaseName("ix_thesis_realizer_student_id");

                    b.HasIndex("TopicId")
                        .HasDatabaseName("ix_thesis_topic_id");

                    b.ToTable("thesis", (string)null);
                });

            modelBuilder.Entity("Core.Models.Topics.Application", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("application_id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<bool>("IsTopicProposal")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(false)
                        .HasColumnName("is_topic_proposal");

                    b.Property<string>("Message")
                        .IsRequired()
                        .HasMaxLength(4000)
                        .HasColumnType("character varying(4000)")
                        .HasColumnName("message");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("status");

                    b.Property<long>("SubmitterId")
                        .HasColumnType("bigint")
                        .HasColumnName("submitter_id");

                    b.Property<DateTime>("Timestamp")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("timestamp");

                    b.Property<long>("TopicId")
                        .HasColumnType("bigint")
                        .HasColumnName("topic_id");

                    b.HasKey("Id")
                        .HasName("pk_application");

                    b.HasIndex("SubmitterId")
                        .HasDatabaseName("ix_application_submitter_id");

                    b.HasIndex("TopicId")
                        .HasDatabaseName("ix_application_topic_id");

                    b.ToTable("application", (string)null);
                });

            modelBuilder.Entity("Core.Models.Topics.FieldOfStudy", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("field_of_study_id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<int>("Degree")
                        .HasColumnType("integer")
                        .HasColumnName("degree");

                    b.Property<int>("HoursForThesis")
                        .HasColumnType("integer")
                        .HasColumnName("hours_for_thesis");

                    b.Property<string>("LectureLanguage")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("lecture_language");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("name");

                    b.Property<string>("StudyForm")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("study_form");

                    b.HasKey("Id")
                        .HasName("pk_field_of_study");

                    b.ToTable("field_of_study", (string)null);
                });

            modelBuilder.Entity("Core.Models.Topics.Topic", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("topic_id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<string>("EnglishName")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("english_name");

                    b.Property<long>("FieldOfStudyId")
                        .HasColumnType("bigint")
                        .HasColumnName("field_of_study_id");

                    b.Property<bool?>("IsAccepted")
                        .HasColumnType("boolean")
                        .HasColumnName("is_accepted");

                    b.Property<bool>("IsFree")
                        .HasColumnType("boolean")
                        .HasColumnName("is_free");

                    b.Property<bool>("IsProposedByStudent")
                        .HasColumnType("boolean")
                        .HasColumnName("is_proposed_by_student");

                    b.Property<int>("MaxRealizationNumber")
                        .HasColumnType("integer")
                        .HasColumnName("max_realization_number");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("name");

                    b.Property<long>("ProposerId")
                        .HasColumnType("bigint")
                        .HasColumnName("proposer_id");

                    b.Property<long>("SupervisorId")
                        .HasColumnType("bigint")
                        .HasColumnName("supervisor_id");

                    b.Property<string>("YearOfDefence")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("year_of_defence");

                    b.HasKey("Id")
                        .HasName("pk_topic");

                    b.HasIndex("FieldOfStudyId")
                        .HasDatabaseName("ix_topic_field_of_study_id");

                    b.HasIndex("ProposerId")
                        .HasDatabaseName("ix_topic_proposer_id");

                    b.HasIndex("SupervisorId")
                        .HasDatabaseName("ix_topic_supervisor_id");

                    b.ToTable("topic", (string)null);
                });

            modelBuilder.Entity("Core.Models.Users.AreaOfInterest", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("area_of_interest_id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("name");

                    b.HasKey("Id")
                        .HasName("pk_area_of_interest");

                    b.ToTable("area_of_interest", (string)null);
                });

            modelBuilder.Entity("Core.Models.Users.StudentFieldOfStudy", b =>
                {
                    b.Property<long>("StudentId")
                        .HasColumnType("bigint")
                        .HasColumnName("student_id");

                    b.Property<long>("FieldOfStudyId")
                        .HasColumnType("bigint")
                        .HasColumnName("field_of_study_id");

                    b.Property<string>("PlannedYearOfDefence")
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("planned_year_of_defence");

                    b.Property<string>("Semester")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("semester");

                    b.Property<string>("Specialization")
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("specialization");

                    b.HasKey("StudentId", "FieldOfStudyId")
                        .HasName("pk_student_field_of_study");

                    b.HasIndex("FieldOfStudyId")
                        .HasDatabaseName("ix_student_field_of_study_field_of_study_id");

                    b.ToTable("student_field_of_study", (string)null);
                });

            modelBuilder.Entity("Core.Models.Users.User", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("user_id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("discriminator");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("email");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("first_name");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("last_name");

                    b.HasKey("Id")
                        .HasName("pk_user");

                    b.HasIndex("Email")
                        .IsUnique()
                        .HasDatabaseName("ix_user_email");

                    b.ToTable("user", (string)null);

                    b.HasDiscriminator<string>("Discriminator").HasValue("User");
                });

            modelBuilder.Entity("Core.Models.Users.Student", b =>
                {
                    b.HasBaseType("Core.Models.Users.User");

                    b.Property<int>("IndexNumber")
                        .HasColumnType("integer")
                        .HasColumnName("index_number");

                    b.HasIndex("IndexNumber")
                        .HasDatabaseName("ix_user_index_number");

                    b.ToTable("user");

                    b.HasDiscriminator().HasValue("Student");
                });

            modelBuilder.Entity("Core.Models.Users.Tutor", b =>
                {
                    b.HasBaseType("Core.Models.Users.User");

                    b.Property<string>("AcademicDegree")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("academic_degree");

                    b.Property<string>("Department")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("department");

                    b.Property<bool>("HasConsentToExtendPensum")
                        .HasColumnType("boolean")
                        .HasColumnName("has_consent_to_extend_pensum");

                    b.Property<bool>("IsPositiveFacultyOpinion")
                        .HasColumnType("boolean")
                        .HasColumnName("is_positive_faculty_opinion");

                    b.Property<int>("Pensum")
                        .HasColumnType("integer")
                        .HasColumnName("pensum");

                    b.Property<string>("Position")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("position");

                    b.ToTable("user");

                    b.HasDiscriminator().HasValue("Tutor");
                });

            modelBuilder.Entity("AreaOfInterestUser", b =>
                {
                    b.HasOne("Core.Models.Users.AreaOfInterest", null)
                        .WithMany()
                        .HasForeignKey("AreasOfInterestId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_area_of_interest_user_area_of_interest_areas_of_interest_id");

                    b.HasOne("Core.Models.Users.User", null)
                        .WithMany()
                        .HasForeignKey("UsersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_area_of_interest_user_user_users_id");
                });

            modelBuilder.Entity("Core.Models.Reviews.Review", b =>
                {
                    b.HasOne("Core.Models.Users.Tutor", "Reviewer")
                        .WithMany()
                        .HasForeignKey("ReviewerId")
                        .OnDelete(DeleteBehavior.SetNull)
                        .IsRequired()
                        .HasConstraintName("fk_review_user_reviewer_id");

                    b.HasOne("Core.Models.Theses.Thesis", null)
                        .WithMany("Reviews")
                        .HasForeignKey("ThesisId")
                        .HasConstraintName("fk_review_thesis_thesis_id");

                    b.Navigation("Reviewer");
                });

            modelBuilder.Entity("Core.Models.Reviews.ReviewModule", b =>
                {
                    b.HasOne("Core.Models.Reviews.Review", null)
                        .WithMany("ReviewModules")
                        .HasForeignKey("ReviewId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .HasConstraintName("fk_review_module_review_review_id");
                });

            modelBuilder.Entity("Core.Models.Theses.Declaration", b =>
                {
                    b.HasOne("Core.Models.Theses.Thesis", null)
                        .WithMany("Declarations")
                        .HasForeignKey("ThesisId")
                        .HasConstraintName("fk_declaration_thesis_thesis_id");
                });

            modelBuilder.Entity("Core.Models.Theses.Thesis", b =>
                {
                    b.HasOne("Core.Models.Users.Student", "RealizerStudent")
                        .WithMany("Theses")
                        .HasForeignKey("RealizerStudentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_thesis_user_realizer_student_id");

                    b.HasOne("Core.Models.Topics.Topic", "Topic")
                        .WithMany("Theses")
                        .HasForeignKey("TopicId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_thesis_topic_topic_id");

                    b.Navigation("RealizerStudent");

                    b.Navigation("Topic");
                });

            modelBuilder.Entity("Core.Models.Topics.Application", b =>
                {
                    b.HasOne("Core.Models.Users.Student", "Submitter")
                        .WithMany("Applications")
                        .HasForeignKey("SubmitterId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_application_user_submitter_id");

                    b.HasOne("Core.Models.Topics.Topic", "Topic")
                        .WithMany("Applications")
                        .HasForeignKey("TopicId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_application_topic_topic_id");

                    b.Navigation("Submitter");

                    b.Navigation("Topic");
                });

            modelBuilder.Entity("Core.Models.Topics.Topic", b =>
                {
                    b.HasOne("Core.Models.Topics.FieldOfStudy", "FieldOfStudy")
                        .WithMany()
                        .HasForeignKey("FieldOfStudyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_topic_field_of_study_field_of_study_id");

                    b.HasOne("Core.Models.Users.User", "Proposer")
                        .WithMany()
                        .HasForeignKey("ProposerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_topic_user_proposer_id");

                    b.HasOne("Core.Models.Users.Tutor", "Supervisor")
                        .WithMany()
                        .HasForeignKey("SupervisorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_topic_user_supervisor_id");

                    b.Navigation("FieldOfStudy");

                    b.Navigation("Proposer");

                    b.Navigation("Supervisor");
                });

            modelBuilder.Entity("Core.Models.Users.StudentFieldOfStudy", b =>
                {
                    b.HasOne("Core.Models.Topics.FieldOfStudy", "FieldOfStudy")
                        .WithMany("StudentFieldsOfStudy")
                        .HasForeignKey("FieldOfStudyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_student_field_of_study_field_of_study_field_of_study_id");

                    b.HasOne("Core.Models.Users.Student", "Student")
                        .WithMany("StudentFieldOfStudies")
                        .HasForeignKey("StudentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_student_field_of_study_user_student_id");

                    b.Navigation("FieldOfStudy");

                    b.Navigation("Student");
                });

            modelBuilder.Entity("Core.Models.Reviews.Review", b =>
                {
                    b.Navigation("ReviewModules");
                });

            modelBuilder.Entity("Core.Models.Theses.Thesis", b =>
                {
                    b.Navigation("Declarations");

                    b.Navigation("Reviews");
                });

            modelBuilder.Entity("Core.Models.Topics.FieldOfStudy", b =>
                {
                    b.Navigation("StudentFieldsOfStudy");
                });

            modelBuilder.Entity("Core.Models.Topics.Topic", b =>
                {
                    b.Navigation("Applications");

                    b.Navigation("Theses");
                });

            modelBuilder.Entity("Core.Models.Users.Student", b =>
                {
                    b.Navigation("Applications");

                    b.Navigation("StudentFieldOfStudies");

                    b.Navigation("Theses");
                });
#pragma warning restore 612, 618
        }
    }
}
