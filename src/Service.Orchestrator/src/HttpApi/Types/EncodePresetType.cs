using Giantnodes.Service.Orchestrator.Domain.Enumerations;

namespace Giantnodes.Service.Orchestrator.HttpApi.Types;

public class EncodePresetType : ObjectType<EncodePreset>
{
    protected override void Configure(IObjectTypeDescriptor<EncodePreset> descriptor)
    {
        descriptor
            .ImplementsNode()
            .IdField(p => p.Id);

        descriptor
            .Field(p => p.Id);

        descriptor
            .Field(x => x.Name);

        descriptor
            .Field(x => x.Value);

        descriptor
            .Field(x => x.Description);
    }
}