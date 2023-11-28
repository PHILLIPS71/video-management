namespace Giantnodes.Infrastructure.Uow.Services;

public interface IUnitOfWorkService
{
    IUnitOfWork? Current { get; }

    Task<IUnitOfWorkContext> BeginAsync(CancellationToken cancellation = default);

    Task<IUnitOfWorkContext> BeginAsync(UnitOfWorkOptions options, CancellationToken cancellation = default);
}