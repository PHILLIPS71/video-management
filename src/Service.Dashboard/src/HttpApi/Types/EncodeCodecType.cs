using Giantnodes.Service.Dashboard.Domain.Enumerations;

namespace Giantnodes.Service.Dashboard.HttpApi.Types;

public class EncodeCodecType : ObjectType<EncodeCodec>
{
    protected override void Configure(IObjectTypeDescriptor<EncodeCodec> descriptor)
    {
        descriptor
            .Field(x => x.Id);

        descriptor
            .Field(x => x.Name);

        descriptor
            .Field(x => x.Description);
    }
}