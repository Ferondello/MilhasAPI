using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace MilhasAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddUserProfile : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserProfiles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    MonthlyIncome = table.Column<decimal>(type: "numeric", nullable: false),
                    InvestmentProfile = table.Column<int>(type: "integer", nullable: false),
                    MonthlyCardSpending = table.Column<decimal>(type: "numeric", nullable: false),
                    AnnualCardFeeBudget = table.Column<decimal>(type: "numeric", nullable: false),
                    NumberOfCreditCards = table.Column<int>(type: "integer", nullable: false),
                    TravelFrequency = table.Column<int>(type: "integer", nullable: false),
                    PreferredCabinClass = table.Column<int>(type: "integer", nullable: false),
                    PreferredLoyaltyProgram = table.Column<int>(type: "integer", nullable: false),
                    CurrentMilesBalance = table.Column<int>(type: "integer", nullable: false),
                    MonthlyMilesGoal = table.Column<int>(type: "integer", nullable: false),
                    PrefersDomesticTravel = table.Column<bool>(type: "boolean", nullable: false),
                    PrefersInternationalTravel = table.Column<bool>(type: "boolean", nullable: false),
                    MaxMilePurchasePrice = table.Column<decimal>(type: "numeric", nullable: false),
                    InterestedInCardUpgrades = table.Column<bool>(type: "boolean", nullable: false),
                    InterestedInMilesTransferPromos = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserProfiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserProfiles_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserProfiles_UserId",
                table: "UserProfiles",
                column: "UserId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserProfiles");
        }
    }
}
