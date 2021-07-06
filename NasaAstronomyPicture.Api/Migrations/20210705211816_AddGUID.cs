using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NasaAstronomyPicture.Api.Migrations
{
    public partial class AddGUID : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateString",
                table: "NasaAstronomyPictureModel");

            migrationBuilder.DropColumn(
                name: "MediaType",
                table: "NasaAstronomyPictureModel");

            migrationBuilder.DropColumn(
                name: "SdUrl",
                table: "NasaAstronomyPictureModel");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "NasaAstronomyPictureModel",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "HdUrl",
                table: "NasaAstronomyPictureModel",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Date",
                table: "NasaAstronomyPictureModel",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "ImageGUID",
                table: "NasaAstronomyPictureModel",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "Media_Type",
                table: "NasaAstronomyPictureModel",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Url",
                table: "NasaAstronomyPictureModel",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Date",
                table: "NasaAstronomyPictureModel");

            migrationBuilder.DropColumn(
                name: "ImageGUID",
                table: "NasaAstronomyPictureModel");

            migrationBuilder.DropColumn(
                name: "Media_Type",
                table: "NasaAstronomyPictureModel");

            migrationBuilder.DropColumn(
                name: "Url",
                table: "NasaAstronomyPictureModel");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "NasaAstronomyPictureModel",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "HdUrl",
                table: "NasaAstronomyPictureModel",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "DateString",
                table: "NasaAstronomyPictureModel",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MediaType",
                table: "NasaAstronomyPictureModel",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SdUrl",
                table: "NasaAstronomyPictureModel",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
