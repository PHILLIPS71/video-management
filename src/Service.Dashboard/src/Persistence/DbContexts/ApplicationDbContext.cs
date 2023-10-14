using Giantnodes.Service.Dashboard.Domain.Aggregates.Libraries.Entities;
using Giantnodes.Service.Dashboard.Domain.Aggregates.Libraries.Values;
using Microsoft.EntityFrameworkCore;

namespace Giantnodes.Service.Dashboard.Persistence.DbContexts;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Library> Libraries => Set<Library>();
    public DbSet<FileSystemEntry> FileSystemEntries => Set<FileSystemEntry>();
    public DbSet<FileSystemDirectory> FileSystemDirectories => Set<FileSystemDirectory>();
    public DbSet<FileSystemFile> FileSystemFiles => Set<FileSystemFile>();
    public DbSet<VideoStream> VideoStreams => Set<VideoStream>();
    public DbSet<AudioStream> AudioStreams => Set<AudioStream>();
    public DbSet<SubtitleStream> SubtitleStreams => Set<SubtitleStream>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.HasDefaultSchema("public");
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
}