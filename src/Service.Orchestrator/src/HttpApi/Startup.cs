using Giantnodes.Service.Orchestrator.Application.Components;
using Giantnodes.Service.Orchestrator.Domain;
using Giantnodes.Service.Orchestrator.Infrastructure;
using Giantnodes.Service.Orchestrator.Persistence;
using HotChocolate.AspNetCore;

namespace Giantnodes.Service.Orchestrator.HttpApi;

public class Startup
{
    private readonly IConfiguration _configuration;
    private readonly IHostEnvironment _environment;

    public Startup(IConfiguration configuration, IHostEnvironment environment)
    {
        _configuration = configuration;
        _environment = environment;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services
            .SetupDomain()
            .SetupPersistence(_configuration)
            .SetupInfrastructure()
            .SetupApplicationComponents()
            .AddHttpApiServices();
    }

    public void Configure(IApplicationBuilder app)
    {
        if (!_environment.IsDevelopment())
            app.UseHttpsRedirection();

        app
            .UseRouting()
            .UseWebSockets()
            .UseCors()
            .UseEndpoints(endpoint =>
            {
                endpoint
                    .MapGraphQL()
                    .WithOptions(new GraphQLServerOptions
                    {
                        Tool = { Enable = _environment.IsDevelopment() }
                    });
            });
    }
}