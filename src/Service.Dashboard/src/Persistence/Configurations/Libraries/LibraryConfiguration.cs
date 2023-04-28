using Giantnodes.Service.Dashboard.Domain.Entities.Libraries;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Giantnodes.Service.Dashboard.Persistence.Configurations.Libraries;

public class LibraryConfiguration : IEntityTypeConfiguration<Library>
{
    public void Configure(EntityTypeBuilder<Library> builder)
    {
        builder
            .HasIndex(p => p.FullPath)
            .IsUnique();
        
        builder
            .HasIndex(p => p.Slug)
            .IsUnique();
        
        builder
            .Property(p => p.DriveStatus)
            .HasConversion<string>();
    }
}