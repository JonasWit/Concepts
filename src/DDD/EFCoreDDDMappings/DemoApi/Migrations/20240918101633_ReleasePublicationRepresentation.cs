using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DemoApi.Migrations
{
    /// <inheritdoc />
    public partial class ReleasePublicationRepresentation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PublicationDate",
                table: "Books",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PublicationDiscriminator",
                table: "Books",
                type: "nvarchar(13)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PublicationDate",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "PublicationDiscriminator",
                table: "Books");
        }
    }
}
