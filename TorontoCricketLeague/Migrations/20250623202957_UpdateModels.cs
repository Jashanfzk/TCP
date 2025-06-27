using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TorontoCricketLeague.Migrations
{
    /// <inheritdoc />
    public partial class UpdateModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FranchiseId",
                table: "Sponsors",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Sponsors_FranchiseId",
                table: "Sponsors",
                column: "FranchiseId");

            migrationBuilder.AddForeignKey(
                name: "FK_Sponsors_Franchises_FranchiseId",
                table: "Sponsors",
                column: "FranchiseId",
                principalTable: "Franchises",
                principalColumn: "FranchiseId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sponsors_Franchises_FranchiseId",
                table: "Sponsors");

            migrationBuilder.DropIndex(
                name: "IX_Sponsors_FranchiseId",
                table: "Sponsors");

            migrationBuilder.DropColumn(
                name: "FranchiseId",
                table: "Sponsors");
        }
    }
}
