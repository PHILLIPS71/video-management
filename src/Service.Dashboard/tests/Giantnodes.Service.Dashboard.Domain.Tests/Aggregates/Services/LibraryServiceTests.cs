using System.Collections.ObjectModel;
using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using Giantnodes.Service.Dashboard.Domain.Aggregates.Libraries.Entities;
using Giantnodes.Service.Dashboard.Domain.Aggregates.Libraries.Services.Impl;
using Xunit;

namespace Giantnodes.Service.Dashboard.Domain.Tests.Aggregates.Services;

public class LibraryServiceTests
{
    private readonly MockFileSystem _fs = new MockFileSystem(new Dictionary<string, MockFileData> {
        { @"C:\tv-shows\Silicon Valley", new MockDirectoryData() },
        { @"C:\tv-shows\Silicon Valley\Season 1", new MockDirectoryData() },
        { @"C:\tv-shows\Silicon Valley\Season 1\.DS_Store", new MockFileData(string.Empty) },
        { @"C:\tv-shows\Silicon Valley\Season 1\poster.png", new MockFileData(string.Empty) },
        { @"C:\tv-shows\Silicon Valley\Season 1\Silicon Valley - S01E01 - Minimum Viable Product.mp4", new MockFileData(string.Empty) },
        { @"C:\tv-shows\Silicon Valley\Season 1\Silicon Valley - S01E02 - The Cap Table.mp4", new MockFileData(string.Empty) },
        { @"C:\tv-shows\Silicon Valley\Season 1\Silicon Valley - S01E03 - Articles of Incorporation.mkv", new MockFileData(string.Empty) }
    });

    [Fact]
    public void Should_Throw_DirectoryNotFoundException()
    {
        // arrange
        var service = new LibraryService(_fs);

        var directory = _fs.DirectoryInfo.New(@"C:\tv-shows\Mr. Robot\Season 1");
        var library = new Library(directory, "Mr. Robot", "mr-robot");

        // act
        // assert
        Assert.Throws<DirectoryNotFoundException>(() => service.GetFileSystemInfos(library));
    }

    [Fact]
    public void Should_Return_Only_Media_Files()
    {
        // arrange
        var service = new LibraryService(_fs);

        var directory = _fs.DirectoryInfo.New(@"C:\tv-shows\Silicon Valley");
        var library = new Library(directory, "Silicon Valley", "silicon-valley");

        // act
        var infos = service.GetFileSystemInfos(library);

        // assert
        var paths = infos.Select(x => x.FullName).ToList();
        Assert.Contains(@"C:\tv-shows\Silicon Valley\Season 1\Silicon Valley - S01E01 - Minimum Viable Product.mp4", paths);
        Assert.Contains(@"C:\tv-shows\Silicon Valley\Season 1\Silicon Valley - S01E02 - The Cap Table.mp4", paths);
        Assert.Contains(@"C:\tv-shows\Silicon Valley\Season 1\Silicon Valley - S01E03 - Articles of Incorporation.mkv", paths);
        
        Assert.DoesNotContain(@"C:\tv-shows\Silicon Valley\Season 1\.DS_Store", paths);
        Assert.DoesNotContain(@"C:\tv-shows\Silicon Valley\Season 1\poster.png", paths);
    }

    [Fact]
    public void Should_Return_Directories()
    {
        // arrange
        var service = new LibraryService(_fs);

        var directory = _fs.DirectoryInfo.New(@"C:\tv-shows\Silicon Valley");
        var library = new Library(directory, "Silicon Valley", "silicon-valley");

        // act
        var infos = service.GetFileSystemInfos(library);

        // assert
        var paths = infos.Select(x => x.FullName).ToList();
        Assert.Contains(@"C:\tv-shows\Silicon Valley\Season 1", paths);
    }

    [Fact]
    public void Should_Return_Read_Only_Collection()
    {
        // arrange
        var service = new LibraryService(_fs);

        var directory = _fs.DirectoryInfo.New(@"C:\tv-shows\Silicon Valley");
        var library = new Library(directory, "Silicon Valley", "silicon-valley");

        // act
        var infos = service.GetFileSystemInfos(library);

        // assert
        Assert.IsType<ReadOnlyCollection<IFileSystemInfo>>(infos);
    }
}