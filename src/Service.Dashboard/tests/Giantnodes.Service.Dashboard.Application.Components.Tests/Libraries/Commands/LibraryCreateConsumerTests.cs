using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using Giantnodes.Service.Dashboard.Application.Components.Libraries.Commands;
using Giantnodes.Service.Dashboard.Application.Contracts.Libraries.Commands;
using Giantnodes.Service.Dashboard.Domain.Aggregates.Libraries.Services;
using Giantnodes.Service.Dashboard.Domain.Aggregates.Libraries.Services.Impl;
using Giantnodes.Service.Dashboard.Persistence.DbContexts;
using Giantnodes.Service.Dashboard.Tests.Shared.Fixtures;
using MassTransit;
using MassTransit.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Xunit;

namespace Giantnodes.Service.Dashboard.Application.Components.Tests.Libraries.Commands;

public class LibraryCreateConsumerTests : FileSystemFixture
{
    private readonly ApplicationDbContext _database;
    private readonly IServiceProvider _provider;

    public LibraryCreateConsumerTests()
    {
        _database = new ApplicationDbContext(new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options);

        _provider = new ServiceCollection()
            .AddSingleton<ApplicationDbContext>(_ => _database)
            .AddSingleton<IFileSystem>(FileSystem)
            .AddSingleton<IFileSystemService, FileSystemService>()
            .AddSingleton<IFileSystemWatcherFactory, FileSystemWatcherFactory>()
            .AddSingleton<IFileSystemWatcherService>(Substitute.For<IFileSystemWatcherService>())
            .AddMassTransitTestHarness(cfg => cfg.AddConsumer<LibraryCreateConsumer>())
            .BuildServiceProvider(true);
    }

    [Fact]
    public async Task Should_Create_Library()
    {
        // arrange
        var command = new LibraryCreate.Command
        {
            Name = "Silicon Valley",
            Slug = "silicon-valley",
            FullPath = MockUnixSupport.Path(@"C:\tv-shows\Silicon Valley")
        };

        var harness = _provider.GetRequiredService<ITestHarness>();
        await harness.Start();

        // act
        var client = harness.GetRequestClient<LibraryCreate.Command>();
        await client.GetResponse<LibraryCreate.Result>(command);

        // assert
        var library = await _database.Libraries.SingleAsync();
        Assert.Equal(command.Name, library.Name);
        Assert.Equal(command.Slug, library.Slug);
    }

    [Fact]
    public async Task Should_Return_Library_Id()
    {
        // arrange
        var command = new LibraryCreate.Command
        {
            Name = "Silicon Valley",
            Slug = "silicon-valley",
            FullPath = MockUnixSupport.Path(@"C:\tv-shows\Silicon Valley")
        };

        var harness = _provider.GetRequiredService<ITestHarness>();
        await harness.Start();

        // act
        var client = harness.GetRequestClient<LibraryCreate.Command>();
        await client.GetResponse<LibraryCreate.Result>(command);

        // assert
        Assert.True(harness.Sent.Select<LibraryCreate.Result>().Any());
    }
}