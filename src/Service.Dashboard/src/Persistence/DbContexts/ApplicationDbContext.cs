using Giantnodes.Infrastructure.Domain.Entities;
using Giantnodes.Service.Dashboard.Domain.Entities.Encoding;
using Giantnodes.Service.Dashboard.Domain.Entities.Files;
using Giantnodes.Service.Dashboard.Domain.Entities.Files.Streams;
using Giantnodes.Service.Dashboard.Domain.Entities.Libraries;
using Giantnodes.Service.Dashboard.Domain.Entities.Presets;
using Giantnodes.Service.Dashboard.Domain.Entities.Presets.Streams;
using Giantnodes.Service.Dashboard.Domain.Entities.Probing;
using Giantnodes.Service.Dashboard.Persistence.Sagas.Configurations;
using MassTransit;
using MassTransit.EntityFrameworkCoreIntegration;
using Microsoft.EntityFrameworkCore;

namespace Giantnodes.Service.Dashboard.Persistence.DbContexts;

public class ApplicationDbContext : SagaDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Library> Libraries => Set<Library>();

    public DbSet<FileSystemNode> Nodes => Set<FileSystemNode>();
    public DbSet<FileSystemDirectory> Directories => Set<FileSystemDirectory>();
    public DbSet<FileSystemFile> Files => Set<FileSystemFile>();
    public DbSet<FileSystemFileStream> FileStreams => Set<FileSystemFileStream>();
    public DbSet<FileSystemFileVideoStream> FileVideoStreams => Set<FileSystemFileVideoStream>();
    public DbSet<FileSystemFileAudioStream> FileAudioStreams => Set<FileSystemFileAudioStream>();
    public DbSet<FileSystemFileSubtitleStream> FileSubtitleStreams => Set<FileSystemFileSubtitleStream>();

    public DbSet<Encode> Encodes => Set<Encode>();
    public DbSet<Probe> Probes => Set<Probe>();

    public DbSet<Preset> Presets => Set<Preset>();
    public DbSet<VideoStreamPreset> VideoStreamPresets => Set<VideoStreamPreset>();
    public DbSet<AudioStreamPreset> AudioStreamPresets => Set<AudioStreamPreset>();
    public DbSet<SubtitleStreamPreset> SubtitleStreamPresets => Set<SubtitleStreamPreset>();

    protected override IEnumerable<ISagaClassMap> Configurations
    {
        get
        {
            yield return new JobTypeSagaMap(true);
            yield return new JobSagaMap(true);
            yield return new JobAttemptSagaMap(true);
            yield return new ProbeSagaMap();
            yield return new EncodeSagaMap();
        }
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }

    public override int SaveChanges()
    {
        AddTimestamps();
        NullifyEmptyStrings();
        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellation = default)
    {
        SetEntitySequentialPrimaryKey();
        AddTimestamps();
        NullifyEmptyStrings();
        return base.SaveChangesAsync(cancellation);
    }

    /// <summary>
    /// Sets the <see cref="IEntity.Id" /> to a sequential Guid when the entity is
    /// being adding into the database.
    /// </summary>
    private void SetEntitySequentialPrimaryKey()
    {
        var entities = ChangeTracker.Entries<IEntity>().Where(x => x.State == EntityState.Added);
        foreach (var entry in entities)
        {
            entry.Entity.Id = NewId.NextGuid();
        }
    }

    private void AddTimestamps()
    {
        foreach (var entry in ChangeTracker.Entries<ITimestampableEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedAt = DateTime.UtcNow;
                    break;
                case EntityState.Modified:
                    entry.Entity.UpdatedAt = DateTime.UtcNow;
                    break;
            }
        }
    }

    private void NullifyEmptyStrings()
    {
        foreach (var entity in ChangeTracker.Entries())
        {
            var properties = entity
                .Entity
                .GetType()
                .GetProperties()
                .Where(p => p.PropertyType == typeof(string) && p.CanRead && p.CanWrite);

            foreach (var property in properties)
            {
                if (string.IsNullOrWhiteSpace(property.GetValue(entity.Entity) as string))
                    property.SetValue(entity.Entity, null, null);
            }
        }
    }
}