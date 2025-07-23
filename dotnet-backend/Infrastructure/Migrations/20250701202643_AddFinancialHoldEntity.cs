using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddFinancialHoldEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "financial_holds",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    reason = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    hold_amount = table.Column<decimal>(type: "numeric(10,2)", nullable: false),
                    date_placed = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    date_resolved = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    is_active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    resolution_notes = table.Column<string>(type: "varchar", maxLength: 500, nullable: true),
                    student_id = table.Column<int>(type: "integer", nullable: false),
                    placed_by_user_id = table.Column<int>(type: "integer", nullable: false),
                    resolved_by_user_id = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_financial_holds", x => x.id);
                    table.ForeignKey(
                        name: "FK_financial_holds_students_student_id",
                        column: x => x.student_id,
                        principalTable: "students",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_financial_holds_users_placed_by_user_id",
                        column: x => x.placed_by_user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_financial_holds_users_resolved_by_user_id",
                        column: x => x.resolved_by_user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_financial_holds_placed_by_user_id",
                table: "financial_holds",
                column: "placed_by_user_id");

            migrationBuilder.CreateIndex(
                name: "IX_financial_holds_resolved_by_user_id",
                table: "financial_holds",
                column: "resolved_by_user_id");

            migrationBuilder.CreateIndex(
                name: "ix_financial_holds_student",
                table: "financial_holds",
                column: "student_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "financial_holds");
        }
    }
}
