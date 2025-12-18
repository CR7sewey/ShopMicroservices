using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Shop.ProductAPI.Migrations
{
    /// <inheritdoc />
    public partial class SeedMoreData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("1e7dfbfd-28fb-491e-947a-3bad622009ee"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("fd15b842-2950-4f90-9c97-8b1b3bbe5cd5"));

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("3576bc6e-b2a4-481a-88c5-8409559c1083"), "Books" },
                    { new Guid("398a9c26-1884-417f-ba22-33405e67f08b"), "Watches" }
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "CategoryId", "Description", "ImageUrl", "Name", "Price", "Stock" },
                values: new object[,]
                {
                    { new Guid("199184ab-3630-4f3c-8232-44a2bf9ac5b5"), new Guid("3576bc6e-b2a4-481a-88c5-8409559c1083"), "Just a book", "", "The persue of hapiness", 9.99m, 111L },
                    { new Guid("1f13c916-dd5d-4fe9-808d-c662917d6c5b"), new Guid("398a9c26-1884-417f-ba22-33405e67f08b"), "Just a watch", "", "Seiko 1", 999m, 111L }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("199184ab-3630-4f3c-8232-44a2bf9ac5b5"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("1f13c916-dd5d-4fe9-808d-c662917d6c5b"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("3576bc6e-b2a4-481a-88c5-8409559c1083"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("398a9c26-1884-417f-ba22-33405e67f08b"));

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("1e7dfbfd-28fb-491e-947a-3bad622009ee"), "Watches" },
                    { new Guid("fd15b842-2950-4f90-9c97-8b1b3bbe5cd5"), "Books" }
                });
        }
    }
}
