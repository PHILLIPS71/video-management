using Giantnodes.Infrastructure;
using Giantnodes.Service.Orchestrator.Domain.Enumerations;

namespace Giantnodes.Service.Orchestrator.HttpApi.Types.Enumerations.Objects;

[ObjectType<EncodePreset>]
public static partial class EncodePresetType
{
    static partial void Configure(IObjectTypeDescriptor<EncodePreset> descriptor)
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
    internal static EncodePreset? GetEncodePresetById(int id)
    {
        return Enumeration.TryParseByValueOrName<EncodePreset>(id.ToString());
    }
}