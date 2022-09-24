﻿// <auto-generated />
using System;
using MafiaOnline.DataAccess.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace MafiaOnline.DataAccess.Migrations
{
    [DbContext(typeof(DataContext))]
    partial class DataContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.17")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("MafiaOnline.DataAccess.Entities.Agent", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<long?>("BossId")
                        .HasColumnType("bigint");

                    b.Property<int>("Dexterity")
                        .HasColumnType("int");

                    b.Property<string>("FirstName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Intelligence")
                        .HasColumnType("int");

                    b.Property<string>("LastName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("State")
                        .HasColumnType("int");

                    b.Property<int>("Strength")
                        .HasColumnType("int");

                    b.Property<long>("Upkeep")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("BossId");

                    b.ToTable("Agent");

                    b.HasData(
                        new
                        {
                            Id = 1L,
                            BossId = 1L,
                            Dexterity = 10,
                            FirstName = "Jotaro",
                            Intelligence = 10,
                            LastName = "Kujo",
                            State = 2,
                            Strength = 10,
                            Upkeep = 100L
                        },
                        new
                        {
                            Id = 2L,
                            BossId = 1L,
                            Dexterity = 5,
                            FirstName = "Adam",
                            Intelligence = 5,
                            LastName = "Mickiewicz",
                            State = 2,
                            Strength = 5,
                            Upkeep = 50L
                        },
                        new
                        {
                            Id = 3L,
                            BossId = 2L,
                            Dexterity = 4,
                            FirstName = "Natalia",
                            Intelligence = 3,
                            LastName = "Natsu",
                            State = 2,
                            Strength = 7,
                            Upkeep = 70L
                        },
                        new
                        {
                            Id = 4L,
                            Dexterity = 7,
                            FirstName = "Eleonora",
                            Intelligence = 0,
                            LastName = "Lora",
                            State = 1,
                            Strength = 8,
                            Upkeep = 30L
                        },
                        new
                        {
                            Id = 5L,
                            BossId = 1L,
                            Dexterity = 1,
                            FirstName = "Robert",
                            Intelligence = 5,
                            LastName = "Makłowicz",
                            State = 2,
                            Strength = 3,
                            Upkeep = 200L
                        });
                });

            modelBuilder.Entity("MafiaOnline.DataAccess.Entities.AgentForSale", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<long>("AgentId")
                        .HasColumnType("bigint");

                    b.Property<long>("Price")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("AgentId")
                        .IsUnique();

                    b.ToTable("AgentForSale");

                    b.HasData(
                        new
                        {
                            Id = 1L,
                            AgentId = 4L,
                            Price = 5000L
                        });
                });

            modelBuilder.Entity("MafiaOnline.DataAccess.Entities.Boss", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("FirstName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("LastSeen")
                        .HasColumnType("datetime2");

                    b.Property<long>("Money")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.ToTable("Boss");

                    b.HasData(
                        new
                        {
                            Id = 1L,
                            FirstName = "Patricio",
                            LastName = "Rico",
                            LastSeen = new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Money = 5000L
                        },
                        new
                        {
                            Id = 2L,
                            FirstName = "Rodrigo",
                            LastName = "Margherita",
                            LastSeen = new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Money = 5000L
                        });
                });

            modelBuilder.Entity("MafiaOnline.DataAccess.Entities.Message", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Content")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long?>("FromBossId")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("ReceivedDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("Seen")
                        .HasColumnType("bit");

                    b.Property<string>("Subject")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long?>("ToBossId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("FromBossId");

                    b.HasIndex("ToBossId");

                    b.ToTable("Message");
                });

            modelBuilder.Entity("MafiaOnline.DataAccess.Entities.Mission", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("DexterityPercentage")
                        .HasColumnType("int");

                    b.Property<int>("DifficultyLevel")
                        .HasColumnType("int");

                    b.Property<double>("Duration")
                        .HasColumnType("float");

                    b.Property<int>("IntelligencePercentage")
                        .HasColumnType("int");

                    b.Property<int>("Loot")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("StrengthPercentage")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Mission");

                    b.HasData(
                        new
                        {
                            Id = 1L,
                            DexterityPercentage = 60,
                            DifficultyLevel = 7,
                            Duration = 30.0,
                            IntelligencePercentage = 20,
                            Loot = 5000,
                            Name = "Bank robbery",
                            StrengthPercentage = 20
                        },
                        new
                        {
                            Id = 2L,
                            DexterityPercentage = 20,
                            DifficultyLevel = 9,
                            Duration = 10.0,
                            IntelligencePercentage = 0,
                            Loot = 10000,
                            Name = "Senator assassination",
                            StrengthPercentage = 80
                        },
                        new
                        {
                            Id = 3L,
                            DexterityPercentage = 20,
                            DifficultyLevel = 2,
                            Duration = 10.0,
                            IntelligencePercentage = 20,
                            Loot = 100,
                            Name = "Party",
                            StrengthPercentage = 60
                        },
                        new
                        {
                            Id = 4L,
                            DexterityPercentage = 60,
                            DifficultyLevel = 1,
                            Duration = 5.0,
                            IntelligencePercentage = 20,
                            Loot = 10,
                            Name = "Buy a coffee",
                            StrengthPercentage = 20
                        },
                        new
                        {
                            Id = 5L,
                            DexterityPercentage = 20,
                            DifficultyLevel = 5,
                            Duration = 55.0,
                            IntelligencePercentage = 60,
                            Loot = 1000,
                            Name = "Money laundering",
                            StrengthPercentage = 20
                        },
                        new
                        {
                            Id = 6L,
                            DexterityPercentage = 60,
                            DifficultyLevel = 6,
                            Duration = 3600.0,
                            IntelligencePercentage = 20,
                            Loot = 2000,
                            Name = "Car theft",
                            StrengthPercentage = 20
                        },
                        new
                        {
                            Id = 7L,
                            DexterityPercentage = 20,
                            DifficultyLevel = 8,
                            Duration = 15.0,
                            IntelligencePercentage = 40,
                            Loot = 4000,
                            Name = "Arms trade",
                            StrengthPercentage = 40
                        });
                });

            modelBuilder.Entity("MafiaOnline.DataAccess.Entities.PerformingMission", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<long>("AgentId")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("CompletionTime")
                        .HasColumnType("datetime2");

                    b.Property<long>("MissionId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("AgentId")
                        .IsUnique();

                    b.HasIndex("MissionId")
                        .IsUnique();

                    b.ToTable("PerformingMission");
                });

            modelBuilder.Entity("MafiaOnline.DataAccess.Entities.Player", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<long>("BossId")
                        .HasColumnType("bigint");

                    b.Property<string>("HashedPassword")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Nick")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RefreshToken")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("RefreshTokenExpiryTime")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("BossId")
                        .IsUnique();

                    b.ToTable("Player");

                    b.HasData(
                        new
                        {
                            Id = 1L,
                            BossId = 1L,
                            HashedPassword = "tlnK6HiwFF4+b4DRVaVdRlIPtzduirsf8W3+nbXlLWlf9c/J",
                            Nick = "mafia",
                            RefreshTokenExpiryTime = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified)
                        },
                        new
                        {
                            Id = 2L,
                            BossId = 2L,
                            HashedPassword = "d2JZt0Jz9UzgW1l544W2WnOaX14u/pfGUDYTQzv5AEWk3W7D",
                            Nick = "tomek",
                            RefreshTokenExpiryTime = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified)
                        });
                });

            modelBuilder.Entity("MafiaOnline.DataAccess.Entities.Agent", b =>
                {
                    b.HasOne("MafiaOnline.DataAccess.Entities.Boss", "Boss")
                        .WithMany("Agents")
                        .HasForeignKey("BossId");

                    b.Navigation("Boss");
                });

            modelBuilder.Entity("MafiaOnline.DataAccess.Entities.AgentForSale", b =>
                {
                    b.HasOne("MafiaOnline.DataAccess.Entities.Agent", "Agent")
                        .WithOne("AgentForSale")
                        .HasForeignKey("MafiaOnline.DataAccess.Entities.AgentForSale", "AgentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Agent");
                });

            modelBuilder.Entity("MafiaOnline.DataAccess.Entities.Message", b =>
                {
                    b.HasOne("MafiaOnline.DataAccess.Entities.Boss", "FromBoss")
                        .WithMany("MessageFromBosses")
                        .HasForeignKey("FromBossId");

                    b.HasOne("MafiaOnline.DataAccess.Entities.Boss", "ToBoss")
                        .WithMany("MessageToBosses")
                        .HasForeignKey("ToBossId");

                    b.Navigation("FromBoss");

                    b.Navigation("ToBoss");
                });

            modelBuilder.Entity("MafiaOnline.DataAccess.Entities.PerformingMission", b =>
                {
                    b.HasOne("MafiaOnline.DataAccess.Entities.Agent", "Agent")
                        .WithOne("PerformingMission")
                        .HasForeignKey("MafiaOnline.DataAccess.Entities.PerformingMission", "AgentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MafiaOnline.DataAccess.Entities.Mission", "Mission")
                        .WithOne("PerformingMission")
                        .HasForeignKey("MafiaOnline.DataAccess.Entities.PerformingMission", "MissionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Agent");

                    b.Navigation("Mission");
                });

            modelBuilder.Entity("MafiaOnline.DataAccess.Entities.Player", b =>
                {
                    b.HasOne("MafiaOnline.DataAccess.Entities.Boss", "Boss")
                        .WithOne("Player")
                        .HasForeignKey("MafiaOnline.DataAccess.Entities.Player", "BossId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Boss");
                });

            modelBuilder.Entity("MafiaOnline.DataAccess.Entities.Agent", b =>
                {
                    b.Navigation("AgentForSale");

                    b.Navigation("PerformingMission");
                });

            modelBuilder.Entity("MafiaOnline.DataAccess.Entities.Boss", b =>
                {
                    b.Navigation("Agents");

                    b.Navigation("MessageFromBosses");

                    b.Navigation("MessageToBosses");

                    b.Navigation("Player");
                });

            modelBuilder.Entity("MafiaOnline.DataAccess.Entities.Mission", b =>
                {
                    b.Navigation("PerformingMission");
                });
#pragma warning restore 612, 618
        }
    }
}
