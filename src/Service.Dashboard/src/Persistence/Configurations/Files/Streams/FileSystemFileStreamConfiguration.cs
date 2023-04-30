using Giantnodes.Service.Dashboard.Domain.Entities.Files.Streams;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Giantnodes.Service.Dashboard.Persistence.Configurations.Files.Streams;

public class FileSystemFileStreamConfiguration : IEntityTypeConfiguration<FileSystemFileStream>
{
    public void Configure(EntityTypeBuilder<FileSystemFileStream> builder)
    {
        builder
            .UseTpcMappingStrategy();
    }
}