using Giantnodes.Service.Dashboard.Domain.Enumerations;

namespace Giantnodes.Service.Dashboard.HttpApi.Types;

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