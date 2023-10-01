using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace R54_M10_Class_09_Work_01.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    ProductId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Size = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "money", nullable: false),
                    Picture = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    OnSale = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.ProductId);
                });

            migrationBuilder.CreateTable(
                name: "Sales",
                columns: table => new
                {
                    SaleId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(type: "date", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sales", x => x.SaleId);
                    table.ForeignKey(
                        name: "FK_Sales_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "ProductId", "OnSale", "Picture", "Price", "ProductName", "Size" },
                values: new object[,]
                {
                    { 1, null, "1jpg", 1014m, "Product 1", 2 },
                    { 2, null, "2jpg", 1093m, "Product 2", 4 },
                    { 3, null, "3jpg", 1081m, "Product 3", 1 },
                    { 4, null, "4jpg", 1976m, "Product 4", 3 },
                    { 5, null, "5jpg", 1109m, "Product 5", 4 }
                });

            migrationBuilder.InsertData(
                table: "Sales",
                columns: new[] { "SaleId", "Date", "ProductId", "Quantity" },
                values: new object[,]
                {
                    { 1, new DateTime(2022, 8, 1, 17, 6, 46, 371, DateTimeKind.Local).AddTicks(9260), 1, 106 },
                    { 2, new DateTime(2022, 6, 14, 17, 6, 46, 371, DateTimeKind.Local).AddTicks(9291), 2, 127 },
                    { 3, new DateTime(2022, 7, 12, 17, 6, 46, 371, DateTimeKind.Local).AddTicks(9299), 3, 141 },
                    { 4, new DateTime(2022, 6, 16, 17, 6, 46, 371, DateTimeKind.Local).AddTicks(9307), 4, 146 },
                    { 5, new DateTime(2022, 7, 13, 17, 6, 46, 371, DateTimeKind.Local).AddTicks(9315), 5, 152 },
                    { 6, new DateTime(2022, 6, 16, 17, 6, 46, 371, DateTimeKind.Local).AddTicks(9325), 1, 196 },
                    { 7, new DateTime(2022, 7, 17, 17, 6, 46, 371, DateTimeKind.Local).AddTicks(9333), 2, 147 },
                    { 8, new DateTime(2022, 6, 11, 17, 6, 46, 371, DateTimeKind.Local).AddTicks(9341), 3, 120 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Sales_ProductId",
                table: "Sales",
                column: "ProductId");
            string sqlI = @"CREATE PROC InsertProduct @n VARCHAR(50), @p MONEY, @s INT, @pi VARCHAR(50), @o BIT
                         AS
                         INSERT INTO Products (ProductName, Price, Size, Picture, OnSale)
                         VALUES (@n, @p, @s, @pi, @o )

                         GO";
            migrationBuilder.Sql(sqlI);
            string sqlU = @"CREATE PROC UpdateProduct @i INT, @n VARCHAR(50), @p MONEY, @s INT, @pi VARCHAR(50), @o BIT
                         AS
                         UPDATE Products SET ProductName=@n, Price=@p, Size=@s, Picture=@pi, OnSale=@o
                         WHERE ProductId=@i
                         GO";
            migrationBuilder.Sql(sqlU);
            string sqlD = @"CREATE PROC DeleteProduct @i INT
                         AS
                         DELETE Products 
                         WHERE ProductId=@i
                         GO";
            migrationBuilder.Sql(sqlD);
            string sqlS = @"CREATE PROC InsertSale @d DATE, @q INT, @pid INT
                         AS
                         INSERT INTO Sales ([Date], Quantity, ProductId)
                         VALUES (@d, @q, @pid )
                         GO";
            migrationBuilder.Sql(sqlS);
            string sqlSU = @"CREATE PROC UpdateSale @id INT, @d DATE, @q INT, @pid INT
                         AS
                         UPDATE Sales SET [Date]=@d, Quantity=@q, ProductId=@pid
                         WHERE SaleId = @id
                         GO";
            migrationBuilder.Sql(sqlSU);
            string sqlDU = @"CREATE PROC DeleteSale @id INT
                     AS
                     DELETE Sales 
                     WHERE SaleId = @id
                     GO";
            migrationBuilder.Sql(sqlDU);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Sales");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.Sql("DROP PROC InsertProduct");
            migrationBuilder.Sql("DROP PROC UpdateProduct");
            migrationBuilder.Sql("DROP PROC DeleteProduct");
            migrationBuilder.Sql("DROP PROC InsertSale");
            migrationBuilder.Sql("DROP PROC UpdateSale");
            migrationBuilder.Sql("DROP PROC DeleteSale");
        }
    }
}
