using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DemoApi.Migrations
{
    /// <inheritdoc />
    public partial class ReleaseEditionRepresentation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EditionDiscriminator",
                table: "Books",
                type: "nvarchar(15)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "EditionNumber",
                table: "Books",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "EditionSeason",
                table: "Books",
                type: "nvarchar(6)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PublisherId",
                table: "Books",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Books_PublisherId",
                table: "Books",
                column: "PublisherId");

            migrationBuilder.AddForeignKey(
                name: "FK_Books_Publishers_PublisherId",
                table: "Books",
                column: "PublisherId",
                principalTable: "Publishers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Books_Publishers_PublisherId",
                table: "Books");

            migrationBuilder.DropIndex(
                name: "IX_Books_PublisherId",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "EditionDiscriminator",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "EditionNumber",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "EditionSeason",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "PublisherId",
                table: "Books");
        }
    }
}
