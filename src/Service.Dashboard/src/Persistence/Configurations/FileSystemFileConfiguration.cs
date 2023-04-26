using Giantnodes.Service.Dashboard.Domain.Entities.Files;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Giantnodes.Service.Dashboard.Persistence.Configurations
{
    public class FileSystemFileConfiguration : IEntityTypeConfiguration<FileSystemFile>
    {
        public void Configure(EntityTypeBuilder<FileSystemFile> builder)
        {
            builder
                .HasIndex(p => p.FullPath)
                .IsUnique();
        }
    }
}
