using System.Transactions;
using Giantnodes.Infrastructure.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace Giantnodes.Infrastructure.Uow.Services.Impl;

public class UnitOfWorkService : IUnitOfWorkService, ITransientDependency
{
    private readonly IServiceProvider _services;

    public IUnitOfWork? Current { get; private set; }

    public UnitOfWorkService(IServiceProvider services)
    {
        _services = services;
    }

    public Task<IUnitOfWorkContext> BeginAsync(CancellationToken cancellation = default)
    {
        return BeginAsync(new UnitOfWorkOptions { Scope = TransactionScopeOption.Required }, cancellation);
    }

    public async Task<IUnitOfWorkContext> BeginAsync(UnitOfWorkOptions options, CancellationToken cancellation = default)
    {
        ArgumentNullException.ThrowIfNull(options);

        var uow = _services.GetRequiredService<IUnitOfWork>();

        uow.Completed += (sender, args) => Current = null;

        uow.Failed += (sender, args) => Current = null;

        uow.Disposed += (sender, args) => Current = null;

        Current = await uow.BeginAsync(options, cancellation);

        return uow;
    }
}