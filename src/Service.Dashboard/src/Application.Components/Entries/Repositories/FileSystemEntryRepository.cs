using System.Linq.Expressions;
using Giantnodes.Service.Dashboard.Domain.Aggregates.Entries;
using Giantnodes.Service.Dashboard.Domain.Aggregates.Entries.Directories;
using Giantnodes.Service.Dashboard.Domain.Aggregates.Entries.Files;
using Giantnodes.Service.Dashboard.Domain.Aggregates.Entries.Repositories;
using Giantnodes.Service.Dashboard.Persistence.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace Giantnodes.Service.Dashboard.Application.Components.Entries.Repositories;

public class FileSystemEntryRepository : IFileSystemEntryRepository
{
    private readonly ApplicationDbContext _database;

    public FileSystemEntryRepository(ApplicationDbContext database)
    {
        _database = database;
    }

    /// <summary>
    /// Builds the <see name="FileSystemEntry"/> aggregates consistency boundary.
    /// </summary>
    /// <returns>A <see cref="IQueryable{TEntity}"/> of the objects that make up the consistency boundary.</returns>
    private IQueryable<FileSystemEntry> Build()
    {
        return _database
            .FileSystemEntries
            .Include(x => x.Library)
            .Include(x => x.ParentDirectory)
            .Include(x => (x as FileSystemDirectory).Entries)
            .Include(x => (x as FileSystemFile).Encodes)
            .Include(x => (x as FileSystemFile).VideoStreams)
            .Include(x => (x as FileSystemFile).AudioStreams)
            .Include(x => (x as FileSystemFile).SubtitleStreams)
            .AsQueryable();
    }

    public Task<bool> ExistsAsync(
        Expression<Func<FileSystemEntry, bool>> predicate,
        CancellationToken cancellation = default)
    {
        return Build().AnyAsync(predicate, cancellation);
    }

    public Task<FileSystemEntry> SingleAsync(
        Expression<Func<FileSystemEntry, bool>> predicate,
        CancellationToken cancellation = default)
    {
        return Build().SingleAsync(predicate, cancellation);
    }

    public Task<FileSystemEntry?> SingleOrDefaultAsync(
        Expression<Func<FileSystemEntry, bool>> predicate,
        CancellationToken cancellation = default)
    {
        return Build().SingleOrDefaultAsync(predicate, cancellation);
    }

    public Task<List<FileSystemEntry>> ToListAsync(
        Expression<Func<FileSystemEntry, bool>> predicate,
        CancellationToken cancellation = default)
    {
        return Build().Where(predicate).ToListAsync(cancellation);
    }

    public FileSystemEntry Create(FileSystemEntry entity)
    {
        return _database.FileSystemEntries.Add(entity).Entity;
    }
}