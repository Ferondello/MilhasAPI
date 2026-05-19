using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MilhasAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddProgramToCardsAndTransactions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Program",
                table: "RewardTransactions",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Program",
                table: "CreditCards",
                type: "integer",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "CreditCards",
                keyColumn: "Id",
                keyValue: 100,
                column: "Program",
                value: null);

            migrationBuilder.UpdateData(
                table: "CreditCards",
                keyColumn: "Id",
                keyValue: 101,
                column: "Program",
                value: null);

            migrationBuilder.UpdateData(
                table: "CreditCards",
                keyColumn: "Id",
                keyValue: 102,
                column: "Program",
                value: null);

            migrationBuilder.UpdateData(
                table: "CreditCards",
                keyColumn: "Id",
                keyValue: 103,
                column: "Program",
                value: null);

            migrationBuilder.UpdateData(
                table: "CreditCards",
                keyColumn: "Id",
                keyValue: 104,
                column: "Program",
                value: null);

            migrationBuilder.UpdateData(
                table: "CreditCards",
                keyColumn: "Id",
                keyValue: 105,
                column: "Program",
                value: null);

            migrationBuilder.UpdateData(
                table: "CreditCards",
                keyColumn: "Id",
                keyValue: 106,
                column: "Program",
                value: null);

            migrationBuilder.UpdateData(
                table: "CreditCards",
                keyColumn: "Id",
                keyValue: 107,
                column: "Program",
                value: null);

            migrationBuilder.UpdateData(
                table: "CreditCards",
                keyColumn: "Id",
                keyValue: 108,
                column: "Program",
                value: null);

            migrationBuilder.UpdateData(
                table: "RewardTransactions",
                keyColumn: "Id",
                keyValue: 100,
                column: "Program",
                value: null);

            migrationBuilder.UpdateData(
                table: "RewardTransactions",
                keyColumn: "Id",
                keyValue: 101,
                column: "Program",
                value: null);

            migrationBuilder.UpdateData(
                table: "RewardTransactions",
                keyColumn: "Id",
                keyValue: 102,
                column: "Program",
                value: null);

            migrationBuilder.UpdateData(
                table: "RewardTransactions",
                keyColumn: "Id",
                keyValue: 103,
                column: "Program",
                value: null);

            migrationBuilder.UpdateData(
                table: "RewardTransactions",
                keyColumn: "Id",
                keyValue: 104,
                column: "Program",
                value: null);

            migrationBuilder.UpdateData(
                table: "RewardTransactions",
                keyColumn: "Id",
                keyValue: 105,
                column: "Program",
                value: null);

            migrationBuilder.UpdateData(
                table: "RewardTransactions",
                keyColumn: "Id",
                keyValue: 106,
                column: "Program",
                value: null);

            migrationBuilder.UpdateData(
                table: "RewardTransactions",
                keyColumn: "Id",
                keyValue: 107,
                column: "Program",
                value: null);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Program",
                table: "RewardTransactions");

            migrationBuilder.DropColumn(
                name: "Program",
                table: "CreditCards");
        }
    }
}
