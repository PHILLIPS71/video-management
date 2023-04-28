using Giantnodes.Service.Dashboard.Domain.Entities.Files;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Giantnodes.Service.Dashboard.Persistence.Configurations.Files
{
    public class FileSystemNodeConfiguration : IEntityTypeConfiguration<FileSystemNode>
    {
        public void Configure(EntityTypeBuilder<FileSystemNode> builder)
        {
            builder
                .UseTpcMappingStrategy();
        }
    }
}
