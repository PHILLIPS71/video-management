using Giantnodes.Infrastructure.EntityFrameworkCore;
using Giantnodes.Service.Encoder.Persistence.Sagas;
using MassTransit;
using MassTransit.EntityFrameworkCoreIntegration;
using Microsoft.EntityFrameworkCore;

namespace Giantnodes.Service.Encoder.Persistence.DbContexts;

public class ApplicationDbContext : GiantnodesDbContext<ApplicationDbContext>
{
    internal const string Schema = "encoder";

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        foreach (var configuration in Configurations)
            configuration.Configure(builder);

        builder.AddTransactionalOutboxEntities();

        builder.HasDefaultSchema(Schema);
        builder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }

    private static IEnumerable<ISagaClassMap> Configurations
    {
        get
        {
            yield return new JobTypeSagaMap(true);
            yield return new JobSagaMap(true);
            yield return new JobAttemptSagaMap(true);
            yield return new EncodeJobSagaMap();
        }
    }
}