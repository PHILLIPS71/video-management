using System.IO.Abstractions;
using System.Reflection;
using Giantnodes.Infrastructure.Masstransit.Validation;
using Giantnodes.Service.Dashboard.Persistence.DbContexts;
using Giantnodes.Service.Dashboard.Persistence.Sagas;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Giantnodes.Service.Dashboard.Application;

public static class ApplicationServiceRegistration
{
    public static void AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.TryAddSingleton<IFileSystem, FileSystem>();

        services.AddMassTransitServices(configuration);
    }

    private static void AddMassTransitServices(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddMassTransit(options =>
            {
                options
                    .SetKebabCaseEndpointNameFormatter();

                options
                    .AddDelayedMessageScheduler();
                
                options
                    .AddConsumers(Assembly.Load("Giantnodes.Service.Dashboard.Application.Components"));
                
                options
                    .AddSagaStateMachines(Assembly.Load("Giantnodes.Service.Dashboard.Application"));

                options
                    .AddSagaRepository<JobSaga>()
                    .EntityFrameworkRepository(r =>
                    {
                        r.ConcurrencyMode = ConcurrencyMode.Optimistic;
                        
                        r.UsePostgres();
                        r.ExistingDbContext<ApplicationDbContext>();
                    });
                
                options
                    .AddSagaRepository<JobTypeSaga>()
                    .EntityFrameworkRepository(r =>
                    {
                        r.ConcurrencyMode = ConcurrencyMode.Optimistic;
                        
                        r.UsePostgres();
                        r.ExistingDbContext<ApplicationDbContext>();
                    });
                
                options
                    .AddSagaRepository<JobAttemptSaga>()
                    .EntityFrameworkRepository(r =>
                    {
                        r.ConcurrencyMode = ConcurrencyMode.Optimistic;
                        
                        r.UsePostgres();
                        r.ExistingDbContext<ApplicationDbContext>();
                    });
                
                options
                    .AddSagaRepository<ProbeSaga>()
                    .EntityFrameworkRepository(r =>
                    {
                        r.ConcurrencyMode = ConcurrencyMode.Optimistic;
                        
                        r.UsePostgres();
                        r.ExistingDbContext<ApplicationDbContext>();
                    });
                
                options
                    .AddSagaRepository<EncodeSaga>()
                    .EntityFrameworkRepository(r =>
                    {
                        r.ConcurrencyMode = ConcurrencyMode.Optimistic;
                        
                        r.UsePostgres();
                        r.ExistingDbContext<ApplicationDbContext>();
                    });
                
                options
                    .UsingRabbitMq((context, config) =>
                    {
                        var options = new ServiceInstanceOptions()
                            .SetEndpointNameFormatter(KebabCaseEndpointNameFormatter.Instance)
                            .EnableJobServiceEndpoints();

                        config.ServiceInstance(options, instance =>
                        {
                            instance.ConfigureJobServiceEndpoints(js =>
                            {
                                js.FinalizeCompleted = true;
                                js.ConfigureSagaRepositories(context);
                            });

                            instance.ConfigureEndpoints(context);
                        });
                        
                        config.UseDelayedMessageScheduler();
                        config.UseConsumeFilter(typeof(FluentValidationFilter<>), context);
                        config.ConfigureEndpoints(context);
                    });
            });
    }
}