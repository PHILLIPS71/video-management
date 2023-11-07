using Giantnodes.Infrastructure.DependencyInjection;
using Giantnodes.Infrastructure.Uow;

namespace Giantnodes.Infrastructure.EntityFrameworkCore.Uow;

public sealed class EntityFrameworkUnitOfWork<TDbContext> : UnitOfWork, ITransientDependency
    where TDbContext : DbContext
{
    private readonly TDbContext _database;

    public EntityFrameworkUnitOfWork(TDbContext database)
    {
        _database = database;
    }

    protected override Task SaveChangesAsync(CancellationToken cancellation = default)
    {
        return _database.SaveChangesAsync(cancellation);
    }
}