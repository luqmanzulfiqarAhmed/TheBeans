using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TheBeans.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CoffeeBeans",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Origin = table.Column<string>(type: "text", nullable: false),
                    RoastLevel = table.Column<string>(type: "text", nullable: false),
                    Price = table.Column<decimal>(type: "numeric(10,2)", nullable: false),
                    Currency = table.Column<string>(type: "text", nullable: false),
                    ImageUrl = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CoffeeBeans", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BeansOfTheDay",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CoffeeBeanId = table.Column<Guid>(type: "uuid", nullable: false),
                    SelectedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BeansOfTheDay", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BeansOfTheDay_CoffeeBeans_CoffeeBeanId",
                        column: x => x.CoffeeBeanId,
                        principalTable: "CoffeeBeans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BeansOfTheDay_CoffeeBeanId",
                table: "BeansOfTheDay",
                column: "CoffeeBeanId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BeansOfTheDay_SelectedDate",
                table: "BeansOfTheDay",
                column: "SelectedDate",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BeansOfTheDay");

            migrationBuilder.DropTable(
                name: "CoffeeBeans");
        }
    }
}
