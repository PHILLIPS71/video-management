using Giantnodes.Infrastructure;
using Giantnodes.Service.Orchestrator.Domain.Enumerations;

namespace Giantnodes.Service.Orchestrator.HttpApi.Types.Enumerations.Objects;

[ObjectType<EncodeTune>]
public static partial class EncodeTuneType
{
    static partial void Configure(IObjectTypeDescriptor<EncodeTune> descriptor)
    {
        descriptor
            .Field(p => p.Id);

        descriptor
            .Field(x => x.Name);

        descriptor
            .Field(x => x.Value);

        descriptor
            .Field(x => x.Description);
    }

    [NodeResolver]
    internal static EncodeTune? GetEncodeTuneById(int id)
    {
        return Enumeration.TryParseByValueOrName<EncodeTune>(id.ToString());
    }
}