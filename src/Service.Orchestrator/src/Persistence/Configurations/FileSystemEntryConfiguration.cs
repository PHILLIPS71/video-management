using Giantnodes.Service.Orchestrator.Domain.Aggregates.Entries;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Giantnodes.Service.Orchestrator.Persistence.Configurations;

public class FileSystemEntryConfiguration : IEntityTypeConfiguration<FileSystemEntry>
{
    public void Configure(EntityTypeBuilder<FileSystemEntry> builder)
    {
        builder
            .UseTpcMappingStrategy();
    }
}