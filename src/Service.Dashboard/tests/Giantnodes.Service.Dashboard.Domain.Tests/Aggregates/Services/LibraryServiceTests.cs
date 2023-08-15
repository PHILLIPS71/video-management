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

        var info = FileSystem.DirectoryInfo.New(MockUnixSupport.Path(@"C:\tv-shows\Mr. Robot\Season 1"));
        var directory = new FileSystemDirectory(info);

        // act
        // assert
        Assert.Throws<DirectoryNotFoundException>(() => service.GetFileSystemInfos(directory));
    }

    [Fact]
    public void Should_Return_Only_Media_Files()
    {
        // arrange
        var service = new LibraryService(FileSystem);

        var info = FileSystem.DirectoryInfo.New(RootPath);
        var directory = new FileSystemDirectory(info);

        // act
        var infos = service.GetFileSystemInfos(directory);

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

        var info = FileSystem.DirectoryInfo.New(RootPath);
        var directory = new FileSystemDirectory(info);

        // act
        var results = service.GetFileSystemInfos(directory);

        // assert
        var paths = results.Select(x => x.FullName);
        Assert.All(TopLevelDirectories, x => Assert.Contains(x, paths));
    }

    [Fact]
    public void Should_Return_Read_Only_Collection()
    {
        // arrange
        var service = new LibraryService(FileSystem);

        var info = FileSystem.DirectoryInfo.New(RootPath);
        var directory = new FileSystemDirectory(info);

        // act
        var infos = service.GetFileSystemInfos(directory);

        // assert
        Assert.IsType<ReadOnlyCollection<IFileSystemInfo>>(infos);
    }
}