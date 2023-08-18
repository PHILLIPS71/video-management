using Giantnodes.Service.Dashboard.Domain.Aggregates.Libraries.Entities;
using Giantnodes.Service.Dashboard.Domain.Values;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Giantnodes.Service.Dashboard.Persistence.Configurations;

public class FileSystemEntryConfiguration : IEntityTypeConfiguration<FileSystemEntry>
{
    public void Configure(EntityTypeBuilder<FileSystemEntry> builder)
    {
        builder
            .UseTpcMappingStrategy();
    }
}