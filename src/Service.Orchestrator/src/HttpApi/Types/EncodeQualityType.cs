using Giantnodes.Service.Orchestrator.Domain.Enumerations;

namespace Giantnodes.Service.Orchestrator.HttpApi.Types;

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