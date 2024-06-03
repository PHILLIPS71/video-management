using System.Transactions;
using Giantnodes.Infrastructure.DependencyInjection;
using Giantnodes.Infrastructure.Domain.Entities;
using Giantnodes.Infrastructure.Uow;
using Giantnodes.Infrastructure.Uow.Execution;
using Microsoft.EntityFrameworkCore.Storage;

namespace Giantnodes.Infrastructure.EntityFrameworkCore.Uow;

/// <summary>
/// Represents a unit of work implementation for Entity Framework Core.
/// </summary>
/// <typeparam name="TDbContext">The type of the DbContext class.</typeparam>
public sealed class EntityFrameworkUnitOfWork<TDbContext> : UnitOfWork, ITransientDependency
    where TDbContext : DbContext
{
    private readonly TDbContext _database;
    private IDbContextTransaction? _transaction;

    public EntityFrameworkUnitOfWork(IUnitOfWorkExecutor executor, TDbContext database)
        : base(executor)
    {
        _database = database;
    }

    /// <inheritdoc />
    protected override async Task OnBeginAsync(UnitOfWorkOptions options, CancellationToken cancellation = default)
    {
        if (options.Timeout.HasValue)
            _database.Database.SetCommandTimeout(options.Timeout.Value);

        var isTransactionRequired =
            options.Scope == TransactionScopeOption.Required && _database.Database.CurrentTransaction == null ||
            options.Scope == TransactionScopeOption.RequiresNew;

        if (isTransactionRequired)
            _transaction = await _database.Database.BeginTransactionAsync(cancellation);
    }

    /// <inheritdoc />
    protected override async Task OnCommitAsync(CancellationToken cancellation = default)
    {
        var events = _database
            .ChangeTracker
            .Entries<AggregateRoot>()
            .SelectMany(x => x.Entity.DomainEvents)
            .ToList();

        DomainEvents.AddRange(events);

        await _database.SaveChangesAsync(cancellation);

        if (_transaction != null)
            await _database.Database.CommitTransactionAsync(cancellation);
    }
}