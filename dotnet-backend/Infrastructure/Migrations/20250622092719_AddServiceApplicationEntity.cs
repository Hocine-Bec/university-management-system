using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddServiceApplicationEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "service_applications",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    application_date = table.Column<DateTime>(type: "date", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    status = table.Column<int>(type: "integer", nullable: false, defaultValue: 1),
                    paid_fees = table.Column<decimal>(type: "numeric(8,2)", nullable: false),
                    varchar = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    completed_date = table.Column<DateTime>(type: "date", nullable: true),
                    person_id = table.Column<int>(type: "integer", nullable: false),
                    service_id = table.Column<int>(type: "integer", nullable: false),
                    processed_by_user_id = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_service_applications", x => x.id);
                    table.ForeignKey(
                        name: "FK_service_applications_people_person_id",
                        column: x => x.person_id,
                        principalTable: "people",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_service_applications_service_offers_service_id",
                        column: x => x.service_id,
                        principalTable: "service_offers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_service_applications_users_processed_by_user_id",
                        column: x => x.processed_by_user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_service_applications_person_id",
                table: "service_applications",
                column: "person_id");

            migrationBuilder.CreateIndex(
                name: "IX_service_applications_processed_by_user_id",
                table: "service_applications",
                column: "processed_by_user_id");

            migrationBuilder.CreateIndex(
                name: "IX_service_applications_service_id",
                table: "service_applications",
                column: "service_id");

            migrationBuilder.CreateIndex(
                name: "ix_service_applications_status",
                table: "service_applications",
                column: "status");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "service_applications");
        }
    }
}
