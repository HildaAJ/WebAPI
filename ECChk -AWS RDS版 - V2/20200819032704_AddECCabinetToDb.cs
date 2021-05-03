using Microsoft.EntityFrameworkCore.Migrations;

namespace ECChkAPI.Migrations
{
    public partial class AddECCabinetToDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ECCabinetBarcode",
                table: "IFECCUTFs",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ECCabinetName",
                table: "IFECCUTFs",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ECCabinetBarcode",
                table: "IFECCUTFs");

            migrationBuilder.DropColumn(
                name: "ECCabinetName",
                table: "IFECCUTFs");
        }
    }
}
