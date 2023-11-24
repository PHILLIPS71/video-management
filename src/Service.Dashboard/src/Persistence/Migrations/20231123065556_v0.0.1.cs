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
                name: "dashboard");

            migrationBuilder.CreateTable(
                name: "inbox_state",
                schema: "dashboard",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    message_id = table.Column<Guid>(type: "uuid", nullable: false),
                    consumer_id = table.Column<Guid>(type: "uuid", nullable: false),
                    lock_id = table.Column<Guid>(type: "uuid", nullable: false),
                    row_version = table.Column<byte[]>(type: "bytea", rowVersion: true, nullable: true),
                    received = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    receive_count = table.Column<int>(type: "integer", nullable: false),
                    expiration_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    consumed = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    delivered = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    last_sequence_number = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_inbox_state", x => x.id);
                    table.UniqueConstraint("ak_inbox_state_message_id_consumer_id", x => new { x.message_id, x.consumer_id });
                });

            migrationBuilder.CreateTable(
                name: "libraries",
                schema: "dashboard",
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
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    concurrency_token = table.Column<byte[]>(type: "bytea", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_libraries", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "outbox_message",
                schema: "dashboard",
                columns: table => new
                {
                    sequence_number = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    enqueue_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    sent_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    headers = table.Column<string>(type: "text", nullable: true),
                    properties = table.Column<string>(type: "text", nullable: true),
                    inbox_message_id = table.Column<Guid>(type: "uuid", nullable: true),
                    inbox_consumer_id = table.Column<Guid>(type: "uuid", nullable: true),
                    outbox_id = table.Column<Guid>(type: "uuid", nullable: true),
                    message_id = table.Column<Guid>(type: "uuid", nullable: false),
                    content_type = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    message_type = table.Column<string>(type: "text", nullable: false),
                    body = table.Column<string>(type: "text", nullable: false),
                    conversation_id = table.Column<Guid>(type: "uuid", nullable: true),
                    correlation_id = table.Column<Guid>(type: "uuid", nullable: true),
                    initiator_id = table.Column<Guid>(type: "uuid", nullable: true),
                    request_id = table.Column<Guid>(type: "uuid", nullable: true),
                    source_address = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    destination_address = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    response_address = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    fault_address = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    expiration_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_outbox_message", x => x.sequence_number);
                });

            migrationBuilder.CreateTable(
                name: "outbox_state",
                schema: "dashboard",
                columns: table => new
                {
                    outbox_id = table.Column<Guid>(type: "uuid", nullable: false),
                    lock_id = table.Column<Guid>(type: "uuid", nullable: false),
                    row_version = table.Column<byte[]>(type: "bytea", rowVersion: true, nullable: true),
                    created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    delivered = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    last_sequence_number = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_outbox_state", x => x.outbox_id);
                });

            migrationBuilder.CreateTable(
                name: "transcode_saga_state",
                schema: "dashboard",
                columns: table => new
                {
                    correlation_id = table.Column<Guid>(type: "uuid", nullable: false),
                    current_state = table.Column<string>(type: "text", nullable: false),
                    job_id = table.Column<Guid>(type: "uuid", nullable: true),
                    input_full_path = table.Column<string>(type: "text", nullable: false),
                    output_full_path = table.Column<string>(type: "text", nullable: true),
                    submitted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    row_version = table.Column<byte[]>(type: "bytea", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_transcode_saga_state", x => x.correlation_id);
                });

            migrationBuilder.CreateTable(
                name: "FileSystemDirectories",
                schema: "dashboard",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    library_id = table.Column<Guid>(type: "uuid", nullable: false),
                    parent_directory_id = table.Column<Guid>(type: "uuid", nullable: true),
                    size = table.Column<long>(type: "bigint", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    path_info_name = table.Column<string>(type: "text", nullable: false),
                    path_info_full_name = table.Column<string>(type: "text", nullable: false),
                    path_info_extension = table.Column<string>(type: "text", nullable: true),
                    path_info_directory_path = table.Column<string>(type: "text", nullable: true),
                    path_info_directory_separator_char = table.Column<char>(type: "character(1)", nullable: false),
                    concurrency_token = table.Column<byte[]>(type: "bytea", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileSystemDirectories", x => x.id);
                    table.ForeignKey(
                        name: "fk_file_system_entries_file_system_directories_parent_directory_id",
                        column: x => x.parent_directory_id,
                        principalSchema: "dashboard",
                        principalTable: "FileSystemDirectories",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_file_system_entries_libraries_library_id",
                        column: x => x.library_id,
                        principalSchema: "dashboard",
                        principalTable: "libraries",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FileSystemFiles",
                schema: "dashboard",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    library_id = table.Column<Guid>(type: "uuid", nullable: false),
                    parent_directory_id = table.Column<Guid>(type: "uuid", nullable: true),
                    size = table.Column<long>(type: "bigint", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    path_info_name = table.Column<string>(type: "text", nullable: false),
                    path_info_full_name = table.Column<string>(type: "text", nullable: false),
                    path_info_extension = table.Column<string>(type: "text", nullable: true),
                    path_info_directory_path = table.Column<string>(type: "text", nullable: true),
                    path_info_directory_separator_char = table.Column<char>(type: "character(1)", nullable: false),
                    concurrency_token = table.Column<byte[]>(type: "bytea", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileSystemFiles", x => x.id);
                    table.ForeignKey(
                        name: "fk_file_system_entries_file_system_directories_parent_directory_id",
                        column: x => x.parent_directory_id,
                        principalSchema: "dashboard",
                        principalTable: "FileSystemDirectories",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_file_system_entries_libraries_library_id",
                        column: x => x.library_id,
                        principalSchema: "dashboard",
                        principalTable: "libraries",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "audio_streams",
                schema: "dashboard",
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
                        principalSchema: "dashboard",
                        principalTable: "FileSystemFiles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "subtitle_streams",
                schema: "dashboard",
                columns: table => new
                {
                    file_system_file_id = table.Column<Guid>(type: "uuid", nullable: false),
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    title = table.Column<string>(type: "text", nullable: true),
                    language = table.Column<string>(type: "text", nullable: true),
                    index = table.Column<int>(type: "integer", nullable: false),
                    codec = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_subtitle_streams", x => new { x.file_system_file_id, x.id });
                    table.ForeignKey(
                        name: "fk_subtitle_streams_file_system_entries_file_system_file_id",
                        column: x => x.file_system_file_id,
                        principalSchema: "dashboard",
                        principalTable: "FileSystemFiles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "transcodes",
                schema: "dashboard",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    file_id = table.Column<Guid>(type: "uuid", nullable: false),
                    status = table.Column<string>(type: "text", nullable: false),
                    percent = table.Column<float>(type: "real", nullable: true),
                    speed_frames = table.Column<float>(type: "real", precision: 3, scale: 2, nullable: true),
                    speed_bitrate = table.Column<long>(type: "bigint", nullable: true),
                    speed_scale = table.Column<float>(type: "real", precision: 3, scale: 2, nullable: true),
                    started_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    failed_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    degraded_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    cancelled_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    completed_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    concurrency_token = table.Column<byte[]>(type: "bytea", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_transcodes", x => x.id);
                    table.ForeignKey(
                        name: "fk_transcodes_file_system_entries_file_id",
                        column: x => x.file_id,
                        principalSchema: "dashboard",
                        principalTable: "FileSystemFiles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "video_streams",
                schema: "dashboard",
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
                        principalSchema: "dashboard",
                        principalTable: "FileSystemFiles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_file_system_directories_library_id",
                schema: "dashboard",
                table: "FileSystemDirectories",
                column: "library_id");

            migrationBuilder.CreateIndex(
                name: "ix_file_system_directories_parent_directory_id",
                schema: "dashboard",
                table: "FileSystemDirectories",
                column: "parent_directory_id");

            migrationBuilder.CreateIndex(
                name: "ix_file_system_files_library_id",
                schema: "dashboard",
                table: "FileSystemFiles",
                column: "library_id");

            migrationBuilder.CreateIndex(
                name: "ix_file_system_files_parent_directory_id",
                schema: "dashboard",
                table: "FileSystemFiles",
                column: "parent_directory_id");

            migrationBuilder.CreateIndex(
                name: "ix_inbox_state_delivered",
                schema: "dashboard",
                table: "inbox_state",
                column: "delivered");

            migrationBuilder.CreateIndex(
                name: "ix_libraries_slug",
                schema: "dashboard",
                table: "libraries",
                column: "slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_outbox_message_enqueue_time",
                schema: "dashboard",
                table: "outbox_message",
                column: "enqueue_time");

            migrationBuilder.CreateIndex(
                name: "ix_outbox_message_expiration_time",
                schema: "dashboard",
                table: "outbox_message",
                column: "expiration_time");

            migrationBuilder.CreateIndex(
                name: "ix_outbox_message_inbox_message_id_inbox_consumer_id_sequence_",
                schema: "dashboard",
                table: "outbox_message",
                columns: new[] { "inbox_message_id", "inbox_consumer_id", "sequence_number" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_outbox_message_outbox_id_sequence_number",
                schema: "dashboard",
                table: "outbox_message",
                columns: new[] { "outbox_id", "sequence_number" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_outbox_state_created",
                schema: "dashboard",
                table: "outbox_state",
                column: "created");

            migrationBuilder.CreateIndex(
                name: "ix_transcode_saga_state_job_id",
                schema: "dashboard",
                table: "transcode_saga_state",
                column: "job_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_transcode_saga_state_output_full_path",
                schema: "dashboard",
                table: "transcode_saga_state",
                column: "output_full_path",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_transcodes_file_id",
                schema: "dashboard",
                table: "transcodes",
                column: "file_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "audio_streams",
                schema: "dashboard");

            migrationBuilder.DropTable(
                name: "inbox_state",
                schema: "dashboard");

            migrationBuilder.DropTable(
                name: "outbox_message",
                schema: "dashboard");

            migrationBuilder.DropTable(
                name: "outbox_state",
                schema: "dashboard");

            migrationBuilder.DropTable(
                name: "subtitle_streams",
                schema: "dashboard");

            migrationBuilder.DropTable(
                name: "transcode_saga_state",
                schema: "dashboard");

            migrationBuilder.DropTable(
                name: "transcodes",
                schema: "dashboard");

            migrationBuilder.DropTable(
                name: "video_streams",
                schema: "dashboard");

            migrationBuilder.DropTable(
                name: "FileSystemFiles",
                schema: "dashboard");

            migrationBuilder.DropTable(
                name: "FileSystemDirectories",
                schema: "dashboard");

            migrationBuilder.DropTable(
                name: "libraries",
                schema: "dashboard");
        }
    }
}
