using MassTransit.EntityFrameworkCoreIntegration;
using Microsoft.EntityFrameworkCore;

namespace Giantnodes.Service.Encoder.Persistence.DbContexts;

public class ApplicationDbContext : SagaDbContext
{
    public ApplicationDbContext(DbContextOptions options)
        : base(options)
    {
    }

    protected override IEnumerable<ISagaClassMap> Configurations
    {
        get
        {
            yield return new JobTypeSagaMap(true);
            yield return new JobSagaMap(true);
            yield return new JobAttemptSagaMap(true);
        }
    }
}