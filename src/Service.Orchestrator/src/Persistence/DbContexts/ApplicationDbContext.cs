using Giantnodes.Infrastructure.EntityFrameworkCore;
using Giantnodes.Service.Orchestrator.Domain.Aggregates.Encodes;
using Giantnodes.Service.Orchestrator.Domain.Aggregates.Encodes.Entities;
using Giantnodes.Service.Orchestrator.Domain.Aggregates.Entries;
using Giantnodes.Service.Orchestrator.Domain.Aggregates.Entries.Directories;
using Giantnodes.Service.Orchestrator.Domain.Aggregates.Entries.Files;
using Giantnodes.Service.Orchestrator.Domain.Aggregates.Libraries;
using Giantnodes.Service.Orchestrator.Domain.Aggregates.Recipes;
using Giantnodes.Service.Orchestrator.Domain.Values;
using Giantnodes.Service.Orchestrator.Persistence.Sagas;
using MassTransit;
using MassTransit.EntityFrameworkCoreIntegration;
using Microsoft.EntityFrameworkCore;

namespace Giantnodes.Service.Orchestrator.Persistence.DbContexts;

public class ApplicationDbContext : GiantnodesDbContext<ApplicationDbContext>
{
    internal const string Schema = "orchestrator";

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

    public DbSet<Recipe> Recipes => Set<Recipe>();

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