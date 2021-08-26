﻿// <auto-generated />
using System;
using AnomalyDetection.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace AnomalyDetection.Data.Migrations.Sqlite
{
    [DbContext(typeof(ManagerContext))]
    [Migration("20210826175039_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "5.0.9");

            modelBuilder.Entity("AnomalyDetection.Data.Model.Datasource", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("DatasourceType")
                        .IsRequired()
                        .HasMaxLength(16)
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsAuthenticated")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("TEXT");

                    b.Property<string>("Password")
                        .HasMaxLength(64)
                        .HasColumnType("TEXT");

                    b.Property<string>("Url")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("TEXT");

                    b.Property<string>("Username")
                        .HasMaxLength(64)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Datasources");
                });

            modelBuilder.Entity("AnomalyDetection.Data.Model.Metric", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("DatasourceId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("TEXT");

                    b.Property<string>("Query")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("TrainingSchedule")
                        .IsRequired()
                        .HasMaxLength(16)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("DatasourceId");

                    b.ToTable("Metrics");
                });

            modelBuilder.Entity("AnomalyDetection.Data.Model.TrainingJob", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("CreationTime")
                        .HasColumnType("TEXT");

                    b.Property<int?>("MetricId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasMaxLength(16)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("MetricId");

                    b.ToTable("TrainingJobs");
                });

            modelBuilder.Entity("AnomalyDetection.Data.Model.Metric", b =>
                {
                    b.HasOne("AnomalyDetection.Data.Model.Datasource", "Datasource")
                        .WithMany()
                        .HasForeignKey("DatasourceId");

                    b.Navigation("Datasource");
                });

            modelBuilder.Entity("AnomalyDetection.Data.Model.TrainingJob", b =>
                {
                    b.HasOne("AnomalyDetection.Data.Model.Metric", "Metric")
                        .WithMany()
                        .HasForeignKey("MetricId");

                    b.Navigation("Metric");
                });
#pragma warning restore 612, 618
        }
    }
}
