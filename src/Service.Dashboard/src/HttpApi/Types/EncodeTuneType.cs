using Giantnodes.Service.Dashboard.Domain.Enumerations;

namespace Giantnodes.Service.Dashboard.HttpApi.Types;

public class EncodeTuneType : ObjectType<EncodeTune>
{
    protected override void Configure(IObjectTypeDescriptor<EncodeTune> descriptor)
    {
        descriptor
            .Field(x => x.Id);

        descriptor
            .Field(x => x.Name);

        descriptor
            .Field(x => x.Description);
    }
}