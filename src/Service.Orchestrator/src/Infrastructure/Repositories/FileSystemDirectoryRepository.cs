using System.Linq.Expressions;
using Giantnodes.Service.Orchestrator.Domain.Aggregates.Entries.Directories;
using Giantnodes.Service.Orchestrator.Domain.Aggregates.Entries.Directories.Repositories;
using Giantnodes.Service.Orchestrator.Persistence.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace Giantnodes.Service.Orchestrator.Infrastructure.Repositories;

public class FileSystemDirectoryRepository : IFileSystemDirectoryRepository
{
    private readonly ApplicationDbContext _database;

    public FileSystemDirectoryRepository(ApplicationDbContext database)
    {
        _database = database;
    }

    public IQueryable<FileSystemDirectory> ToQueryable()
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
        return ToQueryable().AnyAsync(predicate, cancellation);
    }

    public Task<FileSystemDirectory> SingleAsync(
        Expression<Func<FileSystemDirectory, bool>> predicate,
        CancellationToken cancellation = default)
    {
        return ToQueryable().SingleAsync(predicate, cancellation);
    }

    public Task<FileSystemDirectory?> SingleOrDefaultAsync(
        Expression<Func<FileSystemDirectory, bool>> predicate,
        CancellationToken cancellation = default)
    {
        return ToQueryable().SingleOrDefaultAsync(predicate, cancellation);
    }

    public Task<List<FileSystemDirectory>> ToListAsync(
        Expression<Func<FileSystemDirectory, bool>> predicate,
        CancellationToken cancellation = default)
    {
        return ToQueryable()
            .Where(predicate)
            .ToListAsync(cancellation);
    }

    public FileSystemDirectory Create(FileSystemDirectory entity)
    {
        return _database.FileSystemDirectories.Add(entity).Entity;
    }

    public FileSystemDirectory Delete(FileSystemDirectory entity)
    {
        return _database.FileSystemDirectories.Remove(entity).Entity;
    }
}