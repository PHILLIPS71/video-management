using Giantnodes.Service.Dashboard.Domain.Enumerations;

namespace Giantnodes.Service.Dashboard.HttpApi.Types.Files.Objects;

[ObjectType<VideoResolution>]
public static partial class VideoResolutionType
{
    static partial void Configure(IObjectTypeDescriptor<VideoResolution> descriptor)
    {
        descriptor
            .Field(p => p.Name);
        
        descriptor
            .Field(p => p.Abbreviation);

        descriptor
            .Field(p => p.Width);

        descriptor
            .Field(p => p.Height);
    }
}