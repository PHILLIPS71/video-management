using Giantnodes.Service.Dashboard.Domain.Enumerations;

namespace Giantnodes.Service.Dashboard.HttpApi.Types;

public class EncodeTuneType : ObjectType<EncodeTune>
{
    protected override void Configure(IObjectTypeDescriptor<EncodeTune> descriptor)
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