﻿using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RSoft.Entry.Infra.Migrations
{

    [ExcludeFromCodeCoverage(Justification = "Migrations are automatically generated by .net")]
    public partial class InitialCreateDb : InitialSeed
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    FirstName = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LastName = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IsActive = table.Column<ulong>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AccrualPeriod",
                columns: table => new
                {
                    Year = table.Column<short>(type: "smallint", nullable: false),
                    Month = table.Column<sbyte>(type: "tinyint", nullable: false),
                    OpeningBalance = table.Column<float>(type: "float", nullable: false),
                    TotalCredits = table.Column<float>(type: "float", nullable: false),
                    TotalDebts = table.Column<float>(type: "float", nullable: false),
                    IsClosed = table.Column<ulong>(type: "bit", nullable: false, defaultValue: 0ul),
                    UserIdClosed = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    CreatedOn = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    ChangedOn = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    ChangedBy = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccrualPeriod", x => new { x.Year, x.Month });
                    table.ForeignKey(
                        name: "FK_User_AccrualPeriod_ChangedBy",
                        column: x => x.ChangedBy,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_User_AccrualPeriod_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_User_AccrualPeriod_UserIdClosed",
                        column: x => x.UserIdClosed,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Category",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    IsActive = table.Column<ulong>(type: "bit", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    ChangedOn = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    ChangedBy = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    Name = table.Column<string>(type: "varchar(80)", unicode: false, maxLength: 80, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Category", x => x.Id);
                    table.ForeignKey(
                        name: "FK_User_Category_ChangedBy",
                        column: x => x.ChangedBy,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_User_Category_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "PaymentMethod",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    PaymentType = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<ulong>(type: "bit", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    ChangedOn = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    ChangedBy = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    Name = table.Column<string>(type: "varchar(80)", unicode: false, maxLength: 80, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentMethod", x => x.Id);
                    table.ForeignKey(
                        name: "FK_User_PaymentMethod_ChangedBy",
                        column: x => x.ChangedBy,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_User_PaymentMethod_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Entry",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    IsActive = table.Column<ulong>(type: "bit", nullable: false),
                    CategoryId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CreatedOn = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    ChangedOn = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    ChangedBy = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    Name = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Entry", x => x.Id);
                    table.ForeignKey(
                        name: "FK__User_Entry_ChangedBy",
                        column: x => x.ChangedBy,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Entry_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Category",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_User_Entry_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Transaction",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Year = table.Column<short>(type: "smallint", nullable: false),
                    Month = table.Column<sbyte>(type: "tinyint", nullable: false),
                    Date = table.Column<DateTime>(type: "date", nullable: false),
                    TransactionType = table.Column<sbyte>(type: "tinyint", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(16,2)", nullable: false),
                    Comment = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    EntryId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    PaymentMethodId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CreatedOn = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transaction", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AccrualPeriod_Transaction_YearMonth",
                        columns: x => new { x.Year, x.Month },
                        principalTable: "AccrualPeriod",
                        principalColumns: new[] { "Year", "Month" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_User_Transaction_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_User_Transaction_EntryId",
                        column: x => x.EntryId,
                        principalTable: "Entry",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_User_Transaction_PaymentMethodId",
                        column: x => x.PaymentMethodId,
                        principalTable: "PaymentMethod",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_AccrualPeriod_ChangedBy",
                table: "AccrualPeriod",
                column: "ChangedBy");

            migrationBuilder.CreateIndex(
                name: "IX_AccrualPeriod_ChangedOn",
                table: "AccrualPeriod",
                column: "ChangedOn");

            migrationBuilder.CreateIndex(
                name: "IX_AccrualPeriod_CreatedBy",
                table: "AccrualPeriod",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_AccrualPeriod_CreatedOn",
                table: "AccrualPeriod",
                column: "CreatedOn");

            migrationBuilder.CreateIndex(
                name: "IX_AccrualPeriod_UserIdClosed",
                table: "AccrualPeriod",
                column: "UserIdClosed");

            migrationBuilder.CreateIndex(
                name: "AK_Category_Name",
                table: "Category",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Category_ChangedBy",
                table: "Category",
                column: "ChangedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Category_ChangedOn",
                table: "Category",
                column: "ChangedOn");

            migrationBuilder.CreateIndex(
                name: "IX_Category_CreatedBy",
                table: "Category",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Category_CreatedOn",
                table: "Category",
                column: "CreatedOn");

            migrationBuilder.CreateIndex(
                name: "AK_Entry_Name",
                table: "Entry",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Entry_CategoryId",
                table: "Entry",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Entry_ChangedBy",
                table: "Entry",
                column: "ChangedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Entry_ChangedOn",
                table: "Entry",
                column: "ChangedOn");

            migrationBuilder.CreateIndex(
                name: "IX_Entry_CreatedBy",
                table: "Entry",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Entry_CreatedOn",
                table: "Entry",
                column: "CreatedOn");

            migrationBuilder.CreateIndex(
                name: "AK_PaymentMethod_Name",
                table: "PaymentMethod",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PaymentMethod_ChangedBy",
                table: "PaymentMethod",
                column: "ChangedBy");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentMethod_ChangedOn",
                table: "PaymentMethod",
                column: "ChangedOn");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentMethod_CreatedBy",
                table: "PaymentMethod",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentMethod_CreatedOn",
                table: "PaymentMethod",
                column: "CreatedOn");

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_CreatedBy",
                table: "Transaction",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_CreatedOn",
                table: "Transaction",
                column: "CreatedOn");

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_Date",
                table: "Transaction",
                column: "Date");

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_EntryId",
                table: "Transaction",
                column: "EntryId");

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_Month",
                table: "Transaction",
                column: "Month");

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_PaymentMethodId",
                table: "Transaction",
                column: "PaymentMethodId");

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_Year",
                table: "Transaction",
                column: "Year");

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_Year_Month",
                table: "Transaction",
                columns: new[] { "Year", "Month" });

            migrationBuilder.CreateIndex(
                name: "IX_User_FullName",
                table: "User",
                columns: new[] { "FirstName", "LastName" });

            Seed(migrationBuilder);

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Transaction");

            migrationBuilder.DropTable(
                name: "AccrualPeriod");

            migrationBuilder.DropTable(
                name: "Entry");

            migrationBuilder.DropTable(
                name: "PaymentMethod");

            migrationBuilder.DropTable(
                name: "Category");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
