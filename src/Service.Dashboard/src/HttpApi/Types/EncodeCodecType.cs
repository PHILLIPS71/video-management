using Giantnodes.Service.Dashboard.Domain.Enumerations;

namespace Giantnodes.Service.Dashboard.HttpApi.Types;

public class EncodeCodecType : ObjectType<EncodeCodec>
{
    protected override void Configure(IObjectTypeDescriptor<EncodeCodec> descriptor)
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

        descriptor
            .Field(x => x.Quality);

        descriptor
            .Field(x => x.Tunes)
            .UseFiltering()
            .UseSorting();
    }
}