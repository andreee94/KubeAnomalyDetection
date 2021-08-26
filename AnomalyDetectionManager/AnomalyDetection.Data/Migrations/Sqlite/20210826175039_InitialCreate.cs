using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AnomalyDetection.Data.Migrations.Sqlite
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Datasources",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 64, nullable: false),
                    DatasourceType = table.Column<string>(type: "TEXT", maxLength: 16, nullable: false),
                    Url = table.Column<string>(type: "TEXT", maxLength: 128, nullable: false),
                    Username = table.Column<string>(type: "TEXT", maxLength: 64, nullable: true),
                    Password = table.Column<string>(type: "TEXT", maxLength: 64, nullable: true),
                    IsAuthenticated = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Datasources", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Metrics",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 64, nullable: false),
                    Query = table.Column<string>(type: "TEXT", nullable: false),
                    DatasourceId = table.Column<int>(type: "INTEGER", nullable: true),
                    TrainingSchedule = table.Column<string>(type: "TEXT", maxLength: 16, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Metrics", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Metrics_Datasources_DatasourceId",
                        column: x => x.DatasourceId,
                        principalTable: "Datasources",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TrainingJobs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    MetricId = table.Column<int>(type: "INTEGER", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Status = table.Column<string>(type: "TEXT", maxLength: 16, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainingJobs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrainingJobs_Metrics_MetricId",
                        column: x => x.MetricId,
                        principalTable: "Metrics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Metrics_DatasourceId",
                table: "Metrics",
                column: "DatasourceId");

            migrationBuilder.CreateIndex(
                name: "IX_TrainingJobs_MetricId",
                table: "TrainingJobs",
                column: "MetricId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TrainingJobs");

            migrationBuilder.DropTable(
                name: "Metrics");

            migrationBuilder.DropTable(
                name: "Datasources");
        }
    }
}
