using Giantnodes.Service.Dashboard.Domain.Enumerations;

namespace Giantnodes.Service.Dashboard.HttpApi.Types;

public class EncodeQualityType : ObjectType<EncodeCodec.EncodeQuality>
{
    protected override void Configure(IObjectTypeDescriptor<EncodeCodec.EncodeQuality> descriptor)
    {
        descriptor
            .Field(x => x.Min);

        descriptor
            .Field(x => x.Max);

        descriptor
            .Field(x => x.Default);
    }
}