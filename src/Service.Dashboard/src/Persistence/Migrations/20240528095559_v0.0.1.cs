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
                name: "encode_saga_state",
                schema: "dashboard",
                columns: table => new
                {
                    correlation_id = table.Column<Guid>(type: "uuid", nullable: false),
                    current_state = table.Column<string>(type: "text", nullable: false),
                    encode_id = table.Column<Guid>(type: "uuid", nullable: false),
                    job_id = table.Column<Guid>(type: "uuid", nullable: true),
                    input_file_path = table.Column<string>(type: "text", nullable: false),
                    output_file_path = table.Column<string>(type: "text", nullable: false),
                    is_keeping_source_file = table.Column<bool>(type: "boolean", nullable: false),
                    failed_reason = table.Column<string>(type: "text", nullable: true),
                    submitted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    row_version = table.Column<byte[]>(type: "bytea", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_encode_saga_state", x => x.correlation_id);
                });

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
                    concurrency_token = table.Column<Guid>(type: "uuid", nullable: false)
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
                name: "recipes",
                schema: "dashboard",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    container = table.Column<int>(type: "integer", nullable: true),
                    codec = table.Column<int>(type: "integer", nullable: false),
                    preset = table.Column<int>(type: "integer", nullable: false),
                    tune = table.Column<int>(type: "integer", nullable: true),
                    quality = table.Column<int>(type: "integer", nullable: true),
                    use_hardware_acceleration = table.Column<bool>(type: "boolean", nullable: false),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    concurrency_token = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_recipes", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "file_system_directories",
                schema: "dashboard",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    library_id = table.Column<Guid>(type: "uuid", nullable: false),
                    parent_directory_id = table.Column<Guid>(type: "uuid", nullable: true),
                    size = table.Column<long>(type: "bigint", nullable: false),
                    scanned_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    path_info_name = table.Column<string>(type: "text", nullable: false),
                    path_info_full_name = table.Column<string>(type: "text", nullable: false),
                    path_info_extension = table.Column<string>(type: "text", nullable: true),
                    path_info_directory_path = table.Column<string>(type: "text", nullable: true),
                    path_info_directory_separator_char = table.Column<char>(type: "character(1)", nullable: false),
                    concurrency_token = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_file_system_directories", x => x.id);
                    table.ForeignKey(
                        name: "FK_file_system_directories_file_system_directories_parent_dire~",
                        column: x => x.parent_directory_id,
                        principalSchema: "dashboard",
                        principalTable: "file_system_directories",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_file_system_directories_libraries_library_id",
                        column: x => x.library_id,
                        principalSchema: "dashboard",
                        principalTable: "libraries",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "file_system_files",
                schema: "dashboard",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    library_id = table.Column<Guid>(type: "uuid", nullable: false),
                    parent_directory_id = table.Column<Guid>(type: "uuid", nullable: true),
                    size = table.Column<long>(type: "bigint", nullable: false),
                    scanned_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    probed_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    path_info_name = table.Column<string>(type: "text", nullable: false),
                    path_info_full_name = table.Column<string>(type: "text", nullable: false),
                    path_info_extension = table.Column<string>(type: "text", nullable: true),
                    path_info_directory_path = table.Column<string>(type: "text", nullable: true),
                    path_info_directory_separator_char = table.Column<char>(type: "character(1)", nullable: false),
                    concurrency_token = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_file_system_files", x => x.id);
                    table.ForeignKey(
                        name: "FK_file_system_files_file_system_directories_parent_directory_~",
                        column: x => x.parent_directory_id,
                        principalSchema: "dashboard",
                        principalTable: "file_system_directories",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_file_system_files_libraries_library_id",
                        column: x => x.library_id,
                        principalSchema: "dashboard",
                        principalTable: "libraries",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "encodes",
                schema: "dashboard",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    file_id = table.Column<Guid>(type: "uuid", nullable: false),
                    recipe_id = table.Column<Guid>(type: "uuid", nullable: false),
                    speed_frames = table.Column<float>(type: "real", nullable: true),
                    speed_bitrate = table.Column<long>(type: "bigint", nullable: true),
                    speed_scale = table.Column<float>(type: "real", nullable: true),
                    status = table.Column<string>(type: "text", nullable: false),
                    machine_name = table.Column<string>(type: "text", nullable: true),
                    machine_user_name = table.Column<string>(type: "text", nullable: true),
                    machine_processor_type = table.Column<string>(type: "text", nullable: true),
                    percent = table.Column<float>(type: "real", precision: 3, scale: 2, nullable: true),
                    command = table.Column<string>(type: "text", nullable: true),
                    output = table.Column<string>(type: "text", nullable: true),
                    started_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    failed_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    failure_reason = table.Column<string>(type: "text", nullable: true),
                    degraded_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    cancelled_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    completed_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    concurrency_token = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_encodes", x => x.id);
                    table.ForeignKey(
                        name: "fk_encodes_file_system_files_file_id",
                        column: x => x.file_id,
                        principalSchema: "dashboard",
                        principalTable: "file_system_files",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_encodes_recipes_recipe_id",
                        column: x => x.recipe_id,
                        principalSchema: "dashboard",
                        principalTable: "recipes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "file_system_files_audio_streams",
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
                    table.PrimaryKey("pk_file_system_files_audio_streams", x => new { x.file_system_file_id, x.id });
                    table.ForeignKey(
                        name: "fk_file_system_files_audio_streams_file_system_files_file_syst",
                        column: x => x.file_system_file_id,
                        principalSchema: "dashboard",
                        principalTable: "file_system_files",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "file_system_files_subtitle_streams",
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
                    table.PrimaryKey("pk_file_system_files_subtitle_streams", x => new { x.file_system_file_id, x.id });
                    table.ForeignKey(
                        name: "fk_file_system_files_subtitle_streams_file_system_files_file_s",
                        column: x => x.file_system_file_id,
                        principalSchema: "dashboard",
                        principalTable: "file_system_files",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "file_system_files_video_streams",
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
                    table.PrimaryKey("pk_file_system_files_video_streams", x => new { x.file_system_file_id, x.id });
                    table.ForeignKey(
                        name: "fk_file_system_files_video_streams_file_system_files_file_syst",
                        column: x => x.file_system_file_id,
                        principalSchema: "dashboard",
                        principalTable: "file_system_files",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "encode_snapshots",
                schema: "dashboard",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    encode_id = table.Column<Guid>(type: "uuid", nullable: false),
                    size = table.Column<long>(type: "bigint", nullable: false),
                    probed_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_encode_snapshots", x => x.id);
                    table.ForeignKey(
                        name: "fk_encode_snapshots_encodes_encode_id",
                        column: x => x.encode_id,
                        principalSchema: "dashboard",
                        principalTable: "encodes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "encode_snapshots_audio_streams",
                schema: "dashboard",
                columns: table => new
                {
                    encode_snapshot_id = table.Column<Guid>(type: "uuid", nullable: false),
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
                    table.PrimaryKey("pk_encode_snapshots_audio_streams", x => new { x.encode_snapshot_id, x.id });
                    table.ForeignKey(
                        name: "fk_encode_snapshots_audio_streams_encode_snapshots_encode_snap",
                        column: x => x.encode_snapshot_id,
                        principalSchema: "dashboard",
                        principalTable: "encode_snapshots",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "encode_snapshots_subtitle_streams",
                schema: "dashboard",
                columns: table => new
                {
                    encode_snapshot_id = table.Column<Guid>(type: "uuid", nullable: false),
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    title = table.Column<string>(type: "text", nullable: true),
                    language = table.Column<string>(type: "text", nullable: true),
                    index = table.Column<int>(type: "integer", nullable: false),
                    codec = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_encode_snapshots_subtitle_streams", x => new { x.encode_snapshot_id, x.id });
                    table.ForeignKey(
                        name: "fk_encode_snapshots_subtitle_streams_encode_snapshots_encode_s",
                        column: x => x.encode_snapshot_id,
                        principalSchema: "dashboard",
                        principalTable: "encode_snapshots",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "encode_snapshots_video_streams",
                schema: "dashboard",
                columns: table => new
                {
                    encode_snapshot_id = table.Column<Guid>(type: "uuid", nullable: false),
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
                    table.PrimaryKey("pk_encode_snapshots_video_streams", x => new { x.encode_snapshot_id, x.id });
                    table.ForeignKey(
                        name: "fk_encode_snapshots_video_streams_encode_snapshots_encode_snap",
                        column: x => x.encode_snapshot_id,
                        principalSchema: "dashboard",
                        principalTable: "encode_snapshots",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_encode_saga_state_input_file_path",
                schema: "dashboard",
                table: "encode_saga_state",
                column: "input_file_path",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_encode_saga_state_output_file_path",
                schema: "dashboard",
                table: "encode_saga_state",
                column: "output_file_path",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_encode_snapshots_encode_id",
                schema: "dashboard",
                table: "encode_snapshots",
                column: "encode_id");

            migrationBuilder.CreateIndex(
                name: "ix_encodes_file_id",
                schema: "dashboard",
                table: "encodes",
                column: "file_id");

            migrationBuilder.CreateIndex(
                name: "ix_encodes_recipe_id",
                schema: "dashboard",
                table: "encodes",
                column: "recipe_id");

            migrationBuilder.CreateIndex(
                name: "IX_file_system_directories_library_id",
                schema: "dashboard",
                table: "file_system_directories",
                column: "library_id");

            migrationBuilder.CreateIndex(
                name: "IX_file_system_directories_parent_directory_id",
                schema: "dashboard",
                table: "file_system_directories",
                column: "parent_directory_id");

            migrationBuilder.CreateIndex(
                name: "IX_file_system_files_library_id",
                schema: "dashboard",
                table: "file_system_files",
                column: "library_id");

            migrationBuilder.CreateIndex(
                name: "IX_file_system_files_parent_directory_id",
                schema: "dashboard",
                table: "file_system_files",
                column: "parent_directory_id");

            migrationBuilder.CreateIndex(
                name: "ix_inbox_state_delivered",
                schema: "dashboard",
                table: "inbox_state",
                column: "delivered");

            migrationBuilder.CreateIndex(
                name: "ix_libraries_name",
                schema: "dashboard",
                table: "libraries",
                column: "name",
                unique: true);

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
                name: "ix_recipes_name",
                schema: "dashboard",
                table: "recipes",
                column: "name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "encode_saga_state",
                schema: "dashboard");

            migrationBuilder.DropTable(
                name: "encode_snapshots_audio_streams",
                schema: "dashboard");

            migrationBuilder.DropTable(
                name: "encode_snapshots_subtitle_streams",
                schema: "dashboard");

            migrationBuilder.DropTable(
                name: "encode_snapshots_video_streams",
                schema: "dashboard");

            migrationBuilder.DropTable(
                name: "file_system_files_audio_streams",
                schema: "dashboard");

            migrationBuilder.DropTable(
                name: "file_system_files_subtitle_streams",
                schema: "dashboard");

            migrationBuilder.DropTable(
                name: "file_system_files_video_streams",
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
                name: "encode_snapshots",
                schema: "dashboard");

            migrationBuilder.DropTable(
                name: "encodes",
                schema: "dashboard");

            migrationBuilder.DropTable(
                name: "file_system_files",
                schema: "dashboard");

            migrationBuilder.DropTable(
                name: "recipes",
                schema: "dashboard");

            migrationBuilder.DropTable(
                name: "file_system_directories",
                schema: "dashboard");

            migrationBuilder.DropTable(
                name: "libraries",
                schema: "dashboard");
        }
    }
}
