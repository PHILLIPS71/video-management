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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.HasDefaultSchema("public");
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
}