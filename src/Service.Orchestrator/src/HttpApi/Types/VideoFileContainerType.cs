using Giantnodes.Service.Orchestrator.Domain.Enumerations;

namespace Giantnodes.Service.Orchestrator.HttpApi.Types;

public class VideoFileContainerType : ObjectType<VideoFileContainer>
{
    protected override void Configure(IObjectTypeDescriptor<VideoFileContainer> descriptor)
    {
        descriptor
            .ImplementsNode()
            .IdField(p => p.Id);

        descriptor
            .Field(p => p.Id);

        descriptor
            .Field(x => x.Name);

        descriptor
            .Field(x => x.Extension);
    }
}