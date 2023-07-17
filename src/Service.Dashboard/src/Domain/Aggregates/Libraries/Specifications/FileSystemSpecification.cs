using System.Linq.Expressions;
using Giantnodes.Infrastructure.Domain.Specifications;
using Giantnodes.Service.Dashboard.Domain.Aggregates.Libraries.Entities;
using Giantnodes.Service.Dashboard.Domain.Shared.Enums;

namespace Giantnodes.Service.Dashboard.Domain.Aggregates.Libraries.Specifications;

public class FileSystemSpecification : Specification<Library>
{
    public override Expression<Func<Library, bool>> ToExpression()
    {
        return (entity) => entity.DriveStatus == DriveStatus.Online;
    }
}