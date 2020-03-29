﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WebApi.Data;

namespace WebApi.Migrations
{
    [DbContext(typeof(VersesDbContext))]
    partial class VersesDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("WebApi.Data.Models.Group", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTimeOffset?>("CreatedDate")
                        .HasColumnType("datetimeoffset");

                    b.Property<DateTimeOffset?>("LastModifiedDate")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(255)")
                        .HasMaxLength(255);

                    b.Property<bool>("Public")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.ToTable("Groups");
                });

            modelBuilder.Entity("WebApi.Data.Models.GroupRole", b =>
                {
                    b.Property<int>("UserSettingsId")
                        .HasColumnType("int");

                    b.Property<int>("GroupId")
                        .HasColumnType("int");

                    b.Property<int>("Role")
                        .HasColumnType("int");

                    b.HasKey("UserSettingsId", "GroupId");

                    b.HasIndex("GroupId");

                    b.ToTable("GroupRoles");
                });

            modelBuilder.Entity("WebApi.Data.Models.PassageReference", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Book")
                        .HasColumnType("int");

                    b.Property<byte>("Chapter")
                        .HasColumnType("tinyint");

                    b.Property<DateTimeOffset?>("CreatedDate")
                        .HasColumnType("datetimeoffset");

                    b.Property<byte>("EndOffset")
                        .HasColumnType("tinyint");

                    b.Property<int?>("GroupId")
                        .HasColumnType("int");

                    b.Property<DateTimeOffset?>("LastModifiedDate")
                        .HasColumnType("datetimeoffset");

                    b.Property<byte>("Length")
                        .HasColumnType("tinyint");

                    b.Property<byte>("StartOffset")
                        .HasColumnType("tinyint");

                    b.Property<byte>("Verse")
                        .HasColumnType("tinyint");

                    b.HasKey("Id");

                    b.HasIndex("GroupId");

                    b.ToTable("PassageReferences");
                });

            modelBuilder.Entity("WebApi.Data.Models.UserSettings", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTimeOffset?>("CreatedDate")
                        .HasColumnType("datetimeoffset");

                    b.Property<bool>("IsGlobalAdmin")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LastModifiedDate")
                        .HasColumnType("datetimeoffset");

                    b.Property<int?>("PreferredTranslation")
                        .HasColumnType("int");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.HasKey("Id");

                    b.ToTable("UserSettings");
                });

            modelBuilder.Entity("WebApi.Data.Models.Verse", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Book")
                        .HasColumnType("int");

                    b.Property<byte>("Chapter")
                        .HasColumnType("tinyint");

                    b.Property<DateTimeOffset?>("CreatedDate")
                        .HasColumnType("datetimeoffset");

                    b.Property<DateTimeOffset?>("LastModifiedDate")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("nvarchar(255)")
                        .HasMaxLength(255);

                    b.Property<int>("Translation")
                        .HasColumnType("int");

                    b.Property<byte>("VerseNumber")
                        .HasColumnType("tinyint");

                    b.HasKey("Id");

                    b.ToTable("Verses");
                });

            modelBuilder.Entity("WebApi.Data.Models.GroupRole", b =>
                {
                    b.HasOne("WebApi.Data.Models.Group", "Group")
                        .WithMany("GroupRoles")
                        .HasForeignKey("GroupId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WebApi.Data.Models.UserSettings", "UserSettings")
                        .WithMany("GroupRoles")
                        .HasForeignKey("UserSettingsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("WebApi.Data.Models.PassageReference", b =>
                {
                    b.HasOne("WebApi.Data.Models.Group", null)
                        .WithMany("PassageReferences")
                        .HasForeignKey("GroupId");
                });
#pragma warning restore 612, 618
        }
    }
}
