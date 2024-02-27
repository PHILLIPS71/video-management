using Giantnodes.Service.Dashboard.Domain.Enumerations;

namespace Giantnodes.Service.Dashboard.HttpApi.Types;

public class EncodePresetType : ObjectType<EncodePreset>
{
    protected override void Configure(IObjectTypeDescriptor<EncodePreset> descriptor)
    {
        descriptor
            .Field(x => x.Id);

        descriptor
            .Field(x => x.Name);

        descriptor
            .Field(x => x.Value);

        descriptor
            .Field(x => x.Description);
    }
}