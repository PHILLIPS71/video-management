using System.Linq.Expressions;
using Giantnodes.Service.Dashboard.Domain.Aggregates.Entries.Files;
using Giantnodes.Service.Dashboard.Domain.Aggregates.Entries.Files.Repositories;
using Giantnodes.Service.Dashboard.Persistence.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace Giantnodes.Service.Dashboard.Infrastructure.Repositories;

public class FileSystemFileRepository : IFileSystemFileRepository
{
    private readonly ApplicationDbContext _database;

    public FileSystemFileRepository(ApplicationDbContext database)
    {
        _database = database;
    }

    public IQueryable<FileSystemFile> ToQueryable()
    {
        return _database
            .FileSystemFiles
            .Include(x => x.VideoStreams)
            .Include(x => x.AudioStreams)
            .Include(x => x.SubtitleStreams)
            .AsQueryable();
    }

    public Task<bool> ExistsAsync(
        Expression<Func<FileSystemFile, bool>> predicate,
        CancellationToken cancellation = default)
    {
        return ToQueryable().AnyAsync(predicate, cancellation);
    }

    public Task<FileSystemFile> SingleAsync(
        Expression<Func<FileSystemFile, bool>> predicate,
        CancellationToken cancellation = default)
    {
        return ToQueryable().SingleAsync(predicate, cancellation);
    }

    public Task<FileSystemFile?> SingleOrDefaultAsync(
        Expression<Func<FileSystemFile, bool>> predicate,
        CancellationToken cancellation = default)
    {
        return ToQueryable().SingleOrDefaultAsync(predicate, cancellation);
    }

    public Task<List<FileSystemFile>> ToListAsync(
        Expression<Func<FileSystemFile, bool>> predicate,
        CancellationToken cancellation = default)
    {
        return ToQueryable().Where(predicate).ToListAsync(cancellation);
    }

    public FileSystemFile Create(FileSystemFile entity)
    {
        return _database.FileSystemFiles.Add(entity).Entity;
    }

    public FileSystemFile Delete(FileSystemFile entity)
    {
        return _database.FileSystemFiles.Remove(entity).Entity;
    }
}