﻿// <auto-generated />
using System;
using EngineeringThesisAPI.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace EngineeringThesisAPI.Migrations
{
    [DbContext(typeof(EngineeringThesisDbContext))]
    [Migration("20231021192933_ChangeFileEntity2")]
    partial class ChangeFileEntity2
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.12")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("EngineeringThesisAPI.Entities.Attraction", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("City")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("CoordinateX")
                        .HasColumnType("float");

                    b.Property<double>("CoordinateY")
                        .HasColumnType("float");

                    b.Property<double>("CoordinateZ")
                        .HasColumnType("float");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<TimeSpan>("Duration")
                        .HasColumnType("time");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<float>("Price")
                        .HasColumnType("real");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Attractions");
                });

            modelBuilder.Entity("EngineeringThesisAPI.Entities.Comment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("AttractionId")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Rating")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("AttractionId");

                    b.HasIndex("UserId");

                    b.ToTable("Comments");
                });

            modelBuilder.Entity("EngineeringThesisAPI.Entities.FilePath", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("AddTime")
                        .HasColumnType("datetime2");

                    b.Property<int?>("AttractionId")
                        .HasColumnType("int");

                    b.Property<string>("FileName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("AttractionId");

                    b.HasIndex("UserId");

                    b.ToTable("FilePaths");
                });

            modelBuilder.Entity("EngineeringThesisAPI.Entities.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Enabled")
                        .HasColumnType("bit");

                    b.Property<string>("NickName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("EngineeringThesisAPI.Entities.Attraction", b =>
                {
                    b.HasOne("EngineeringThesisAPI.Entities.User", "User")
                        .WithMany("Attractions")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("EngineeringThesisAPI.Entities.Comment", b =>
                {
                    b.HasOne("EngineeringThesisAPI.Entities.Attraction", "Attraction")
                        .WithMany("Comments")
                        .HasForeignKey("AttractionId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("EngineeringThesisAPI.Entities.User", "User")
                        .WithMany("Comments")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Attraction");

                    b.Navigation("User");
                });

            modelBuilder.Entity("EngineeringThesisAPI.Entities.FilePath", b =>
                {
                    b.HasOne("EngineeringThesisAPI.Entities.Attraction", "Attraction")
                        .WithMany("Photos")
                        .HasForeignKey("AttractionId");

                    b.HasOne("EngineeringThesisAPI.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId");

                    b.Navigation("Attraction");

                    b.Navigation("User");
                });

            modelBuilder.Entity("EngineeringThesisAPI.Entities.Attraction", b =>
                {
                    b.Navigation("Comments");

                    b.Navigation("Photos");
                });

            modelBuilder.Entity("EngineeringThesisAPI.Entities.User", b =>
                {
                    b.Navigation("Attractions");

                    b.Navigation("Comments");
                });
#pragma warning restore 612, 618
        }
    }
}
