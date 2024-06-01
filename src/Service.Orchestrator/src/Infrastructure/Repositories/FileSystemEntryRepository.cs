using System.Linq.Expressions;
using Giantnodes.Service.Orchestrator.Domain.Aggregates.Entries;
using Giantnodes.Service.Orchestrator.Domain.Aggregates.Entries.Directories;
using Giantnodes.Service.Orchestrator.Domain.Aggregates.Entries.Files;
using Giantnodes.Service.Orchestrator.Domain.Aggregates.Entries.Repositories;
using Giantnodes.Service.Orchestrator.Persistence.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace Giantnodes.Service.Orchestrator.Infrastructure.Repositories;

public class FileSystemEntryRepository : IFileSystemEntryRepository
{
    private readonly ApplicationDbContext _database;

    public FileSystemEntryRepository(ApplicationDbContext database)
    {
        _database = database;
    }

    public IQueryable<FileSystemEntry> ToQueryable()
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
        return ToQueryable().AnyAsync(predicate, cancellation);
    }

    public Task<FileSystemEntry> SingleAsync(
        Expression<Func<FileSystemEntry, bool>> predicate,
        CancellationToken cancellation = default)
    {
        return ToQueryable().SingleAsync(predicate, cancellation);
    }

    public Task<FileSystemEntry?> SingleOrDefaultAsync(
        Expression<Func<FileSystemEntry, bool>> predicate,
        CancellationToken cancellation = default)
    {
        return ToQueryable().SingleOrDefaultAsync(predicate, cancellation);
    }

    public Task<List<FileSystemEntry>> ToListAsync(
        Expression<Func<FileSystemEntry, bool>> predicate,
        CancellationToken cancellation = default)
    {
        return ToQueryable().Where(predicate).ToListAsync(cancellation);
    }

    public FileSystemEntry Create(FileSystemEntry entity)
    {
        return _database.FileSystemEntries.Add(entity).Entity;
    }

    public FileSystemEntry Delete(FileSystemEntry entity)
    {
        return _database.FileSystemEntries.Remove(entity).Entity;
    }
}