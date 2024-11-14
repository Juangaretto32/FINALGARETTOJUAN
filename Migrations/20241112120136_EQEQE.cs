using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FINALGARETTOJUAN.Migrations
{
    /// <inheritdoc />
    public partial class EQEQE : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IdStock",
                table: "Productos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "stockId",
                table: "Productos",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Productos_stockId",
                table: "Productos",
                column: "stockId");

            migrationBuilder.AddForeignKey(
                name: "FK_Productos_Stocks_stockId",
                table: "Productos",
                column: "stockId",
                principalTable: "Stocks",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Productos_Stocks_stockId",
                table: "Productos");

            migrationBuilder.DropIndex(
                name: "IX_Productos_stockId",
                table: "Productos");

            migrationBuilder.DropColumn(
                name: "IdStock",
                table: "Productos");

            migrationBuilder.DropColumn(
                name: "stockId",
                table: "Productos");
        }
    }
}
