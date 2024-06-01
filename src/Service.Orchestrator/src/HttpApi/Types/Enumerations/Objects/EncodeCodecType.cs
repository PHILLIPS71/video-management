using Giantnodes.Infrastructure;
using Giantnodes.Service.Orchestrator.Domain.Enumerations;

namespace Giantnodes.Service.Orchestrator.HttpApi.Types.Enumerations.Objects;

[ObjectType<EncodeCodec>]
public static partial class EncodeCodecType
{
    static partial void Configure(IObjectTypeDescriptor<EncodeCodec> descriptor)
    {
        descriptor
            .Field(p => p.Id);

        descriptor
            .Field(x => x.Name);

        descriptor
            .Field(x => x.Value);

        descriptor
            .Field(x => x.Description);

        descriptor
            .Field(x => x.Quality);

        descriptor
            .Field(x => x.Tunes)
            .UseFiltering()
            .UseSorting();
    }

    [NodeResolver]
    internal static EncodeCodec? GetEncodeCodecById(int id)
    {
        return Enumeration.TryParseByValueOrName<EncodeCodec>(id.ToString());
    }
}

[ObjectType<EncodeCodec.EncodeQuality>]
public static partial class EncodeQualityType
{
    static partial void Configure(IObjectTypeDescriptor<EncodeCodec.EncodeQuality> descriptor)
    {
        descriptor
            .Field(x => x.Min);

        descriptor
            .Field(x => x.Max);

        descriptor
            .Field(x => x.Default);
    }
}