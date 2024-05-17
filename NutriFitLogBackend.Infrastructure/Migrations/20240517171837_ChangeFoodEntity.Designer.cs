﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using NutriFitLogBackend.Infrastructure.Database;

#nullable disable

namespace NutriFitLogBackend.Infrastructure.Migrations
{
    [DbContext(typeof(NutriFitLogContext))]
    [Migration("20240517171837_ChangeFoodEntity")]
    partial class ChangeFoodEntity
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.17")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("NutriFitLogBackend.Domain.Entities.Nutrition.DayPart", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("character varying(30)");

                    b.HasKey("Id");

                    b.ToTable("DayParts");
                });

            modelBuilder.Entity("NutriFitLogBackend.Domain.Entities.Nutrition.Food", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<string>("PictureUrl")
                        .IsRequired()
                        .HasMaxLength(2048)
                        .HasColumnType("character varying(2048)");

                    b.Property<int>("Unit")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("Foods");
                });

            modelBuilder.Entity("NutriFitLogBackend.Domain.Entities.Nutrition.Meal", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<long>("UserId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Meals");
                });

            modelBuilder.Entity("NutriFitLogBackend.Domain.Entities.Nutrition.MealFood", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<double?>("Calories")
                        .HasColumnType("double precision");

                    b.Property<double?>("Carbohydrates")
                        .HasColumnType("double precision");

                    b.Property<long>("DayPartId")
                        .HasColumnType("bigint");

                    b.Property<double?>("Fats")
                        .HasColumnType("double precision");

                    b.Property<long>("FoodId")
                        .HasColumnType("bigint");

                    b.Property<long>("MealId")
                        .HasColumnType("bigint");

                    b.Property<double?>("Protein")
                        .HasColumnType("double precision");

                    b.Property<double>("Quantity")
                        .HasColumnType("double precision");

                    b.HasKey("Id");

                    b.HasIndex("DayPartId");

                    b.HasIndex("FoodId");

                    b.HasIndex("MealId");

                    b.ToTable("MealFoods");
                });

            modelBuilder.Entity("NutriFitLogBackend.Domain.Entities.Trainings.Exercise", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<string>("PictureUrl")
                        .IsRequired()
                        .HasMaxLength(2048)
                        .HasColumnType("character varying(2048)");

                    b.Property<int>("Type")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("Exercises");
                });

            modelBuilder.Entity("NutriFitLogBackend.Domain.Entities.Trainings.Set", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<double?>("Distance")
                        .HasColumnType("double precision");

                    b.Property<double?>("Duration")
                        .HasColumnType("double precision");

                    b.Property<long>("ExerciseId")
                        .HasColumnType("bigint");

                    b.Property<long?>("Repetitions")
                        .HasColumnType("bigint");

                    b.Property<long>("TrainingId")
                        .HasColumnType("bigint");

                    b.Property<double?>("Weight")
                        .HasColumnType("double precision");

                    b.HasKey("Id");

                    b.HasIndex("TrainingId", "ExerciseId");

                    b.ToTable("Sets");
                });

            modelBuilder.Entity("NutriFitLogBackend.Domain.Entities.Trainings.Training", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<long>("UserId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Trainings");
                });

            modelBuilder.Entity("NutriFitLogBackend.Domain.Entities.Trainings.TrainingExercise", b =>
                {
                    b.Property<long>("TrainingId")
                        .HasColumnType("bigint");

                    b.Property<long>("ExerciseId")
                        .HasColumnType("bigint");

                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.HasKey("TrainingId", "ExerciseId");

                    b.HasIndex("ExerciseId");

                    b.HasIndex("Id")
                        .IsUnique();

                    b.ToTable("TrainingExercise");
                });

            modelBuilder.Entity("NutriFitLogBackend.Domain.Entities.Users.StudentTrainer", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<bool>("IsWorking")
                        .HasColumnType("boolean");

                    b.Property<long>("StudentId")
                        .HasColumnType("bigint");

                    b.Property<long>("TrainerId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("StudentId");

                    b.HasIndex("TrainerId", "StudentId")
                        .IsUnique();

                    b.ToTable("StudentTrainer");
                });

            modelBuilder.Entity("NutriFitLogBackend.Domain.Entities.Users.User", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("IsActiveTrainer")
                        .HasColumnType("boolean");

                    b.Property<string>("Roles")
                        .IsRequired()
                        .HasColumnType("json");

                    b.Property<long>("TelegramId")
                        .HasColumnType("bigint");

                    b.Property<DateTime?>("UpdatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("TelegramId")
                        .IsUnique();

                    b.ToTable("Users");
                });

            modelBuilder.Entity("NutriFitLogBackend.Domain.Entities.Nutrition.Meal", b =>
                {
                    b.HasOne("NutriFitLogBackend.Domain.Entities.Users.User", "User")
                        .WithMany("Meals")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("NutriFitLogBackend.Domain.Entities.Nutrition.MealFood", b =>
                {
                    b.HasOne("NutriFitLogBackend.Domain.Entities.Nutrition.DayPart", "DayPart")
                        .WithMany("Meals")
                        .HasForeignKey("DayPartId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("NutriFitLogBackend.Domain.Entities.Nutrition.Food", "Food")
                        .WithMany("Meals")
                        .HasForeignKey("FoodId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("NutriFitLogBackend.Domain.Entities.Nutrition.Meal", "Meal")
                        .WithMany("Foods")
                        .HasForeignKey("MealId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("DayPart");

                    b.Navigation("Food");

                    b.Navigation("Meal");
                });

            modelBuilder.Entity("NutriFitLogBackend.Domain.Entities.Trainings.Set", b =>
                {
                    b.HasOne("NutriFitLogBackend.Domain.Entities.Trainings.TrainingExercise", "TrainingExercise")
                        .WithMany("Sets")
                        .HasForeignKey("TrainingId", "ExerciseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("TrainingExercise");
                });

            modelBuilder.Entity("NutriFitLogBackend.Domain.Entities.Trainings.Training", b =>
                {
                    b.HasOne("NutriFitLogBackend.Domain.Entities.Users.User", "User")
                        .WithMany("Trainings")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("NutriFitLogBackend.Domain.Entities.Trainings.TrainingExercise", b =>
                {
                    b.HasOne("NutriFitLogBackend.Domain.Entities.Trainings.Exercise", "Exercise")
                        .WithMany("Trainings")
                        .HasForeignKey("ExerciseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("NutriFitLogBackend.Domain.Entities.Trainings.Training", "Training")
                        .WithMany("Exercises")
                        .HasForeignKey("TrainingId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Exercise");

                    b.Navigation("Training");
                });

            modelBuilder.Entity("NutriFitLogBackend.Domain.Entities.Users.StudentTrainer", b =>
                {
                    b.HasOne("NutriFitLogBackend.Domain.Entities.Users.User", "Student")
                        .WithMany("Trainers")
                        .HasForeignKey("StudentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("NutriFitLogBackend.Domain.Entities.Users.User", "Trainer")
                        .WithMany("Students")
                        .HasForeignKey("TrainerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Student");

                    b.Navigation("Trainer");
                });

            modelBuilder.Entity("NutriFitLogBackend.Domain.Entities.Nutrition.DayPart", b =>
                {
                    b.Navigation("Meals");
                });

            modelBuilder.Entity("NutriFitLogBackend.Domain.Entities.Nutrition.Food", b =>
                {
                    b.Navigation("Meals");
                });

            modelBuilder.Entity("NutriFitLogBackend.Domain.Entities.Nutrition.Meal", b =>
                {
                    b.Navigation("Foods");
                });

            modelBuilder.Entity("NutriFitLogBackend.Domain.Entities.Trainings.Exercise", b =>
                {
                    b.Navigation("Trainings");
                });

            modelBuilder.Entity("NutriFitLogBackend.Domain.Entities.Trainings.Training", b =>
                {
                    b.Navigation("Exercises");
                });

            modelBuilder.Entity("NutriFitLogBackend.Domain.Entities.Trainings.TrainingExercise", b =>
                {
                    b.Navigation("Sets");
                });

            modelBuilder.Entity("NutriFitLogBackend.Domain.Entities.Users.User", b =>
                {
                    b.Navigation("Meals");

                    b.Navigation("Students");

                    b.Navigation("Trainers");

                    b.Navigation("Trainings");
                });
#pragma warning restore 612, 618
        }
    }
}
