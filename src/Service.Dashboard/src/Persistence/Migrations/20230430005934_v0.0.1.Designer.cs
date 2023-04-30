﻿// <auto-generated />
using System;
using Giantnodes.Service.Dashboard.Persistence.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Giantnodes.Service.Dashboard.Persistence.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20230430005934_v0.0.1")]
    partial class v001
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Giantnodes.Service.Dashboard.Domain.Entities.Files.FileSystemNode", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.Property<string>("FullPath")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("full_path");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.Property<Guid?>("ParentDirectoryId")
                        .HasColumnType("uuid")
                        .HasColumnName("parent_directory_id");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_at");

                    b.HasKey("Id");

                    b.HasIndex("ParentDirectoryId")
                        .HasDatabaseName("ix_nodes_parent_directory_id");

                    b.ToTable((string)null);

                    b.UseTpcMappingStrategy();
                });

            modelBuilder.Entity("Giantnodes.Service.Dashboard.Domain.Entities.Files.Streams.FileSystemFileStream", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("Codec")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("codec");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.Property<Guid>("FileSystemFileId")
                        .HasColumnType("uuid")
                        .HasColumnName("file_system_file_id");

                    b.Property<int>("Index")
                        .HasColumnType("integer")
                        .HasColumnName("index");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_at");

                    b.HasKey("Id");

                    b.ToTable((string)null);

                    b.UseTpcMappingStrategy();
                });

            modelBuilder.Entity("Giantnodes.Service.Dashboard.Domain.Entities.Libraries.Library", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.Property<string>("DriveStatus")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("drive_status");

                    b.Property<string>("FullPath")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("full_path");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.Property<string>("Slug")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("slug");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_at");

                    b.HasKey("Id")
                        .HasName("pk_libraries");

                    b.HasIndex("FullPath")
                        .IsUnique()
                        .HasDatabaseName("ix_libraries_full_path");

                    b.HasIndex("Slug")
                        .IsUnique()
                        .HasDatabaseName("ix_libraries_slug");

                    b.ToTable("libraries", (string)null);
                });

            modelBuilder.Entity("Giantnodes.Service.Dashboard.Domain.Entities.Probing.Probe", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<DateTime?>("CancelledAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("cancelled_at");

                    b.Property<DateTime?>("CompletedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("completed_at");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.Property<DateTime?>("FailedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("failed_at");

                    b.Property<string>("FailedReason")
                        .HasColumnType("text")
                        .HasColumnName("failed_reason");

                    b.Property<string>("FullPath")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("full_path");

                    b.Property<DateTime?>("StartedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("started_at");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("status");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_at");

                    b.HasKey("Id")
                        .HasName("pk_probes");

                    b.ToTable("probes", (string)null);
                });

            modelBuilder.Entity("Giantnodes.Service.Dashboard.Persistence.Sagas.ProbeSaga", b =>
                {
                    b.Property<Guid>("CorrelationId")
                        .HasColumnType("uuid")
                        .HasColumnName("correlation_id");

                    b.Property<DateTime?>("CancelledAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("cancelled_at");

                    b.Property<string>("CurrentState")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("current_state");

                    b.Property<string>("FullPath")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("full_path");

                    b.Property<Guid>("JobId")
                        .HasColumnType("uuid")
                        .HasColumnName("job_id");

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("bytea")
                        .HasColumnName("row_version");

                    b.Property<DateTime?>("StartedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("started_at");

                    b.Property<DateTime>("SubmittedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("submitted_at");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_at");

                    b.HasKey("CorrelationId")
                        .HasName("pk_probe_saga");

                    b.HasIndex("FullPath")
                        .IsUnique()
                        .HasDatabaseName("ix_probe_saga_full_path");

                    b.HasIndex("JobId")
                        .IsUnique()
                        .HasDatabaseName("ix_probe_saga_job_id");

                    b.ToTable("probe_saga", (string)null);
                });

            modelBuilder.Entity("MassTransit.JobAttemptSaga", b =>
                {
                    b.Property<Guid>("CorrelationId")
                        .HasColumnType("uuid")
                        .HasColumnName("correlation_id");

                    b.Property<int>("CurrentState")
                        .HasColumnType("integer")
                        .HasColumnName("current_state");

                    b.Property<DateTime?>("Faulted")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("faulted");

                    b.Property<string>("InstanceAddress")
                        .HasColumnType("text")
                        .HasColumnName("instance_address");

                    b.Property<Guid>("JobId")
                        .HasColumnType("uuid")
                        .HasColumnName("job_id");

                    b.Property<int>("RetryAttempt")
                        .HasColumnType("integer")
                        .HasColumnName("retry_attempt");

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("bytea")
                        .HasColumnName("row_version");

                    b.Property<string>("ServiceAddress")
                        .HasColumnType("text")
                        .HasColumnName("service_address");

                    b.Property<DateTime?>("Started")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("started");

                    b.Property<Guid?>("StatusCheckTokenId")
                        .HasColumnType("uuid")
                        .HasColumnName("status_check_token_id");

                    b.HasKey("CorrelationId")
                        .HasName("pk_job_attempt_saga");

                    b.HasIndex("JobId", "RetryAttempt")
                        .IsUnique()
                        .HasDatabaseName("ix_job_attempt_saga_job_id_retry_attempt");

                    b.ToTable("job_attempt_saga", (string)null);
                });

            modelBuilder.Entity("MassTransit.JobSaga", b =>
                {
                    b.Property<Guid>("CorrelationId")
                        .HasColumnType("uuid")
                        .HasColumnName("correlation_id");

                    b.Property<Guid>("AttemptId")
                        .HasColumnType("uuid")
                        .HasColumnName("attempt_id");

                    b.Property<DateTime?>("Completed")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("completed");

                    b.Property<int>("CurrentState")
                        .HasColumnType("integer")
                        .HasColumnName("current_state");

                    b.Property<TimeSpan?>("Duration")
                        .HasColumnType("interval")
                        .HasColumnName("duration");

                    b.Property<DateTime?>("Faulted")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("faulted");

                    b.Property<string>("Job")
                        .HasColumnType("text")
                        .HasColumnName("job");

                    b.Property<Guid?>("JobRetryDelayToken")
                        .HasColumnType("uuid")
                        .HasColumnName("job_retry_delay_token");

                    b.Property<Guid?>("JobSlotWaitToken")
                        .HasColumnType("uuid")
                        .HasColumnName("job_slot_wait_token");

                    b.Property<TimeSpan?>("JobTimeout")
                        .HasColumnType("interval")
                        .HasColumnName("job_timeout");

                    b.Property<Guid>("JobTypeId")
                        .HasColumnType("uuid")
                        .HasColumnName("job_type_id");

                    b.Property<string>("Reason")
                        .HasColumnType("text")
                        .HasColumnName("reason");

                    b.Property<int>("RetryAttempt")
                        .HasColumnType("integer")
                        .HasColumnName("retry_attempt");

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("bytea")
                        .HasColumnName("row_version");

                    b.Property<string>("ServiceAddress")
                        .HasColumnType("text")
                        .HasColumnName("service_address");

                    b.Property<DateTime?>("Started")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("started");

                    b.Property<DateTime?>("Submitted")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("submitted");

                    b.HasKey("CorrelationId")
                        .HasName("pk_job_saga");

                    b.ToTable("job_saga", (string)null);
                });

            modelBuilder.Entity("MassTransit.JobTypeSaga", b =>
                {
                    b.Property<Guid>("CorrelationId")
                        .HasColumnType("uuid")
                        .HasColumnName("correlation_id");

                    b.Property<int>("ActiveJobCount")
                        .HasColumnType("integer")
                        .HasColumnName("active_job_count");

                    b.Property<string>("ActiveJobs")
                        .HasColumnType("text")
                        .HasColumnName("active_jobs");

                    b.Property<int>("ConcurrentJobLimit")
                        .HasColumnType("integer")
                        .HasColumnName("concurrent_job_limit");

                    b.Property<int>("CurrentState")
                        .HasColumnType("integer")
                        .HasColumnName("current_state");

                    b.Property<string>("Instances")
                        .HasColumnType("text")
                        .HasColumnName("instances");

                    b.Property<int?>("OverrideJobLimit")
                        .HasColumnType("integer")
                        .HasColumnName("override_job_limit");

                    b.Property<DateTime?>("OverrideLimitExpiration")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("override_limit_expiration");

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("bytea")
                        .HasColumnName("row_version");

                    b.HasKey("CorrelationId")
                        .HasName("pk_job_type_saga");

                    b.ToTable("job_type_saga", (string)null);
                });

            modelBuilder.Entity("Giantnodes.Service.Dashboard.Domain.Entities.Files.FileSystemDirectory", b =>
                {
                    b.HasBaseType("Giantnodes.Service.Dashboard.Domain.Entities.Files.FileSystemNode");

                    b.HasIndex("FullPath")
                        .IsUnique()
                        .HasDatabaseName("ix_directories_full_path");

                    b.ToTable("Directories");
                });

            modelBuilder.Entity("Giantnodes.Service.Dashboard.Domain.Entities.Files.FileSystemFile", b =>
                {
                    b.HasBaseType("Giantnodes.Service.Dashboard.Domain.Entities.Files.FileSystemNode");

                    b.Property<DateTime?>("ProbedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("probed_at");

                    b.Property<long>("Size")
                        .HasColumnType("bigint")
                        .HasColumnName("size");

                    b.HasIndex("FullPath")
                        .IsUnique()
                        .HasDatabaseName("ix_files_full_path");

                    b.ToTable("Files");
                });

            modelBuilder.Entity("Giantnodes.Service.Dashboard.Domain.Entities.Files.Streams.FileSystemFileAudioStream", b =>
                {
                    b.HasBaseType("Giantnodes.Service.Dashboard.Domain.Entities.Files.Streams.FileSystemFileStream");

                    b.Property<long>("Bitrate")
                        .HasColumnType("bigint")
                        .HasColumnName("bitrate");

                    b.Property<int>("Channels")
                        .HasColumnType("integer")
                        .HasColumnName("channels");

                    b.Property<bool>("Default")
                        .HasColumnType("boolean")
                        .HasColumnName("default");

                    b.Property<TimeSpan>("Duration")
                        .HasColumnType("interval")
                        .HasColumnName("duration");

                    b.Property<bool>("Forced")
                        .HasColumnType("boolean")
                        .HasColumnName("forced");

                    b.Property<string>("Language")
                        .HasColumnType("text")
                        .HasColumnName("language");

                    b.Property<int>("SampleRate")
                        .HasColumnType("integer")
                        .HasColumnName("sample_rate");

                    b.Property<string>("Title")
                        .HasColumnType("text")
                        .HasColumnName("title");

                    b.HasIndex("FileSystemFileId")
                        .HasDatabaseName("ix_file_audio_streams_file_system_file_id");

                    b.ToTable("FileAudioStreams");
                });

            modelBuilder.Entity("Giantnodes.Service.Dashboard.Domain.Entities.Files.Streams.FileSystemFileSubtitleStream", b =>
                {
                    b.HasBaseType("Giantnodes.Service.Dashboard.Domain.Entities.Files.Streams.FileSystemFileStream");

                    b.Property<bool>("Default")
                        .HasColumnType("boolean")
                        .HasColumnName("default");

                    b.Property<bool>("Forced")
                        .HasColumnType("boolean")
                        .HasColumnName("forced");

                    b.Property<string>("Language")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("language");

                    b.Property<string>("Title")
                        .HasColumnType("text")
                        .HasColumnName("title");

                    b.HasIndex("FileSystemFileId")
                        .HasDatabaseName("ix_file_subtitle_streams_file_system_file_id");

                    b.ToTable("FileSubtitleStreams");
                });

            modelBuilder.Entity("Giantnodes.Service.Dashboard.Domain.Entities.Files.Streams.FileSystemFileVideoStream", b =>
                {
                    b.HasBaseType("Giantnodes.Service.Dashboard.Domain.Entities.Files.Streams.FileSystemFileStream");

                    b.Property<long>("Bitrate")
                        .HasColumnType("bigint")
                        .HasColumnName("bitrate");

                    b.Property<bool>("Default")
                        .HasColumnType("boolean")
                        .HasColumnName("default");

                    b.Property<TimeSpan>("Duration")
                        .HasColumnType("interval")
                        .HasColumnName("duration");

                    b.Property<bool>("Forced")
                        .HasColumnType("boolean")
                        .HasColumnName("forced");

                    b.Property<double>("Framerate")
                        .HasColumnType("double precision")
                        .HasColumnName("framerate");

                    b.Property<int>("Height")
                        .HasColumnType("integer")
                        .HasColumnName("height");

                    b.Property<string>("PixelFormat")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("pixel_format");

                    b.Property<string>("Ratio")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("ratio");

                    b.Property<int?>("Rotation")
                        .HasColumnType("integer")
                        .HasColumnName("rotation");

                    b.Property<int>("Width")
                        .HasColumnType("integer")
                        .HasColumnName("width");

                    b.HasIndex("FileSystemFileId")
                        .HasDatabaseName("ix_file_video_streams_file_system_file_id");

                    b.ToTable("FileVideoStreams");
                });

            modelBuilder.Entity("Giantnodes.Service.Dashboard.Domain.Entities.Files.FileSystemNode", b =>
                {
                    b.HasOne("Giantnodes.Service.Dashboard.Domain.Entities.Files.FileSystemDirectory", "ParentDirectory")
                        .WithMany("Nodes")
                        .HasForeignKey("ParentDirectoryId")
                        .HasConstraintName("fk_nodes_directories_parent_directory_id");

                    b.Navigation("ParentDirectory");
                });

            modelBuilder.Entity("Giantnodes.Service.Dashboard.Domain.Entities.Files.Streams.FileSystemFileAudioStream", b =>
                {
                    b.HasOne("Giantnodes.Service.Dashboard.Domain.Entities.Files.FileSystemFile", "FileSystemFile")
                        .WithMany("AudioStreams")
                        .HasForeignKey("FileSystemFileId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_file_audio_streams_files_file_system_file_id");

                    b.Navigation("FileSystemFile");
                });

            modelBuilder.Entity("Giantnodes.Service.Dashboard.Domain.Entities.Files.Streams.FileSystemFileSubtitleStream", b =>
                {
                    b.HasOne("Giantnodes.Service.Dashboard.Domain.Entities.Files.FileSystemFile", "FileSystemFile")
                        .WithMany("SubtitleStreams")
                        .HasForeignKey("FileSystemFileId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_file_subtitle_streams_files_file_system_file_id");

                    b.Navigation("FileSystemFile");
                });

            modelBuilder.Entity("Giantnodes.Service.Dashboard.Domain.Entities.Files.Streams.FileSystemFileVideoStream", b =>
                {
                    b.HasOne("Giantnodes.Service.Dashboard.Domain.Entities.Files.FileSystemFile", "FileSystemFile")
                        .WithMany("VideoStreams")
                        .HasForeignKey("FileSystemFileId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_file_video_streams_files_file_system_file_id");

                    b.Navigation("FileSystemFile");
                });

            modelBuilder.Entity("Giantnodes.Service.Dashboard.Domain.Entities.Files.FileSystemDirectory", b =>
                {
                    b.Navigation("Nodes");
                });

            modelBuilder.Entity("Giantnodes.Service.Dashboard.Domain.Entities.Files.FileSystemFile", b =>
                {
                    b.Navigation("AudioStreams");

                    b.Navigation("SubtitleStreams");

                    b.Navigation("VideoStreams");
                });
#pragma warning restore 612, 618
        }
    }
}
