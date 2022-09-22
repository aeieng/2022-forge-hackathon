﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Backend.Migrations
{
    [DbContext(typeof(BackendDbContext))]
    partial class BackendDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.9")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Backend.Entities.Activity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("ActivityType")
                        .IsRequired()
                        .HasColumnType("varchar(32)");

                    b.Property<string>("AppBundleId")
                        .HasColumnType("text");

                    b.Property<string>("ModelType")
                        .IsRequired()
                        .HasColumnType("varchar(32)");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Activities");
                });

            modelBuilder.Entity("Backend.Entities.Building", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<double>("FloorArea")
                        .HasColumnType("double precision");

                    b.Property<double>("Height")
                        .HasColumnType("double precision");

                    b.Property<double?>("Latitude")
                        .HasColumnType("double precision");

                    b.Property<double?>("Longitude")
                        .HasColumnType("double precision");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("NumberOfFloors")
                        .HasColumnType("integer");

                    b.Property<string>("PrimaryBuildingType")
                        .HasColumnType("text");

                    b.Property<string>("ProjectNumber")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Buildings");
                });

            modelBuilder.Entity("Backend.Entities.BuildingCost", b =>
                {
                    b.Property<Guid>("BuildingId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<double>("ArchitecturalCost")
                        .HasColumnType("double precision");

                    b.Property<double>("ElectricalCost")
                        .HasColumnType("double precision");

                    b.Property<double>("MechanicalCost")
                        .HasColumnType("double precision");

                    b.Property<double>("PipingCost")
                        .HasColumnType("double precision");

                    b.Property<double>("StructuralCost")
                        .HasColumnType("double precision");

                    b.HasKey("BuildingId");

                    b.ToTable("BuildingCosts");
                });

            modelBuilder.Entity("Backend.Entities.BuildingEmbodiedCarbon", b =>
                {
                    b.Property<Guid>("BuildingId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<double>("TotalEmbodiedCarbonUseIntensity")
                        .HasColumnType("double precision");

                    b.HasKey("BuildingId");

                    b.ToTable("BuildingEmbodiedCarbons");
                });

            modelBuilder.Entity("Backend.Entities.BuildingOperationalCarbon", b =>
                {
                    b.Property<Guid>("BuildingId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<double?>("ElectricityCarbonIntensity")
                        .HasColumnType("double precision");

                    b.Property<double?>("ElectricityEnergySourcePercentage")
                        .HasColumnType("double precision");

                    b.Property<double?>("NaturalGasCarbonIntensity")
                        .HasColumnType("double precision");

                    b.Property<double?>("NaturalGasEnergySourcePercentage")
                        .HasColumnType("double precision");

                    b.Property<double?>("OperationalEnergyUseIntensity")
                        .HasColumnType("double precision");

                    b.Property<double?>("OtherEnergySourceCarbonIntensity")
                        .HasColumnType("double precision");

                    b.Property<double?>("OtherEnergySourcePercentage")
                        .HasColumnType("double precision");

                    b.Property<double?>("TotalOperatingCarbonUseIntensity")
                        .HasColumnType("double precision");

                    b.HasKey("BuildingId");

                    b.ToTable("BuildingOperationalCarbons");
                });

            modelBuilder.Entity("Backend.Entities.BuildingRoomType", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<double?>("Area")
                        .HasColumnType("double precision");

                    b.Property<Guid>("BuildingId")
                        .HasColumnType("uuid");

                    b.Property<double>("Percentage")
                        .HasColumnType("double precision");

                    b.Property<Guid>("RoomTypeId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.ToTable("BuildingRoomTypes");
                });

            modelBuilder.Entity("Backend.Entities.ExtractionLog", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("DesignAutomationLog")
                        .HasColumnType("text");

                    b.Property<string>("DesignAutomationWorkItemId")
                        .HasColumnType("text");

                    b.Property<Guid>("ModelId")
                        .HasColumnType("uuid");

                    b.Property<string>("Operation")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("ResultSignedUrl")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("StartedRunAtUtc")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("ExtractionLog");
                });

            modelBuilder.Entity("Backend.Entities.Material", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<double>("AchievableEpd")
                        .HasColumnType("double precision");

                    b.Property<double>("BaselineEpd")
                        .HasColumnType("double precision");

                    b.Property<Guid>("BuildingId")
                        .HasColumnType("uuid");

                    b.Property<string>("Category")
                        .IsRequired()
                        .HasColumnType("varchar(32)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<double>("Quantity")
                        .HasColumnType("double precision");

                    b.Property<double>("RealizedEpd")
                        .HasColumnType("double precision");

                    b.Property<string>("SubCategory")
                        .IsRequired()
                        .HasColumnType("varchar(32)");

                    b.Property<string>("Unit")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Materials");
                });

            modelBuilder.Entity("Backend.Entities.Model", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("AutodeskHubId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("AutodeskItemId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("AutodeskProjectId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("BuildingId")
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("RevitVersion")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("varchar(32)");

                    b.HasKey("Id");

                    b.HasIndex("BuildingId");

                    b.ToTable("Models");
                });

            modelBuilder.Entity("Backend.Entities.Room", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<int>("ElementId")
                        .HasColumnType("integer");

                    b.Property<double>("ExteriorWallArea")
                        .HasColumnType("double precision");

                    b.Property<double>("ExteriorWindowArea")
                        .HasColumnType("double precision");

                    b.Property<double>("FloorArea")
                        .HasColumnType("double precision");

                    b.Property<Guid>("ModelId")
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Number")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("ModelId");

                    b.ToTable("Rooms");
                });

            modelBuilder.Entity("Backend.Entities.RoomType", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<double>("EquipmentPowerDensity")
                        .HasColumnType("double precision");

                    b.Property<double>("LightingPowerDensity")
                        .HasColumnType("double precision");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<double>("PeopleDensity")
                        .HasColumnType("double precision");

                    b.Property<double>("PeopleLatentRate")
                        .HasColumnType("double precision");

                    b.Property<double>("PeopleSensibleRate")
                        .HasColumnType("double precision");

                    b.Property<double>("VentilationRate")
                        .HasColumnType("double precision");

                    b.HasKey("Id");

                    b.ToTable("RoomTypes");
                });

            modelBuilder.Entity("Backend.Entities.SelectedActivity", b =>
                {
                    b.Property<Guid>("ActivityId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.HasKey("ActivityId");

                    b.ToTable("SelectedActivities");
                });

            modelBuilder.Entity("Backend.Entities.Model", b =>
                {
                    b.HasOne("Backend.Entities.Building", "Building")
                        .WithMany()
                        .HasForeignKey("BuildingId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.OwnsOne("Backend.Entities.ModelData", "ModelData", b1 =>
                        {
                            b1.Property<Guid>("ModelId")
                                .HasColumnType("uuid");

                            b1.Property<double>("DuctSurfaceArea")
                                .HasColumnType("double precision");

                            b1.Property<double>("ExteriorWallArea")
                                .HasColumnType("double precision");

                            b1.Property<double>("GlazingArea")
                                .HasColumnType("double precision");

                            b1.Property<int>("NumberOfCircuits")
                                .HasColumnType("integer");

                            b1.Property<int>("NumberOfLightingFixtures")
                                .HasColumnType("integer");

                            b1.Property<double>("TotalPipeLength")
                                .HasColumnType("double precision");

                            b1.HasKey("ModelId");

                            b1.ToTable("Models");

                            b1.WithOwner()
                                .HasForeignKey("ModelId");
                        });

                    b.Navigation("Building");

                    b.Navigation("ModelData")
                        .IsRequired();
                });

            modelBuilder.Entity("Backend.Entities.Room", b =>
                {
                    b.HasOne("Backend.Entities.Model", "Model")
                        .WithMany("Rooms")
                        .HasForeignKey("ModelId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Model");
                });

            modelBuilder.Entity("Backend.Entities.Model", b =>
                {
                    b.Navigation("Rooms");
                });
#pragma warning restore 612, 618
        }
    }
}
