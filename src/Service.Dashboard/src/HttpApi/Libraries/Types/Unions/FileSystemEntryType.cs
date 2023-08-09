using Giantnodes.Service.Dashboard.Domain.Aggregates.Libraries.Entities;

namespace Giantnodes.Service.Dashboard.HttpApi.Libraries.Types.Unions;

public class FileSystemEntryType : UnionType<FileSystemEntry>
{
    protected override void Configure(IUnionTypeDescriptor descriptor)
    {
        descriptor.Type<ObjectType<FileSystemDirectory>>();
        descriptor.Type<ObjectType<FileSystemFile>>();
    }
}