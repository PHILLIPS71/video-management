﻿using Giantnodes.Service.Dashboard.Persistence;
using HotChocolate.AspNetCore;

namespace Giantnodes.Service.Dashboard.HttpApi;

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
            .AddHttpApiServices();
        
        services
            .AddPersistenceServices(_configuration);
    }

    public void Configure(IApplicationBuilder app)
    {
        if (!_environment.IsDevelopment())
            app.UseHttpsRedirection();

        app
            .UseRouting()
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