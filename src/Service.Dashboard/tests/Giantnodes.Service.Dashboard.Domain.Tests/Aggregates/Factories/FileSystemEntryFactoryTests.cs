using System.IO.Abstractions.TestingHelpers;
using Giantnodes.Service.Dashboard.Domain.Aggregates.Libraries.Entities;
using Giantnodes.Service.Dashboard.Domain.Aggregates.Libraries.Factories;
using Xunit;

namespace Giantnodes.Service.Dashboard.Domain.Tests.Aggregates.Factories;

public class FileSystemEntryFactoryTests
{
    private readonly MockFileSystem _fs = new MockFileSystem(new Dictionary<string, MockFileData> {
        { @"/media/tvshows/Silicon Valley", new MockDirectoryData() },
        { @"/media/tvshows/Silicon Valley/Season 1", new MockDirectoryData() },
        { @"/media/tvshows/Silicon Valley/Season 1/Silicon Valley - S01E01 - Minimum Viable Product.mp4", new MockFileData(string.Empty) },
    });

    [Fact]
    public void Should_Build_FileSystemFile()
    {
        // arrange
        var file = _fs.FileInfo.New(@"/media/tvshows/Silicon Valley/Season 1/Silicon Valley - S01E01 - Minimum Viable Product.mp4");

        // act
        var entry = FileSystemEntryFactory.Build(file);

        // assert
        Assert.IsType<FileSystemFile>(entry);
    }
    
    [Fact]
    public void Should_Build_FileSystemDirectory()
    {
        // arrange
        var directory = _fs.DirectoryInfo.New(@"/media/tvshows/Silicon Valley");

        // act
        var entry = FileSystemEntryFactory.Build(directory);

        // assert
        Assert.IsType<FileSystemDirectory>(entry);
    }
}