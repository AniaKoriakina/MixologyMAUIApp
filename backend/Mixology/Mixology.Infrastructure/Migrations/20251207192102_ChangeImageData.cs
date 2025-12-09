using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mixology.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangeImageData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Image",
                table: "Mixes");

            migrationBuilder.DropColumn(
                name: "Image",
                table: "Collections");

            migrationBuilder.DropColumn(
                name: "Logo",
                table: "Brands");

            migrationBuilder.AddColumn<string>(
                name: "ImageContentType",
                table: "Mixes",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "ImageData",
                table: "Mixes",
                type: "bytea",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImageFileName",
                table: "Mixes",
                type: "character varying(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImageContentType",
                table: "Collections",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "ImageData",
                table: "Collections",
                type: "bytea",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImageFileName",
                table: "Collections",
                type: "character varying(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LogoContentType",
                table: "Brands",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "LogoData",
                table: "Brands",
                type: "bytea",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LogoFileName",
                table: "Brands",
                type: "character varying(255)",
                maxLength: 255,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageContentType",
                table: "Mixes");

            migrationBuilder.DropColumn(
                name: "ImageData",
                table: "Mixes");

            migrationBuilder.DropColumn(
                name: "ImageFileName",
                table: "Mixes");

            migrationBuilder.DropColumn(
                name: "ImageContentType",
                table: "Collections");

            migrationBuilder.DropColumn(
                name: "ImageData",
                table: "Collections");

            migrationBuilder.DropColumn(
                name: "ImageFileName",
                table: "Collections");

            migrationBuilder.DropColumn(
                name: "LogoContentType",
                table: "Brands");

            migrationBuilder.DropColumn(
                name: "LogoData",
                table: "Brands");

            migrationBuilder.DropColumn(
                name: "LogoFileName",
                table: "Brands");

            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "Mixes",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "Collections",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Logo",
                table: "Brands",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
