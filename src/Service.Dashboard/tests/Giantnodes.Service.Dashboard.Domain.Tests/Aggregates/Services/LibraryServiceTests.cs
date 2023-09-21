using System.Collections.ObjectModel;
using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using Giantnodes.Service.Dashboard.Domain.Aggregates.Libraries.Services.Impl;
using Giantnodes.Service.Dashboard.Tests.Shared.Fixtures;
using Xunit;

namespace Giantnodes.Service.Dashboard.Domain.Tests.Aggregates.Services;

public class LibraryServiceTests : FileSystemFixture
{
    [Fact]
    public void Should_Return_Empty_Array_Directory_Not_Found()
    {
        // arrange
        var service = new FileSystemService(FileSystem);

        var path = MockUnixSupport.Path(@"C:\tv-shows\Mr. Robot\Season 1");

        // act
        var infos = service.GetFileSystemEntries(path);

        // assert
        Assert.Empty(infos);
    }

    [Fact]
    public void Should_Return_Only_Media_Files()
    {
        // arrange
        var service = new FileSystemService(FileSystem);

        // act
        var infos = service.GetFileSystemEntries(RootPath);

        // assert
        var paths = infos.Select(x => x.FullName).ToList();
        Assert.DoesNotContain(MockUnixSupport.Path(@"C:\tv-shows\Silicon Valley\Season 1\.DS_Store"), paths);
        Assert.DoesNotContain(MockUnixSupport.Path(@"C:\tv-shows\Silicon Valley\Season 1\poster.png"), paths);
    }

    [Fact]
    public void Should_Return_Directories()
    {
        // arrange
        var service = new FileSystemService(FileSystem);

        // act
        var results = service.GetFileSystemEntries(RootPath);

        // assert
        var paths = results.Select(x => x.FullName);
        Assert.All(TopLevelDirectories, x => Assert.Contains(x, paths));
    }

    [Fact]
    public void Should_Return_Read_Only_Collection()
    {
        // arrange
        var service = new FileSystemService(FileSystem);

        // act
        var infos = service.GetFileSystemEntries(RootPath);

        // assert
        Assert.IsType<ReadOnlyCollection<IFileSystemInfo>>(infos);
    }
}