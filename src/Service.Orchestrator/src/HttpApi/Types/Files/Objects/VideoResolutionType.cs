using Giantnodes.Service.Orchestrator.Domain.Enumerations;

namespace Giantnodes.Service.Orchestrator.HttpApi.Types.Files.Objects;

public class VideoResolutionType : ObjectType<VideoResolution>
{
    protected override void Configure(IObjectTypeDescriptor<VideoResolution> descriptor)
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