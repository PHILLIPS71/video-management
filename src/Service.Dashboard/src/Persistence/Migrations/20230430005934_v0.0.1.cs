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
                name: "job_attempt_saga",
                columns: table => new
                {
                    correlation_id = table.Column<Guid>(type: "uuid", nullable: false),
                    current_state = table.Column<int>(type: "integer", nullable: false),
                    job_id = table.Column<Guid>(type: "uuid", nullable: false),
                    retry_attempt = table.Column<int>(type: "integer", nullable: false),
                    service_address = table.Column<string>(type: "text", nullable: true),
                    instance_address = table.Column<string>(type: "text", nullable: true),
                    started = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    faulted = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    status_check_token_id = table.Column<Guid>(type: "uuid", nullable: true),
                    row_version = table.Column<byte[]>(type: "bytea", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_job_attempt_saga", x => x.correlation_id);
                });

            migrationBuilder.CreateTable(
                name: "job_saga",
                columns: table => new
                {
                    correlation_id = table.Column<Guid>(type: "uuid", nullable: false),
                    current_state = table.Column<int>(type: "integer", nullable: false),
                    submitted = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    service_address = table.Column<string>(type: "text", nullable: true),
                    job_timeout = table.Column<TimeSpan>(type: "interval", nullable: true),
                    job = table.Column<string>(type: "text", nullable: true),
                    job_type_id = table.Column<Guid>(type: "uuid", nullable: false),
                    attempt_id = table.Column<Guid>(type: "uuid", nullable: false),
                    retry_attempt = table.Column<int>(type: "integer", nullable: false),
                    started = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    completed = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    duration = table.Column<TimeSpan>(type: "interval", nullable: true),
                    faulted = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    reason = table.Column<string>(type: "text", nullable: true),
                    job_slot_wait_token = table.Column<Guid>(type: "uuid", nullable: true),
                    job_retry_delay_token = table.Column<Guid>(type: "uuid", nullable: true),
                    row_version = table.Column<byte[]>(type: "bytea", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_job_saga", x => x.correlation_id);
                });

            migrationBuilder.CreateTable(
                name: "job_type_saga",
                columns: table => new
                {
                    correlation_id = table.Column<Guid>(type: "uuid", nullable: false),
                    current_state = table.Column<int>(type: "integer", nullable: false),
                    active_job_count = table.Column<int>(type: "integer", nullable: false),
                    concurrent_job_limit = table.Column<int>(type: "integer", nullable: false),
                    override_job_limit = table.Column<int>(type: "integer", nullable: true),
                    override_limit_expiration = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    active_jobs = table.Column<string>(type: "text", nullable: true),
                    instances = table.Column<string>(type: "text", nullable: true),
                    row_version = table.Column<byte[]>(type: "bytea", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_job_type_saga", x => x.correlation_id);
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
                name: "probe_saga",
                columns: table => new
                {
                    correlation_id = table.Column<Guid>(type: "uuid", nullable: false),
                    current_state = table.Column<string>(type: "text", nullable: false),
                    full_path = table.Column<string>(type: "text", nullable: false),
                    job_id = table.Column<Guid>(type: "uuid", nullable: false),
                    submitted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    started_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    cancelled_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    row_version = table.Column<byte[]>(type: "bytea", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_probe_saga", x => x.correlation_id);
                });

            migrationBuilder.CreateTable(
                name: "probes",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    full_path = table.Column<string>(type: "text", nullable: false),
                    status = table.Column<string>(type: "text", nullable: false),
                    started_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    completed_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    cancelled_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    failed_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    failed_reason = table.Column<string>(type: "text", nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_probes", x => x.id);
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
                    size = table.Column<long>(type: "bigint", nullable: false),
                    probed_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
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

            migrationBuilder.CreateTable(
                name: "FileAudioStreams",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    file_system_file_id = table.Column<Guid>(type: "uuid", nullable: false),
                    index = table.Column<int>(type: "integer", nullable: false),
                    codec = table.Column<string>(type: "text", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    title = table.Column<string>(type: "text", nullable: true),
                    language = table.Column<string>(type: "text", nullable: true),
                    duration = table.Column<TimeSpan>(type: "interval", nullable: false),
                    bitrate = table.Column<long>(type: "bigint", nullable: false),
                    sample_rate = table.Column<int>(type: "integer", nullable: false),
                    channels = table.Column<int>(type: "integer", nullable: false),
                    @default = table.Column<bool>(name: "default", type: "boolean", nullable: false),
                    forced = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileAudioStreams", x => x.id);
                    table.ForeignKey(
                        name: "fk_file_audio_streams_files_file_system_file_id",
                        column: x => x.file_system_file_id,
                        principalTable: "Files",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FileSubtitleStreams",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    file_system_file_id = table.Column<Guid>(type: "uuid", nullable: false),
                    index = table.Column<int>(type: "integer", nullable: false),
                    codec = table.Column<string>(type: "text", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    title = table.Column<string>(type: "text", nullable: true),
                    language = table.Column<string>(type: "text", nullable: false),
                    @default = table.Column<bool>(name: "default", type: "boolean", nullable: false),
                    forced = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileSubtitleStreams", x => x.id);
                    table.ForeignKey(
                        name: "fk_file_subtitle_streams_files_file_system_file_id",
                        column: x => x.file_system_file_id,
                        principalTable: "Files",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FileVideoStreams",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    file_system_file_id = table.Column<Guid>(type: "uuid", nullable: false),
                    index = table.Column<int>(type: "integer", nullable: false),
                    codec = table.Column<string>(type: "text", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    bitrate = table.Column<long>(type: "bigint", nullable: false),
                    framerate = table.Column<double>(type: "double precision", nullable: false),
                    pixel_format = table.Column<string>(type: "text", nullable: false),
                    height = table.Column<int>(type: "integer", nullable: false),
                    width = table.Column<int>(type: "integer", nullable: false),
                    duration = table.Column<TimeSpan>(type: "interval", nullable: false),
                    ratio = table.Column<string>(type: "text", nullable: false),
                    rotation = table.Column<int>(type: "integer", nullable: true),
                    @default = table.Column<bool>(name: "default", type: "boolean", nullable: false),
                    forced = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileVideoStreams", x => x.id);
                    table.ForeignKey(
                        name: "fk_file_video_streams_files_file_system_file_id",
                        column: x => x.file_system_file_id,
                        principalTable: "Files",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
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
                name: "ix_file_audio_streams_file_system_file_id",
                table: "FileAudioStreams",
                column: "file_system_file_id");

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
                name: "ix_file_subtitle_streams_file_system_file_id",
                table: "FileSubtitleStreams",
                column: "file_system_file_id");

            migrationBuilder.CreateIndex(
                name: "ix_file_video_streams_file_system_file_id",
                table: "FileVideoStreams",
                column: "file_system_file_id");

            migrationBuilder.CreateIndex(
                name: "ix_job_attempt_saga_job_id_retry_attempt",
                table: "job_attempt_saga",
                columns: new[] { "job_id", "retry_attempt" },
                unique: true);

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

            migrationBuilder.CreateIndex(
                name: "ix_probe_saga_full_path",
                table: "probe_saga",
                column: "full_path",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_probe_saga_job_id",
                table: "probe_saga",
                column: "job_id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FileAudioStreams");

            migrationBuilder.DropTable(
                name: "FileSubtitleStreams");

            migrationBuilder.DropTable(
                name: "FileVideoStreams");

            migrationBuilder.DropTable(
                name: "job_attempt_saga");

            migrationBuilder.DropTable(
                name: "job_saga");

            migrationBuilder.DropTable(
                name: "job_type_saga");

            migrationBuilder.DropTable(
                name: "libraries");

            migrationBuilder.DropTable(
                name: "probe_saga");

            migrationBuilder.DropTable(
                name: "probes");

            migrationBuilder.DropTable(
                name: "Files");

            migrationBuilder.DropTable(
                name: "Directories");
        }
    }
}
