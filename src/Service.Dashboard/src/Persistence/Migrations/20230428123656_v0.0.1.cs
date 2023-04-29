using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Giantnodes.Service.Dashboard.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class v001 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Directories",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    full_path = table.Column<string>(type: "text", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    parent_directory_id = table.Column<Guid>(type: "uuid", nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Directories", x => x.id);
                    table.ForeignKey(
                        name: "fk_nodes_directories_parent_directory_id",
                        column: x => x.parent_directory_id,
                        principalTable: "Directories",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "libraries",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    slug = table.Column<string>(type: "text", nullable: false),
                    full_path = table.Column<string>(type: "text", nullable: false),
                    drive_status = table.Column<string>(type: "text", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_libraries", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Files",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    full_path = table.Column<string>(type: "text", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    parent_directory_id = table.Column<Guid>(type: "uuid", nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    size = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Files", x => x.id);
                    table.ForeignKey(
                        name: "fk_nodes_directories_parent_directory_id",
                        column: x => x.parent_directory_id,
                        principalTable: "Directories",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "ix_directories_full_path",
                table: "Directories",
                column: "full_path",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_directories_parent_directory_id",
                table: "Directories",
                column: "parent_directory_id");

            migrationBuilder.CreateIndex(
                name: "ix_files_full_path",
                table: "Files",
                column: "full_path",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_files_parent_directory_id",
                table: "Files",
                column: "parent_directory_id");

            migrationBuilder.CreateIndex(
                name: "ix_libraries_full_path",
                table: "libraries",
                column: "full_path",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_libraries_slug",
                table: "libraries",
                column: "slug",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Files");

            migrationBuilder.DropTable(
                name: "libraries");

            migrationBuilder.DropTable(
                name: "Directories");
        }
    }
}
