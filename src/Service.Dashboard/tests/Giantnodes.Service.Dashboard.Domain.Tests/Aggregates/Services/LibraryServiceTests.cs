using System.Collections.ObjectModel;
using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using Giantnodes.Service.Dashboard.Domain.Aggregates.Libraries.Entities;
using Giantnodes.Service.Dashboard.Domain.Aggregates.Libraries.Services.Impl;
using Giantnodes.Service.Dashboard.Tests.Shared.Fixtures;
using Xunit;

namespace Giantnodes.Service.Dashboard.Domain.Tests.Aggregates.Services;

public class LibraryServiceTests : FileSystemFixture
{
    [Fact]
    public void Should_Throw_DirectoryNotFoundException()
    {
        // arrange
        var service = new LibraryService(FileSystem);

        var directory = FileSystem.DirectoryInfo.New(MockUnixSupport.Path(@"C:\tv-shows\Mr. Robot\Season 1"));
        var library = new Library(directory, "Mr. Robot", "mr-robot");

        // act
        // assert
        Assert.Throws<DirectoryNotFoundException>(() => service.GetFileSystemInfos(library));
    }

    [Fact]
    public void Should_Return_Only_Media_Files()
    {
        // arrange
        var service = new LibraryService(FileSystem);

        var directory = FileSystem.DirectoryInfo.New(RootPath);
        var library = new Library(directory, "Silicon Valley", "silicon-valley");

        // act
        var infos = service.GetFileSystemInfos(library);

        // assert
        var paths = infos.Select(x => x.FullName).ToList();
        Assert.DoesNotContain(MockUnixSupport.Path(@"C:\tv-shows\Silicon Valley\Season 1\.DS_Store"), paths);
        Assert.DoesNotContain(MockUnixSupport.Path(@"C:\tv-shows\Silicon Valley\Season 1\poster.png"), paths);
    }

    [Fact]
    public void Should_Return_Directories()
    {
        // arrange
        var service = new LibraryService(FileSystem);

        var directory = FileSystem.DirectoryInfo.New(RootPath);
        var library = new Library(directory, "Silicon Valley", "silicon-valley");

        // act
        var results = service.GetFileSystemInfos(library);

        // assert
        var paths = results.Select(x => x.FullName);
        Assert.All(AllDirectories, x => Assert.Contains(x, paths));
    }

    [Fact]
    public void Should_Return_Read_Only_Collection()
    {
        // arrange
        var service = new LibraryService(FileSystem);

        var directory = FileSystem.DirectoryInfo.New(RootPath);
        var library = new Library(directory, "Silicon Valley", "silicon-valley");

        // act
        var infos = service.GetFileSystemInfos(library);

        // assert
        Assert.IsType<ReadOnlyCollection<IFileSystemInfo>>(infos);
    }
}