using Giantnodes.Infrastructure.Domain.Entities;
using Giantnodes.Service.Dashboard.Domain.Entities;
using Giantnodes.Service.Dashboard.Domain.Entities.Files;
using Giantnodes.Service.Dashboard.Domain.Entities.Libraries;
using Microsoft.EntityFrameworkCore;

namespace Giantnodes.Service.Dashboard.Persistence.DbContexts;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Library> Libraries => Set<Library>();

    public DbSet<FileSystemNode> Nodes => Set<FileSystemNode>();
    public DbSet<FileSystemFile> Files => Set<FileSystemFile>();
    public DbSet<FileSystemDirectory> Directories => Set<FileSystemDirectory>();

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
        AddTimestamps();
        NullifyEmptyStrings();
        return base.SaveChangesAsync(cancellation);
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
}