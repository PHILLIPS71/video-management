using System.Linq.Expressions;
using Giantnodes.Service.Dashboard.Domain.Aggregates.Entries.Files;
using Giantnodes.Service.Dashboard.Domain.Aggregates.Entries.Files.Repositories;
using Giantnodes.Service.Dashboard.Persistence.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace Giantnodes.Service.Dashboard.Application.Components.Files.Repositories;

public class FileSystemFileRepository : IFileSystemFileRepository
{
    private readonly ApplicationDbContext _database;

    public FileSystemFileRepository(ApplicationDbContext database)
    {
        _database = database;
    }

    /// <summary>
    /// Builds the <see name="FileSystemFile"/> aggregates consistency boundary.
    /// </summary>
    /// <returns>A <see cref="IQueryable{TEntity}"/> of the objects that make up the consistency boundary.</returns>
    private IQueryable<FileSystemFile> Build()
    {
        return _database
            .FileSystemFiles
            .Include(x => x.Library)
            .Include(x => x.ParentDirectory)
            .Include(x => x.VideoStreams)
            .Include(x => x.AudioStreams)
            .Include(x => x.SubtitleStreams)
            .AsQueryable();
    }

    public Task<bool> ExistsAsync(
        Expression<Func<FileSystemFile, bool>> predicate, 
        CancellationToken cancellation = default)
    {
        return Build().AnyAsync(predicate, cancellation);
    }

    public Task<FileSystemFile?> SingleOrDefaultAsync(
        Expression<Func<FileSystemFile, bool>> predicate,
        CancellationToken cancellation = default)
    {
        return Build().SingleOrDefaultAsync(predicate, cancellation);
    }

    public Task<List<FileSystemFile>> ToListAsync(
        Expression<Func<FileSystemFile, bool>> predicate,
        CancellationToken cancellation = default)
    {
        return Build()
            .Where(predicate)
            .ToListAsync(cancellation);
    }

    public FileSystemFile Create(FileSystemFile entity)
    {
        return _database.FileSystemFiles.Add(entity).Entity;
    }
}