using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataProj.DAL.Migrations
{
    /// <inheritdoc />
    public partial class InitTest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ProjectName",
                table: "Projects",
                newName: "Name");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Projects",
                newName: "ProjectName");
        }
    }
}
