using System.Transactions;

namespace Giantnodes.Infrastructure.Uow;

public class UnitOfWorkOptions
{
    public TransactionScopeOption Scope { get; init; }

    public TimeSpan? Timeout { get; init; }
}