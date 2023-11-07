namespace Giantnodes.Infrastructure.Uow.Services;

public interface IUnitOfWorkService
{
    IUnitOfWork? Current { get; }

    IUnitOfWorkContext Begin();

    IUnitOfWorkContext Begin(UnitOfWorkOptions options);
}