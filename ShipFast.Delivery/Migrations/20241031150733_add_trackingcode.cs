using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShipFast.Delivery.Migrations
{
    public partial class add_trackingcode : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TrackingCode",
                table: "Deliveries",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TrackingCode",
                table: "Deliveries");
        }
    }
}
