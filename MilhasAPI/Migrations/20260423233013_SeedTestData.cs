using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MilhasAPI.Migrations
{
    /// <inheritdoc />
    public partial class SeedTestData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "Name" },
                values: new object[,]
                {
                    { 100, "ana.silva@email.com", "Ana Silva" },
                    { 101, "carlos.mendes@email.com", "Carlos Mendes" },
                    { 102, "beatriz.oliveira@email.com", "Beatriz Oliveira" },
                    { 103, "rafael.costa@email.com", "Rafael Costa" },
                    { 104, "juliana.santos@email.com", "Juliana Santos" }
                });

            migrationBuilder.InsertData(
                table: "CreditCards",
                columns: new[] { "Id", "Brand", "CardNumber", "UserId" },
                values: new object[,]
                {
                    { 100, "Visa Infinite", "**** **** **** 1234", 100 },
                    { 101, "Mastercard Black", "**** **** **** 5678", 100 },
                    { 102, "Amex Platinum", "**** **** **** 9012", 101 },
                    { 103, "Visa Platinum", "**** **** **** 3456", 101 },
                    { 104, "Elo Nanquim", "**** **** **** 7890", 102 },
                    { 105, "Mastercard Platinum", "**** **** **** 2345", 102 },
                    { 106, "Visa Gold", "**** **** **** 6789", 103 },
                    { 107, "Amex Gold", "**** **** **** 0123", 104 },
                    { 108, "Mastercard Black", "**** **** **** 4567", 104 }
                });

            migrationBuilder.InsertData(
                table: "UserProfiles",
                columns: new[] { "Id", "AnnualCardFeeBudget", "CurrentMilesBalance", "InterestedInCardUpgrades", "InterestedInMilesTransferPromos", "InvestmentProfile", "MaxMilePurchasePrice", "MonthlyCardSpending", "MonthlyIncome", "MonthlyMilesGoal", "NumberOfCreditCards", "PreferredCabinClass", "PreferredLoyaltyProgram", "PrefersDomesticTravel", "PrefersInternationalTravel", "TravelFrequency", "UserId" },
                values: new object[,]
                {
                    { 100, 1500m, 120000, true, true, 2, 22m, 8000m, 12000m, 15000, 2, 2, 4, true, true, 2, 100 },
                    { 101, 3000m, 350000, true, true, 2, 30m, 18000m, 25000m, 30000, 2, 3, 0, false, true, 3, 101 },
                    { 102, 600m, 45000, false, true, 1, 18m, 4000m, 7000m, 8000, 2, 1, 2, true, false, 1, 102 },
                    { 103, 0m, 8000, false, false, 0, 12m, 2500m, 4500m, 3000, 1, 0, 1, true, false, 0, 103 },
                    { 104, 2000m, 200000, true, true, 1, 25m, 10000m, 15000m, 20000, 2, 2, 5, true, true, 2, 104 }
                });

            migrationBuilder.InsertData(
                table: "RewardTransactions",
                columns: new[] { "Id", "Amount", "CreditCardId", "Date", "MilesEarned", "UserId" },
                values: new object[,]
                {
                    { 100, 2500m, 100, new DateTime(2026, 1, 15, 0, 0, 0, 0, DateTimeKind.Utc), 7500, 100 },
                    { 101, 3200m, 101, new DateTime(2026, 2, 10, 0, 0, 0, 0, DateTimeKind.Utc), 9600, 100 },
                    { 102, 8000m, 102, new DateTime(2026, 1, 20, 0, 0, 0, 0, DateTimeKind.Utc), 24000, 101 },
                    { 103, 5500m, 103, new DateTime(2026, 3, 5, 0, 0, 0, 0, DateTimeKind.Utc), 11000, 101 },
                    { 104, 1800m, 104, new DateTime(2026, 2, 28, 0, 0, 0, 0, DateTimeKind.Utc), 3600, 102 },
                    { 105, 900m, 106, new DateTime(2026, 3, 12, 0, 0, 0, 0, DateTimeKind.Utc), 1800, 103 },
                    { 106, 4200m, 107, new DateTime(2026, 1, 8, 0, 0, 0, 0, DateTimeKind.Utc), 12600, 104 },
                    { 107, 6100m, 108, new DateTime(2026, 2, 18, 0, 0, 0, 0, DateTimeKind.Utc), 18300, 104 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "CreditCards",
                keyColumn: "Id",
                keyValue: 105);

            migrationBuilder.DeleteData(
                table: "RewardTransactions",
                keyColumn: "Id",
                keyValue: 100);

            migrationBuilder.DeleteData(
                table: "RewardTransactions",
                keyColumn: "Id",
                keyValue: 101);

            migrationBuilder.DeleteData(
                table: "RewardTransactions",
                keyColumn: "Id",
                keyValue: 102);

            migrationBuilder.DeleteData(
                table: "RewardTransactions",
                keyColumn: "Id",
                keyValue: 103);

            migrationBuilder.DeleteData(
                table: "RewardTransactions",
                keyColumn: "Id",
                keyValue: 104);

            migrationBuilder.DeleteData(
                table: "RewardTransactions",
                keyColumn: "Id",
                keyValue: 105);

            migrationBuilder.DeleteData(
                table: "RewardTransactions",
                keyColumn: "Id",
                keyValue: 106);

            migrationBuilder.DeleteData(
                table: "RewardTransactions",
                keyColumn: "Id",
                keyValue: 107);

            migrationBuilder.DeleteData(
                table: "UserProfiles",
                keyColumn: "Id",
                keyValue: 100);

            migrationBuilder.DeleteData(
                table: "UserProfiles",
                keyColumn: "Id",
                keyValue: 101);

            migrationBuilder.DeleteData(
                table: "UserProfiles",
                keyColumn: "Id",
                keyValue: 102);

            migrationBuilder.DeleteData(
                table: "UserProfiles",
                keyColumn: "Id",
                keyValue: 103);

            migrationBuilder.DeleteData(
                table: "UserProfiles",
                keyColumn: "Id",
                keyValue: 104);

            migrationBuilder.DeleteData(
                table: "CreditCards",
                keyColumn: "Id",
                keyValue: 100);

            migrationBuilder.DeleteData(
                table: "CreditCards",
                keyColumn: "Id",
                keyValue: 101);

            migrationBuilder.DeleteData(
                table: "CreditCards",
                keyColumn: "Id",
                keyValue: 102);

            migrationBuilder.DeleteData(
                table: "CreditCards",
                keyColumn: "Id",
                keyValue: 103);

            migrationBuilder.DeleteData(
                table: "CreditCards",
                keyColumn: "Id",
                keyValue: 104);

            migrationBuilder.DeleteData(
                table: "CreditCards",
                keyColumn: "Id",
                keyValue: 106);

            migrationBuilder.DeleteData(
                table: "CreditCards",
                keyColumn: "Id",
                keyValue: 107);

            migrationBuilder.DeleteData(
                table: "CreditCards",
                keyColumn: "Id",
                keyValue: 108);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 100);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 101);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 102);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 103);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 104);
        }
    }
}
