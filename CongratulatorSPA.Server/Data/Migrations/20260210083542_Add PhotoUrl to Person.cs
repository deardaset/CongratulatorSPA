using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CongratulatorSPA.Server.Migrations
{
    /// <inheritdoc />
    public partial class AddPhotoUrltoPerson : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PhotoUrl",
                table: "People",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PhotoUrl",
                table: "People");
        }
    }
}
