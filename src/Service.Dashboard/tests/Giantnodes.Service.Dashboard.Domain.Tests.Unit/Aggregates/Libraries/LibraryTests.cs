using Giantnodes.Service.Dashboard.Domain.Aggregates.Libraries;
using Giantnodes.Service.Dashboard.Domain.Shared.Enums;
using Giantnodes.Service.Dashboard.Tests.Shared.Fixtures;
using Xunit;

namespace Giantnodes.Service.Dashboard.Domain.Tests.Aggregates.Libraries;

public class LibraryTests : FileSystemFixture
{
    [Theory]
    [MemberData(nameof(GetDirectories), parameters: 5)]
    public void Construct_ShouldCreateLibrary_WhenGivenValidDirectory(string path)
    {
        // arrange
        var directory = FileSystem.DirectoryInfo.New(path);

        // act
        var library = new Library(directory, "Silicon Valley", "silicon-valley");

        // assert
        Assert.Equal("Silicon Valley", library.Name);
        Assert.Equal("silicon-valley", library.Slug);
        Assert.Equal(FileSystemStatus.Online, library.Status);
        Assert.Equal(path, library.Directory.PathInfo.FullName);
    }

    [Theory]
    [MemberData(nameof(GetDirectories), parameters: 5)]
    public void Construct_ShouldSetStatusOnline_WhenGivenValidDirectory(string path)
    {
        // arrange
        var directory = FileSystem.DirectoryInfo.New(path);

        // act
        var library = new Library(directory, "Silicon Valley", "silicon-valley");

        // assert
        Assert.Equal(FileSystemStatus.Online, library.Status);
    }

    [Fact]
    public void Construct_ShouldSetStatusOffline_WhenGivenNonExistentDirectory()
    {
        // arrange
        var directory = FileSystem.DirectoryInfo.New(@"C:\tv-shows\Mr. Robot\Season 1");

        // act
        var library = new Library(directory, "Mr. Robot", "mr-robot");

        // assert
        Assert.Equal(FileSystemStatus.Offline, library.Status);
    }
}