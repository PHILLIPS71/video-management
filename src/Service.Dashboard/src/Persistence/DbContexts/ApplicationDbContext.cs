using Giantnodes.Service.Dashboard.Domain.Aggregates.Libraries.Entities;
using Microsoft.EntityFrameworkCore;

namespace Giantnodes.Service.Dashboard.Persistence.DbContexts;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Library> Libraries => Set<Library>();
    protected DbSet<FileSystemEntry> FileSystemEntries => Set<FileSystemEntry>();
    protected DbSet<FileSystemDirectory> FileSystemDirectories => Set<FileSystemDirectory>();
    protected DbSet<FileSystemFile> FileSystemFiles => Set<FileSystemFile>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.HasDefaultSchema("public");
        builder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
}