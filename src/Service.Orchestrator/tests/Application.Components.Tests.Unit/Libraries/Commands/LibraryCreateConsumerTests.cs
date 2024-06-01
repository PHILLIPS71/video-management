using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using Giantnodes.Infrastructure.Faults;
using Giantnodes.Infrastructure.Faults.Types;
using Giantnodes.Infrastructure.Uow.Services;
using Giantnodes.Service.Orchestrator.Application.Components.Libraries.Commands;
using Giantnodes.Service.Orchestrator.Application.Contracts.Libraries.Commands;
using Giantnodes.Service.Orchestrator.Domain.Aggregates.Libraries;
using Giantnodes.Service.Orchestrator.Domain.Aggregates.Libraries.Repositories;
using Giantnodes.Service.Orchestrator.Tests.Shared.Fixtures;
using MassTransit;
using MassTransit.Testing;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Xunit;

namespace Giantnodes.Service.Orchestrator.Application.Components.Tests.Libraries.Commands;

public class LibraryCreateConsumerTests : FileSystemFixture
{
    private readonly IServiceProvider _provider;
    private readonly IFileSystem _fs = Substitute.For<IFileSystem>();
    private readonly IUnitOfWorkService _uow = Substitute.For<IUnitOfWorkService>();
    private readonly ILibraryRepository _repository = Substitute.For<ILibraryRepository>();

    public LibraryCreateConsumerTests()
    {
        _provider = new ServiceCollection()
            .AddSingleton(_fs)
            .AddSingleton(_uow)
            .AddSingleton(_repository)
            .AddSingleton<IFileSystem>(FileSystem)
            .AddMassTransitTestHarness(cfg => cfg.AddConsumer<LibraryCreateConsumer>())
            .BuildServiceProvider(true);
    }

    [Fact]
    public async Task Consumer_ShouldCreateLibrary_WhenGivenValidCommand()
    {
        // arrange
        var command = new LibraryCreate.Command
        {
            Name = "Silicon Valley",
            Slug = "silicon-valley",
            DirectoryPath = MockUnixSupport.Path(@"C:\tv-shows\Silicon Valley")
        };

        var harness = _provider.GetRequiredService<ITestHarness>();
        await harness.Start();

        // act
        var client = harness.GetRequestClient<LibraryCreate.Command>();
        await client.GetResponse<LibraryCreate.Result>(command);

        // assert
        _repository.Received().Create(Arg.Is<Library>(x =>
            x.Name == command.Name &&
            x.Slug == command.Slug &&
            x.PathInfo.FullName == command.DirectoryPath &&
            !x.IsWatched));

        Assert.True(await harness.Sent.Any<LibraryCreate.Result>(x => x.Context.Message.Id != Guid.Empty));
    }

    [Fact]
    public async Task Consumer_ShouldCreateWatchedLibrary_WhenIsWatchedFlagIsTrue()
    {
        // arrange
        var command = new LibraryCreate.Command
        {
            Name = "Silicon Valley",
            Slug = "silicon-valley",
            DirectoryPath = MockUnixSupport.Path(@"C:\tv-shows\Silicon Valley"),
            IsWatched = true
        };

        var harness = _provider.GetRequiredService<ITestHarness>();
        await harness.Start();

        // act
        var client = harness.GetRequestClient<LibraryCreate.Command>();
        await client.GetResponse<LibraryCreate.Result>(command);

        // assert
        _repository.Received().Create(Arg.Is<Library>(x => x.IsWatched));

        Assert.True(await harness.Sent.Any<LibraryCreate.Result>(x => x.Context.Message.Id != Guid.Empty));
    }

    [Fact]
    public async Task Consumer_ShouldReturnDomainFault_WhenDirectoryDoesNotExist()
    {
        // arrange
        var command = new LibraryCreate.Command
        {
            Name = "The Simpsons",
            Slug = "the-simpsons",
            DirectoryPath = MockUnixSupport.Path(@"C:\tv-shows\The Simpsons")
        };

        var harness = _provider.GetRequiredService<ITestHarness>();
        await harness.Start();

        // act
        var client = harness.GetRequestClient<LibraryCreate.Command>();
        await client.GetResponse<DomainFault>(command);

        // assert
        Assert.True(await harness.Sent.Any<DomainFault>(x => x.Context.Message.Code == FaultKind.NotFound.Code));
    }
}