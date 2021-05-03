using Microsoft.EntityFrameworkCore.Migrations;

namespace ECChkAPI.Migrations
{
    public partial class initailRDS : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "IFECCUTFs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StoreNo = table.Column<string>(nullable: false),
                    EcCode1 = table.Column<string>(nullable: false),
                    EcCode2 = table.Column<string>(nullable: false),
                    EcLayer = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    CompanyName = table.Column<string>(nullable: false),
                    InDate = table.Column<string>(nullable: false),
                    Price = table.Column<string>(nullable: false),
                    State = table.Column<string>(nullable: false),
                    DeliType = table.Column<string>(nullable: false),
                    NewNumber = table.Column<string>(nullable: false),
                    EndThreeYard = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IFECCUTFs", x => x.Id);
                });

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
            migrationBuilder.DropTable(
               name: "IFECCUTFs");

        }
    }
}
