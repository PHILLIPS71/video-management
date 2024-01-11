using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using Giantnodes.Infrastructure.Domain.Entities.Auditing;
using Giantnodes.Infrastructure.Domain.Objects;

namespace Giantnodes.Infrastructure.EntityFrameworkCore;

public class GiantnodesDbContext<TDbContext> : DbContext
    where TDbContext : DbContext
{
    protected GiantnodesDbContext(DbContextOptions<TDbContext> options)
        : base(options)
    {
    }

    private static readonly MethodInfo? ConfigureValueConverterMethodInfo
        = typeof(GiantnodesDbContext<TDbContext>)
            .GetMethod(
                nameof(ConfigureIdValueGenerated),
                BindingFlags.Instance | BindingFlags.NonPublic
            );

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        foreach (var type in builder.Model.GetEntityTypes())
        {
            ConfigureValueConverterMethodInfo?
                .MakeGenericMethod(type.ClrType)
                .Invoke(this, new object[] { builder });
        }
    }

    public override int SaveChanges()
    {
        NullifyEmptyStrings();
        SetTimestampEntityProperties();

        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellation = default)
    {
        NullifyEmptyStrings();
        SetTimestampEntityProperties();

        return base.SaveChangesAsync(cancellation);
    }

    /// <summary>
    /// Prevents Id properties being generated for models implementing <see cref="IEntity"/>, leaving the
    /// responsibility to the model class.
    /// </summary>
    /// <param name="builder">The builder being used to construct the model for this context.</param>
    /// <typeparam name="TEntity">A entity class being created via ef core.</typeparam>
    private void ConfigureIdValueGenerated<TEntity>(ModelBuilder builder)
        where TEntity : class
    {
        if (!typeof(IEntity<Guid>).IsAssignableFrom(typeof(TEntity)))
            return;

        var identifier = builder.Entity<TEntity>().Property(x => ((IEntity<Guid>)x).Id);
        if (identifier.Metadata.PropertyInfo != null &&
            identifier.Metadata.PropertyInfo.IsDefined(typeof(DatabaseGeneratedAttribute), true))
            return;

        identifier.ValueGeneratedNever();
    }

    private void NullifyEmptyStrings()
    {
        foreach (var entity in ChangeTracker.Entries())
        {
            var properties = entity
                .Entity
                .GetType()
                .GetProperties()
                .Where(p => p.PropertyType == typeof(string) && p.CanRead && p.CanWrite);

            foreach (var property in properties)
            {
                property.SetValue(entity.Entity, (property.GetValue(entity.Entity) as string)?.Trim());

                if (string.IsNullOrWhiteSpace(property.GetValue(entity.Entity) as string))
                    property.SetValue(entity.Entity, null, null);
            }
        }
    }

    private void SetTimestampEntityProperties()
    {
        foreach (var entry in ChangeTracker.Entries<ITimestampableEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    ObjectHelper.SetProperty(entry.Entity, x => x.CreatedAt, _ => DateTime.UtcNow);
                    break;

                case EntityState.Modified:
                    ObjectHelper.SetProperty(entry.Entity, x => x.UpdatedAt, _ => DateTime.UtcNow);
                    break;
            }
        }
    }
}