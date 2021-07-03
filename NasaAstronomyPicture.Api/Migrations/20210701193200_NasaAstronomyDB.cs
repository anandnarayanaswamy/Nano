using Microsoft.EntityFrameworkCore.Migrations;

namespace NasaAstronomyPicture.Api.Migrations
{
    public partial class NasaAstronomyDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "NasaAstronomyPictureModel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Copyright = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateString = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HdUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MediaType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SdUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NasaAstronomyPictureModel", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NasaAstronomyPictureModel");
        }
    }
}
