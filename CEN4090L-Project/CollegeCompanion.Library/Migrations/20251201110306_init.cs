using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CollegeCompanion.Library.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "SavingsGoal");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "SavingsGoal",
                newName: "Title");

            migrationBuilder.RenameColumn(
                name: "TotalAmount",
                table: "Budget",
                newName: "total_amount");

            migrationBuilder.AlterColumn<DateTime>(
                name: "TargetDate",
                table: "SavingsGoal",
                type: "timestamp",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "TargetAmount",
                table: "SavingsGoal",
                type: "decimal",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric");

            migrationBuilder.AlterColumn<int>(
                name: "Priority",
                table: "SavingsGoal",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<decimal>(
                name: "MonthlyAllocation",
                table: "SavingsGoal",
                type: "decimal",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric");

            migrationBuilder.AlterColumn<decimal>(
                name: "CurrentAmount",
                table: "SavingsGoal",
                type: "decimal",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "SavingsGoal",
                type: "timestamp",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateTime",
                table: "Expense",
                type: "timestamp",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<decimal>(
                name: "Amount",
                table: "Expense",
                type: "decimal",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Title",
                table: "SavingsGoal",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "total_amount",
                table: "Budget",
                newName: "TotalAmount");

            migrationBuilder.AlterColumn<DateTime>(
                name: "TargetDate",
                table: "SavingsGoal",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "TargetAmount",
                table: "SavingsGoal",
                type: "numeric",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal");

            migrationBuilder.AlterColumn<int>(
                name: "Priority",
                table: "SavingsGoal",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<decimal>(
                name: "MonthlyAllocation",
                table: "SavingsGoal",
                type: "numeric",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal");

            migrationBuilder.AlterColumn<decimal>(
                name: "CurrentAmount",
                table: "SavingsGoal",
                type: "numeric",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "SavingsGoal",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "SavingsGoal",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateTime",
                table: "Expense",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp");

            migrationBuilder.AlterColumn<decimal>(
                name: "Amount",
                table: "Expense",
                type: "numeric",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal",
                oldNullable: true);
        }
    }
}
