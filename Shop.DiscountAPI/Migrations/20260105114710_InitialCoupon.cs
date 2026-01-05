using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Shop.DiscountAPI.Migrations
{
    /// <inheritdoc />
    public partial class InitialCoupon : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Coupons",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CouponCode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    DiscountAmount = table.Column<decimal>(type: "numeric(14,2)", precision: 14, scale: 2, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ExpiryDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Coupons", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Coupons",
                columns: new[] { "Id", "CouponCode", "CreatedAt", "DiscountAmount", "ExpiryDate" },
                values: new object[,]
                {
                    { new Guid("5c656c3f-2b59-4209-836f-9bf53516de0e"), "SUMMER15", new DateTime(2026, 1, 5, 11, 47, 10, 519, DateTimeKind.Utc).AddTicks(8428), 15.00m, new DateTime(2026, 3, 5, 11, 47, 10, 519, DateTimeKind.Utc).AddTicks(8429) },
                    { new Guid("83212972-ae1d-424c-a65f-b0033be942f6"), "WELCOME10", new DateTime(2026, 1, 5, 11, 47, 10, 519, DateTimeKind.Utc).AddTicks(7961), 10.00m, new DateTime(2026, 2, 5, 11, 47, 10, 519, DateTimeKind.Utc).AddTicks(8085) }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Coupons");
        }
    }
}
