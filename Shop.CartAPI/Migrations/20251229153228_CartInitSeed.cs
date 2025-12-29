using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shop.CartAPI.Migrations
{
    /// <inheritdoc />
    public partial class CartInitSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "CartItems",
                keyColumn: "Id",
                keyValue: new Guid("8a71835c-2042-4387-b576-3239a6ef4189"));

            migrationBuilder.DeleteData(
                table: "CartHeaders",
                keyColumn: "Id",
                keyValue: new Guid("78f8b0cd-44c3-48ad-9676-bcbb9288eeef"));

            migrationBuilder.InsertData(
                table: "CartHeaders",
                columns: new[] { "Id", "CouponCode", "UserId" },
                values: new object[] { new Guid("0b6d1249-7e5c-4ce0-b910-ec96bbee5ac3"), "DISC123", new Guid("01c3d0c8-3e3c-421c-b19d-c53d0bc751e5") });

            migrationBuilder.InsertData(
                table: "CartItems",
                columns: new[] { "Id", "CartHeaderId", "ProductId", "Quantity" },
                values: new object[] { new Guid("ee45e77b-2248-4e3a-bcea-b0e16a297e4b"), new Guid("0b6d1249-7e5c-4ce0-b910-ec96bbee5ac3"), new Guid("199184ab-3630-4f3c-8232-44a2bf9ac5b5"), 1 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "CartItems",
                keyColumn: "Id",
                keyValue: new Guid("ee45e77b-2248-4e3a-bcea-b0e16a297e4b"));

            migrationBuilder.DeleteData(
                table: "CartHeaders",
                keyColumn: "Id",
                keyValue: new Guid("0b6d1249-7e5c-4ce0-b910-ec96bbee5ac3"));

            migrationBuilder.InsertData(
                table: "CartHeaders",
                columns: new[] { "Id", "CouponCode", "UserId" },
                values: new object[] { new Guid("78f8b0cd-44c3-48ad-9676-bcbb9288eeef"), "DISC123", new Guid("01c3d0c8-3e3c-421c-b19d-c53d0bc751e5") });

            migrationBuilder.InsertData(
                table: "CartItems",
                columns: new[] { "Id", "CartHeaderId", "ProductId", "Quantity" },
                values: new object[] { new Guid("8a71835c-2042-4387-b576-3239a6ef4189"), new Guid("78f8b0cd-44c3-48ad-9676-bcbb9288eeef"), new Guid("199184ab-3630-4f3c-8232-44a2bf9ac5b5"), 1 });
        }
    }
}
