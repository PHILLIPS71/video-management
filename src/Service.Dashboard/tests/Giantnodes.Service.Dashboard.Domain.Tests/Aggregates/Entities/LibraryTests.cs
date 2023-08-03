using System.IO.Abstractions.TestingHelpers;
using Giantnodes.Service.Dashboard.Domain.Aggregates.Libraries.Entities;
using Giantnodes.Service.Dashboard.Domain.Shared.Enums;
using Xunit;

namespace Giantnodes.Service.Dashboard.Domain.Tests.Aggregates.Entities;

public class LibraryTests
{
    private readonly MockFileSystem _fs = new MockFileSystem(new Dictionary<string, MockFileData> {
        { @"/media/tvshows/Silicon Valley", new MockDirectoryData() },
        { @"/media/tvshows/Silicon Valley/Season 1", new MockDirectoryData() },
        { @"/media/tvshows/Silicon Valley/Season 1/.DS_Store", new MockFileData(string.Empty) },
        { @"/media/tvshows/Silicon Valley/Season 1/poster.png", new MockFileData(string.Empty) },
        { @"/media/tvshows/Silicon Valley/Season 1/Silicon Valley - S01E01 - Minimum Viable Product.mp4", new MockFileData(string.Empty) },
        { @"/media/tvshows/Silicon Valley/Season 1/Silicon Valley - S01E02 - The Cap Table.mp4", new MockFileData(string.Empty) },
        { @"/media/tvshows/Silicon Valley/Season 1/Silicon Valley - S01E03 - Articles of Incorporation.mkv", new MockFileData(string.Empty) },
        { @"/media/tvshows/Silicon Valley/Season 1/Silicon Valley - S01E04 - Fiduciary Duties.mkv", new MockFileData(string.Empty) },
        { @"/media/tvshows/Silicon Valley/Season 1/Silicon Valley - S01E05 - Signaling Risk.avi", new MockFileData(string.Empty) },
        { @"/media/tvshows/Silicon Valley/Season 1/Silicon Valley - S01E06 - Third Party Insourcing.avi", new MockFileData(string.Empty) },
        { @"/media/tvshows/Silicon Valley/Season 1/Silicon Valley - S01E07 - Proof of Concept.mov", new MockFileData(string.Empty) },
        { @"/media/tvshows/Silicon Valley/Season 1/Silicon Valley - S01E08 - Optimal Tip-to-Tip Efficiency.mov", new MockFileData(string.Empty) }
    });

    [Fact]
    public void Should_Construct()
    {
        // arrange
        var directory = _fs.DirectoryInfo.New(@"/media/tvshows/Silicon Valley/Season 1");

        // act
        var library = new Library(directory, "Silicon Valley", "silicon-valley");

        // assert
        Assert.Equal("Silicon Valley", library.Name);
        Assert.Equal("silicon-valley", library.Slug);
    }
    
    [Fact]
    public void Should_Construct_Path_Info()
    {
        // arrange
        var directory = _fs.DirectoryInfo.New(@"/media/tvshows/Silicon Valley/Season 1");

        // act
        var library = new Library(directory, "Silicon Valley", "silicon-valley");

        // assert
        Assert.Equal(library.PathInfo.Name, directory.Name);
        Assert.Equal(library.PathInfo.FullName, directory.FullName);
        Assert.Equal(library.PathInfo.Extension, directory.Extension);
        Assert.Equal(library.PathInfo.DirectoryPath, Path.GetDirectoryName(directory.FullName));
    }

    [Fact]
    public void Should_Construct_Drive_Status_Online()
    {
        // arrange
        var directory = _fs.DirectoryInfo.New(@"/media/tvshows/Silicon Valley/Season 1");

        // act
        var library = new Library(directory, "Silicon Valley", "silicon-valley");

        // assert
        Assert.Equal(DriveStatus.Online, library.DriveStatus);
    }

    [Fact]
    public void Should_Construct_Drive_Status_Offline()
    {
        // arrange
        var directory = _fs.DirectoryInfo.New(@"/media/tvshows/Mr. Robot/Season 1");

        // act
        var library = new Library(directory, "Mr. Robot", "mr-robot");

        // assert
        Assert.Equal(DriveStatus.Offline, library.DriveStatus);
    }
}