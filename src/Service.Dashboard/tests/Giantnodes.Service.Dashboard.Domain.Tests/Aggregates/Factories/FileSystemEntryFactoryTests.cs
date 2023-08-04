using System.IO.Abstractions.TestingHelpers;
using Giantnodes.Service.Dashboard.Domain.Aggregates.Libraries.Entities;
using Giantnodes.Service.Dashboard.Domain.Aggregates.Libraries.Factories;
using Xunit;

namespace Giantnodes.Service.Dashboard.Domain.Tests.Aggregates.Factories;

public class FileSystemEntryFactoryTests
{
    private readonly MockFileSystem _fs = new MockFileSystem(new Dictionary<string, MockFileData> {
        { @"C:\tv-shows\Silicon Valley", new MockDirectoryData() },
        { @"C:\tv-shows\Silicon Valley\Season 1", new MockDirectoryData() },
        { @"C:\tv-shows\Silicon Valley\Season 1\Silicon Valley - S01E01 - Minimum Viable Product.mp4", new MockFileData(string.Empty) },
    });

    [Fact]
    public void Should_Build_FileSystemFile()
    {
        // arrange
        var file = _fs.FileInfo.New(@"C:\tv-shows\Silicon Valley\Season 1\Silicon Valley - S01E01 - Minimum Viable Product.mp4");

        // act
        var entry = FileSystemEntryFactory.Build(file);

        // assert
        Assert.IsType<FileSystemFile>(entry);
    }
    
    [Fact]
    public void Should_Build_FileSystemDirectory()
    {
        // arrange
        var directory = _fs.DirectoryInfo.New(@"C:\tv-shows\Silicon Valley");

        // act
        var entry = FileSystemEntryFactory.Build(directory);

        // assert
        Assert.IsType<FileSystemDirectory>(entry);
    }
}