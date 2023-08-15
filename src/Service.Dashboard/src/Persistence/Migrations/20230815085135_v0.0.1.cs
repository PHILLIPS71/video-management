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
            migrationBuilder.EnsureSchema(
                name: "public");

            migrationBuilder.CreateTable(
                name: "FileSystemDirectories",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    parent_directory_id = table.Column<Guid>(type: "uuid", nullable: true),
                    path_info_name = table.Column<string>(type: "text", nullable: false),
                    path_info_full_name = table.Column<string>(type: "text", nullable: false),
                    path_info_extension = table.Column<string>(type: "text", nullable: true),
                    path_info_directory_path = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileSystemDirectories", x => x.id);
                    table.ForeignKey(
                        name: "fk_file_system_entries_file_system_directories_parent_directory_id",
                        column: x => x.parent_directory_id,
                        principalSchema: "public",
                        principalTable: "FileSystemDirectories",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "FileSystemFiles",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    parent_directory_id = table.Column<Guid>(type: "uuid", nullable: true),
                    size = table.Column<long>(type: "bigint", nullable: false),
                    path_info_name = table.Column<string>(type: "text", nullable: false),
                    path_info_full_name = table.Column<string>(type: "text", nullable: false),
                    path_info_extension = table.Column<string>(type: "text", nullable: true),
                    path_info_directory_path = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileSystemFiles", x => x.id);
                    table.ForeignKey(
                        name: "fk_file_system_entries_file_system_directories_parent_directory_id",
                        column: x => x.parent_directory_id,
                        principalSchema: "public",
                        principalTable: "FileSystemDirectories",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "libraries",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    slug = table.Column<string>(type: "text", nullable: false),
                    drive_status = table.Column<string>(type: "text", nullable: false),
                    directory_id = table.Column<Guid>(type: "uuid", nullable: false),
                    concurrency_token = table.Column<byte[]>(type: "bytea", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_libraries", x => x.id);
                    table.ForeignKey(
                        name: "fk_libraries_file_system_entries_directory_id",
                        column: x => x.directory_id,
                        principalSchema: "public",
                        principalTable: "FileSystemDirectories",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_file_system_directories_parent_directory_id",
                schema: "public",
                table: "FileSystemDirectories",
                column: "parent_directory_id");

            migrationBuilder.CreateIndex(
                name: "ix_file_system_files_parent_directory_id",
                schema: "public",
                table: "FileSystemFiles",
                column: "parent_directory_id");

            migrationBuilder.CreateIndex(
                name: "ix_libraries_directory_id",
                schema: "public",
                table: "libraries",
                column: "directory_id");

            migrationBuilder.CreateIndex(
                name: "ix_libraries_slug",
                schema: "public",
                table: "libraries",
                column: "slug",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FileSystemFiles",
                schema: "public");

            migrationBuilder.DropTable(
                name: "libraries",
                schema: "public");

            migrationBuilder.DropTable(
                name: "FileSystemDirectories",
                schema: "public");
        }
    }
}
