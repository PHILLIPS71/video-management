using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

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
                name: "libraries",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    slug = table.Column<string>(type: "text", nullable: false),
                    path_info_name = table.Column<string>(type: "text", nullable: false),
                    path_info_full_name = table.Column<string>(type: "text", nullable: false),
                    path_info_extension = table.Column<string>(type: "text", nullable: true),
                    path_info_directory_path = table.Column<string>(type: "text", nullable: true),
                    path_info_directory_separator_char = table.Column<char>(type: "character(1)", nullable: false),
                    status = table.Column<string>(type: "text", nullable: false),
                    is_watched = table.Column<bool>(type: "boolean", nullable: false),
                    concurrency_token = table.Column<byte[]>(type: "bytea", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_libraries", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "FileSystemDirectories",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    size = table.Column<long>(type: "bigint", nullable: false),
                    parent_directory_id = table.Column<Guid>(type: "uuid", nullable: true),
                    library_id = table.Column<Guid>(type: "uuid", nullable: false),
                    path_info_name = table.Column<string>(type: "text", nullable: false),
                    path_info_full_name = table.Column<string>(type: "text", nullable: false),
                    path_info_extension = table.Column<string>(type: "text", nullable: true),
                    path_info_directory_path = table.Column<string>(type: "text", nullable: true),
                    path_info_directory_separator_char = table.Column<char>(type: "character(1)", nullable: false)
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
                    table.ForeignKey(
                        name: "fk_file_system_entries_libraries_library_id",
                        column: x => x.library_id,
                        principalSchema: "public",
                        principalTable: "libraries",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FileSystemFiles",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    size = table.Column<long>(type: "bigint", nullable: false),
                    parent_directory_id = table.Column<Guid>(type: "uuid", nullable: true),
                    library_id = table.Column<Guid>(type: "uuid", nullable: false),
                    path_info_name = table.Column<string>(type: "text", nullable: false),
                    path_info_full_name = table.Column<string>(type: "text", nullable: false),
                    path_info_extension = table.Column<string>(type: "text", nullable: true),
                    path_info_directory_path = table.Column<string>(type: "text", nullable: true),
                    path_info_directory_separator_char = table.Column<char>(type: "character(1)", nullable: false)
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
                    table.ForeignKey(
                        name: "fk_file_system_entries_libraries_library_id",
                        column: x => x.library_id,
                        principalSchema: "public",
                        principalTable: "libraries",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "audio_streams",
                schema: "public",
                columns: table => new
                {
                    file_system_file_id = table.Column<Guid>(type: "uuid", nullable: false),
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    title = table.Column<string>(type: "text", nullable: true),
                    language = table.Column<string>(type: "text", nullable: true),
                    duration = table.Column<TimeSpan>(type: "interval", nullable: false),
                    bitrate = table.Column<long>(type: "bigint", nullable: false),
                    sample_rate = table.Column<int>(type: "integer", nullable: false),
                    channels = table.Column<int>(type: "integer", nullable: false),
                    index = table.Column<int>(type: "integer", nullable: false),
                    codec = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_audio_streams", x => new { x.file_system_file_id, x.id });
                    table.ForeignKey(
                        name: "fk_audio_streams_file_system_entries_file_system_file_id",
                        column: x => x.file_system_file_id,
                        principalSchema: "public",
                        principalTable: "FileSystemFiles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "subtitle_streams",
                schema: "public",
                columns: table => new
                {
                    file_system_file_id = table.Column<Guid>(type: "uuid", nullable: false),
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    language = table.Column<string>(type: "text", nullable: false),
                    title = table.Column<string>(type: "text", nullable: true),
                    index = table.Column<int>(type: "integer", nullable: false),
                    codec = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_subtitle_streams", x => new { x.file_system_file_id, x.id });
                    table.ForeignKey(
                        name: "fk_subtitle_streams_file_system_entries_file_system_file_id",
                        column: x => x.file_system_file_id,
                        principalSchema: "public",
                        principalTable: "FileSystemFiles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "video_streams",
                schema: "public",
                columns: table => new
                {
                    file_system_file_id = table.Column<Guid>(type: "uuid", nullable: false),
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    duration = table.Column<TimeSpan>(type: "interval", nullable: false),
                    quality_width = table.Column<int>(type: "integer", nullable: false),
                    quality_height = table.Column<int>(type: "integer", nullable: false),
                    quality_aspect_ratio = table.Column<string>(type: "text", nullable: false),
                    quality_resolution = table.Column<int>(type: "integer", nullable: false),
                    framerate = table.Column<double>(type: "double precision", nullable: false),
                    bitrate = table.Column<long>(type: "bigint", nullable: false),
                    pixel_format = table.Column<string>(type: "text", nullable: false),
                    index = table.Column<int>(type: "integer", nullable: false),
                    codec = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_video_streams", x => new { x.file_system_file_id, x.id });
                    table.ForeignKey(
                        name: "fk_video_streams_file_system_entries_file_system_file_id",
                        column: x => x.file_system_file_id,
                        principalSchema: "public",
                        principalTable: "FileSystemFiles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_file_system_directories_library_id",
                schema: "public",
                table: "FileSystemDirectories",
                column: "library_id");

            migrationBuilder.CreateIndex(
                name: "ix_file_system_directories_parent_directory_id",
                schema: "public",
                table: "FileSystemDirectories",
                column: "parent_directory_id");

            migrationBuilder.CreateIndex(
                name: "ix_file_system_files_library_id",
                schema: "public",
                table: "FileSystemFiles",
                column: "library_id");

            migrationBuilder.CreateIndex(
                name: "ix_file_system_files_parent_directory_id",
                schema: "public",
                table: "FileSystemFiles",
                column: "parent_directory_id");

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
                name: "audio_streams",
                schema: "public");

            migrationBuilder.DropTable(
                name: "subtitle_streams",
                schema: "public");

            migrationBuilder.DropTable(
                name: "video_streams",
                schema: "public");

            migrationBuilder.DropTable(
                name: "FileSystemFiles",
                schema: "public");

            migrationBuilder.DropTable(
                name: "FileSystemDirectories",
                schema: "public");

            migrationBuilder.DropTable(
                name: "libraries",
                schema: "public");
        }
    }
}
