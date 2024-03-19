using Giantnodes.Infrastructure.EntityFrameworkCore;
using Giantnodes.Service.Dashboard.Domain.Aggregates.EncodeProfiles;
using Giantnodes.Service.Dashboard.Domain.Aggregates.Encodes;
using Giantnodes.Service.Dashboard.Domain.Aggregates.Encodes.Entities;
using Giantnodes.Service.Dashboard.Domain.Aggregates.Entries;
using Giantnodes.Service.Dashboard.Domain.Aggregates.Entries.Directories;
using Giantnodes.Service.Dashboard.Domain.Aggregates.Entries.Files;
using Giantnodes.Service.Dashboard.Domain.Aggregates.Libraries;
using Giantnodes.Service.Dashboard.Domain.Values;
using Giantnodes.Service.Dashboard.Persistence.Sagas;
using MassTransit;
using MassTransit.EntityFrameworkCoreIntegration;
using Microsoft.EntityFrameworkCore;

namespace Giantnodes.Service.Dashboard.Persistence.DbContexts;

public class ApplicationDbContext : GiantnodesDbContext<ApplicationDbContext>
{
    internal const string Schema = "dashboard";

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Library> Libraries => Set<Library>();

    public DbSet<FileSystemEntry> FileSystemEntries => Set<FileSystemEntry>();
    public DbSet<FileSystemDirectory> FileSystemDirectories => Set<FileSystemDirectory>();
    public DbSet<FileSystemFile> FileSystemFiles => Set<FileSystemFile>();

    public DbSet<Encode> Encodes => Set<Encode>();
    public DbSet<EncodeSnapshot> EncodeSnapshots => Set<EncodeSnapshot>();

    public DbSet<EncodeProfile> EncodeProfiles => Set<EncodeProfile>();

    public DbSet<VideoStream> VideoStreams => Set<VideoStream>();
    public DbSet<AudioStream> AudioStreams => Set<AudioStream>();
    public DbSet<SubtitleStream> SubtitleStreams => Set<SubtitleStream>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        foreach (var configuration in Configurations)
            configuration.Configure(modelBuilder);

        modelBuilder.AddTransactionalOutboxEntities();

        modelBuilder.HasDefaultSchema(Schema);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }

    private static IEnumerable<ISagaClassMap> Configurations
    {
        get { yield return new EncodeSagaStateMap(); }
    }
}