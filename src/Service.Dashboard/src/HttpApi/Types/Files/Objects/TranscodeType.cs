using Giantnodes.Service.Dashboard.Domain.Aggregates.Entries.Files.Entities;

namespace Giantnodes.Service.Dashboard.HttpApi.Types.Files.Objects;

public class TranscodeType : ObjectType<Transcode>
{
    protected override void Configure(IObjectTypeDescriptor<Transcode> descriptor)
    {
        descriptor
            .Field(p => p.Id);

        descriptor
            .Field(p => p.File);

        descriptor
            .Field(p => p.Percent);

        descriptor
            .Field(p => p.Status);

        descriptor
            .Field(p => p.StartedAt);

        descriptor
            .Field(p => p.CompletedAt);

        descriptor
            .Field(p => p.CreatedAt);

        descriptor
            .Field(p => p.UpdatedAt);
    }
}