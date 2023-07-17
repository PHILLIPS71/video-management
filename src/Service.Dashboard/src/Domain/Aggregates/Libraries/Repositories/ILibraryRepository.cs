using Giantnodes.Infrastructure.Domain.Repositories;
using Giantnodes.Service.Dashboard.Domain.Aggregates.Libraries.Entities;

namespace Giantnodes.Service.Dashboard.Domain.Aggregates.Libraries.Repositories;

public interface ILibraryRepository : IReadOnlyRepository<Library>
{
}