﻿// <auto-generated />
using System;
using GO.DataAccess.MsSql;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace GO.DataAccess.MsSql.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.10")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("GO.Domain.Entities.Budgets.Budget", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("CreatedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTimeOffset>("CreatedDate")
                        .HasColumnType("datetimeoffset");

                    b.Property<bool>("IsArchived")
                        .HasColumnType("bit");

                    b.Property<byte>("Payday")
                        .HasColumnType("tinyint");

                    b.Property<Guid?>("UpdatedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTimeOffset?>("UpdatedDate")
                        .HasColumnType("datetimeoffset");

                    b.HasKey("Id");

                    b.ToTable("Budget");

                    b.HasData(
                        new
                        {
                            Id = new Guid("936da01f-9abd-4d9d-80c7-02af85c822a8"),
                            CreatedBy = new Guid("936da01f-9abd-4d9d-80c7-02af85c822a8"),
                            CreatedDate = new DateTimeOffset(new DateTime(2021, 10, 14, 22, 11, 29, 713, DateTimeKind.Unspecified).AddTicks(5663), new TimeSpan(0, 0, 0, 0, 0)),
                            IsArchived = false,
                            Payday = (byte)0
                        });
                });

            modelBuilder.Entity("GO.Domain.Entities.Budgets.BudgetRecord", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<long>("Amount")
                        .HasColumnType("bigint");

                    b.Property<Guid>("BudgetId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("CategoryType")
                        .HasColumnType("int");

                    b.Property<Guid>("CreatedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTimeOffset>("CreatedDate")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("Description")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<Guid?>("UpdatedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTimeOffset?>("UpdatedDate")
                        .HasColumnType("datetimeoffset");

                    b.HasKey("Id");

                    b.HasIndex("BudgetId");

                    b.ToTable("BudgetRecord");
                });

            modelBuilder.Entity("GO.Domain.Entities.Budgets.BudgetsUsersRelation", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("BudgetId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("IsDisabled")
                        .HasColumnType("bit");

                    b.HasKey("UserId", "BudgetId");

                    b.HasIndex("BudgetId");

                    b.ToTable("BudgetsUsers");

                    b.HasData(
                        new
                        {
                            UserId = new Guid("936da01f-9abd-4d9d-80c7-02af85c822a8"),
                            BudgetId = new Guid("936da01f-9abd-4d9d-80c7-02af85c822a8"),
                            IsDisabled = false
                        });
                });

            modelBuilder.Entity("GO.Domain.Entities.Users.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("CreatedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTimeOffset>("CreatedDate")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<bool>("IsArchived")
                        .HasColumnType("bit");

                    b.Property<bool>("IsLocked")
                        .HasColumnType("bit");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<byte>("Scopes")
                        .HasColumnType("tinyint");

                    b.Property<Guid?>("UpdatedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTimeOffset?>("UpdatedDate")
                        .HasColumnType("datetimeoffset");

                    b.HasKey("Id");

                    b.ToTable("User");

                    b.HasData(
                        new
                        {
                            Id = new Guid("936da01f-9abd-4d9d-80c7-02af85c822a8"),
                            CreatedBy = new Guid("936da01f-9abd-4d9d-80c7-02af85c822a8"),
                            CreatedDate = new DateTimeOffset(new DateTime(2021, 10, 14, 22, 11, 29, 709, DateTimeKind.Unspecified).AddTicks(2007), new TimeSpan(0, 0, 0, 0, 0)),
                            FirstName = "Dmytro",
                            IsArchived = false,
                            IsLocked = false,
                            LastName = "😇",
                            Scopes = (byte)14
                        });
                });

            modelBuilder.Entity("GO.Domain.Entities.Users.UserConnection", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<long>("ConnectionId")
                        .HasMaxLength(50)
                        .HasColumnType("bigint");

                    b.Property<Guid>("CreatedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTimeOffset>("CreatedDate")
                        .HasColumnType("datetimeoffset");

                    b.Property<byte>("CurrentScope")
                        .HasColumnType("tinyint");

                    b.Property<string>("NickName")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.Property<Guid?>("UpdatedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTimeOffset?>("UpdatedDate")
                        .HasColumnType("datetimeoffset");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("ConnectionId")
                        .IsUnique();

                    b.HasIndex("UserId");

                    b.ToTable("UserConnection");

                    b.HasData(
                        new
                        {
                            Id = new Guid("936da01f-9abd-4d9d-80c7-02af85c822a8"),
                            ConnectionId = 428296956L,
                            CreatedBy = new Guid("936da01f-9abd-4d9d-80c7-02af85c822a8"),
                            CreatedDate = new DateTimeOffset(new DateTime(2021, 10, 14, 22, 11, 29, 712, DateTimeKind.Unspecified).AddTicks(5765), new TimeSpan(0, 0, 0, 0, 0)),
                            CurrentScope = (byte)8,
                            NickName = "Nimb84",
                            Type = 1,
                            UserId = new Guid("936da01f-9abd-4d9d-80c7-02af85c822a8")
                        });
                });

            modelBuilder.Entity("GO.Domain.Entities.Budgets.BudgetRecord", b =>
                {
                    b.HasOne("GO.Domain.Entities.Budgets.Budget", "Budget")
                        .WithMany("Records")
                        .HasForeignKey("BudgetId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Budget");
                });

            modelBuilder.Entity("GO.Domain.Entities.Budgets.BudgetsUsersRelation", b =>
                {
                    b.HasOne("GO.Domain.Entities.Budgets.Budget", "Budget")
                        .WithMany("Users")
                        .HasForeignKey("BudgetId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("GO.Domain.Entities.Users.User", "User")
                        .WithMany("Budgets")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Budget");

                    b.Navigation("User");
                });

            modelBuilder.Entity("GO.Domain.Entities.Users.UserConnection", b =>
                {
                    b.HasOne("GO.Domain.Entities.Users.User", "User")
                        .WithMany("Connections")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("GO.Domain.Entities.Budgets.Budget", b =>
                {
                    b.Navigation("Records");

                    b.Navigation("Users");
                });

            modelBuilder.Entity("GO.Domain.Entities.Users.User", b =>
                {
                    b.Navigation("Budgets");

                    b.Navigation("Connections");
                });
#pragma warning restore 612, 618
        }
    }
}
