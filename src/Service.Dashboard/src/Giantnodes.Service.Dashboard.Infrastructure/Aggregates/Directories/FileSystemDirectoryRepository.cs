using System.Linq.Expressions;
using Giantnodes.Service.Dashboard.Domain.Aggregates.Entries.Directories;
using Giantnodes.Service.Dashboard.Domain.Aggregates.Entries.Directories.Repositories;
using Giantnodes.Service.Dashboard.Persistence.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace Giantnodes.Service.Dashboard.Infrastructure.Aggregates.Directories;

public class FileSystemDirectoryRepository : IFileSystemDirectoryRepository
{
    private readonly ApplicationDbContext _database;

    public FileSystemDirectoryRepository(ApplicationDbContext database)
    {
        _database = database;
    }

    /// <summary>
    /// Builds the <see name="FileSystemDirectory"/> aggregates consistency boundary.
    /// </summary>
    /// <returns>A <see cref="IQueryable{TEntity}"/> of the objects that make up the consistency boundary.</returns>
    private IQueryable<FileSystemDirectory> Build()
    {
        return _database
            .FileSystemDirectories
            .Include(x => x.Library)
            .Include(x => x.ParentDirectory)
            .Include(x => x.Entries)
            .AsQueryable();
    }

    public Task<bool> ExistsAsync(
        Expression<Func<FileSystemDirectory, bool>> predicate,
        CancellationToken cancellation = default)
    {
        return Build().AnyAsync(predicate, cancellation);
    }

    public Task<FileSystemDirectory> SingleAsync(
        Expression<Func<FileSystemDirectory, bool>> predicate,
        CancellationToken cancellation = default)
    {
        return Build().SingleAsync(predicate, cancellation);
    }

    public Task<FileSystemDirectory?> SingleOrDefaultAsync(
        Expression<Func<FileSystemDirectory, bool>> predicate,
        CancellationToken cancellation = default)
    {
        return Build().SingleOrDefaultAsync(predicate, cancellation);
    }

    public Task<List<FileSystemDirectory>> ToListAsync(
        Expression<Func<FileSystemDirectory, bool>> predicate,
        CancellationToken cancellation = default)
    {
        return Build()
            .Where(predicate)
            .ToListAsync(cancellation);
    }

    public FileSystemDirectory Create(FileSystemDirectory entity)
    {
        return _database.FileSystemDirectories.Add(entity).Entity;
    }
}