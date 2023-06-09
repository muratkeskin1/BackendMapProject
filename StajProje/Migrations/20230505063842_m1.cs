using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace StajProje.Migrations
{
    public partial class m1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AtmDeliveryHistories",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    AtmId = table.Column<int>(type: "integer", nullable: false),
                    Banknot10 = table.Column<int>(type: "integer", nullable: false),
                    Banknot20 = table.Column<int>(type: "integer", nullable: false),
                    Banknot50 = table.Column<int>(type: "integer", nullable: false),
                    Banknot100 = table.Column<int>(type: "integer", nullable: false),
                    Banknot200 = table.Column<int>(type: "integer", nullable: false),
                    Date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AtmDeliveryHistories", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "AtmHistories",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    ATMId = table.Column<int>(type: "integer", nullable: false),
                    Date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Banknot10 = table.Column<int>(type: "integer", nullable: false),
                    Banknot20 = table.Column<int>(type: "integer", nullable: false),
                    Banknot50 = table.Column<int>(type: "integer", nullable: false),
                    Banknot100 = table.Column<int>(type: "integer", nullable: false),
                    Banknot200 = table.Column<int>(type: "integer", nullable: false),
                    ProcessType = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AtmHistories", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "ATMs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    ATMId = table.Column<int>(type: "integer", nullable: false),
                    Latitude = table.Column<float>(type: "real", nullable: false),
                    Longitude = table.Column<float>(type: "real", nullable: false),
                    ParaYatırma = table.Column<bool>(type: "boolean", nullable: false),
                    ParaÇekme = table.Column<bool>(type: "boolean", nullable: false),
                    TL = table.Column<bool>(type: "boolean", nullable: false),
                    USD = table.Column<bool>(type: "boolean", nullable: false),
                    Euro = table.Column<bool>(type: "boolean", nullable: false),
                    Status = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ATMs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DeliveryHistories",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    TotalDistance = table.Column<double>(type: "double precision", nullable: false),
                    TotalTimeMinute = table.Column<double>(type: "double precision", nullable: false),
                    TotalRoute = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeliveryHistories", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    name = table.Column<string>(type: "text", nullable: true),
                    email = table.Column<string>(type: "text", nullable: true),
                    password = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Capacities",
                columns: table => new
                {
                    CapacityId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    ATMId = table.Column<int>(type: "integer", nullable: false),
                    MevcutBanknot10 = table.Column<int>(type: "integer", nullable: false),
                    MevcutBanknot20 = table.Column<int>(type: "integer", nullable: false),
                    MevcutBanknot50 = table.Column<int>(type: "integer", nullable: false),
                    MevcutBanknot100 = table.Column<int>(type: "integer", nullable: false),
                    MevcutBanknot200 = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Capacities", x => x.CapacityId);
                    table.ForeignKey(
                        name: "FK_Capacities_ATMs_ATMId",
                        column: x => x.ATMId,
                        principalTable: "ATMs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StdCapacities",
                columns: table => new
                {
                    StdCapacityId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    ATMId = table.Column<int>(type: "integer", nullable: false),
                    Banknot10 = table.Column<int>(type: "integer", nullable: false),
                    Banknot20 = table.Column<int>(type: "integer", nullable: false),
                    Banknot50 = table.Column<int>(type: "integer", nullable: false),
                    Banknot100 = table.Column<int>(type: "integer", nullable: false),
                    Banknot200 = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StdCapacities", x => x.StdCapacityId);
                    table.ForeignKey(
                        name: "FK_StdCapacities_ATMs_ATMId",
                        column: x => x.ATMId,
                        principalTable: "ATMs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Capacities_ATMId",
                table: "Capacities",
                column: "ATMId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_StdCapacities_ATMId",
                table: "StdCapacities",
                column: "ATMId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AtmDeliveryHistories");

            migrationBuilder.DropTable(
                name: "AtmHistories");

            migrationBuilder.DropTable(
                name: "Capacities");

            migrationBuilder.DropTable(
                name: "DeliveryHistories");

            migrationBuilder.DropTable(
                name: "StdCapacities");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "ATMs");
        }
    }
}
