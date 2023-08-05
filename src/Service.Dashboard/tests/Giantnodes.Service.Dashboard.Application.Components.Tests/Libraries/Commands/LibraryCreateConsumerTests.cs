﻿using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using Giantnodes.Service.Dashboard.Application.Components.Libraries.Commands;
using Giantnodes.Service.Dashboard.Application.Contracts.Libraries.Commands;
using Giantnodes.Service.Dashboard.Domain.Aggregates.Libraries.Services;
using Giantnodes.Service.Dashboard.Domain.Aggregates.Libraries.Services.Impl;
using Giantnodes.Service.Dashboard.Persistence.DbContexts;
using MassTransit;
using MassTransit.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Giantnodes.Service.Dashboard.Application.Components.Tests.Libraries.Commands;

public class LibraryCreateConsumerTests
{
    private readonly ApplicationDbContext _database;
    private readonly IServiceCollection _services;

    private readonly MockFileSystem _fs = new MockFileSystem(new Dictionary<string, MockFileData>
    {
        { MockUnixSupport.Path(@"C:\tv-shows\Silicon Valley"), new MockDirectoryData() },
        { MockUnixSupport.Path(@"C:\tv-shows\Silicon Valley\Season 1"), new MockDirectoryData() },
        { MockUnixSupport.Path(@"C:\tv-shows\Silicon Valley\Season 1\.DS_Store"), new MockFileData(string.Empty) },
        { MockUnixSupport.Path(@"C:\tv-shows\Silicon Valley\Season 1\poster.png"), new MockFileData(string.Empty) },
        { MockUnixSupport.Path(@"C:\tv-shows\Silicon Valley\Season 1\Silicon Valley - S01E01 - Minimum Viable Product.mp4"), new MockFileData(string.Empty) }
    });

    public LibraryCreateConsumerTests()
    {
        _database = new ApplicationDbContext(new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options);

        _services = new ServiceCollection()
            .AddSingleton<ApplicationDbContext>(_ => _database)
            .AddSingleton<IFileSystem>(_fs)
            .AddMassTransitTestHarness(cfg => cfg.AddConsumer<LibraryCreateConsumer>());
    }

    [Fact]
    public async Task Should_Create_Library()
    {
        var provider = _services
            .AddSingleton<ILibraryService, LibraryService>()
            .BuildServiceProvider(true);

        // arrange
        var command = new LibraryCreate.Command
        {
            Name = "Silicon Valley",
            Slug = "silicon-valley",
            FullPath = MockUnixSupport.Path(@"C:\tv-shows\Silicon Valley")
        };

        var harness = provider.GetRequiredService<ITestHarness>();
        await harness.Start();

        // act
        var client = harness.GetRequestClient<LibraryCreate.Command>();
        await client.GetResponse<LibraryCreate.Result>(command);

        // assert
        var library = await _database.Libraries.SingleAsync();
        Assert.Equal(command.Name, library.Name);
        Assert.Equal(command.Slug, library.Slug);
        Assert.Equal(command.FullPath, library.PathInfo.FullName);
    }

    [Fact]
    public async Task Should_Return_Library_Id()
    {
        var provider = _services
            .AddSingleton<ILibraryService, LibraryService>()
            .BuildServiceProvider(true);

        // arrange
        var command = new LibraryCreate.Command
        {
            Name = "Silicon Valley",
            Slug = "silicon-valley",
            FullPath = MockUnixSupport.Path(@"C:\tv-shows\Silicon Valley")
        };

        var harness = provider.GetRequiredService<ITestHarness>();
        await harness.Start();

        // act
        var client = harness.GetRequestClient<LibraryCreate.Command>();
        await client.GetResponse<LibraryCreate.Result>(command);

        // assert
        Assert.True(harness.Sent.Select<LibraryCreate.Result>().Any());
    }
}