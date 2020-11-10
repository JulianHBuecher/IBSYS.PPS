using Microsoft.EntityFrameworkCore.Migrations;

namespace IBSYS.PPS.Migrations
{
    public partial class Orders_For_K_Added : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OrdersForK",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PartName = table.Column<string>(nullable: true),
                    OrderQuantity = table.Column<string>(nullable: true),
                    OrderModus = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrdersForK", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrdersForK");
        }
    }
}
