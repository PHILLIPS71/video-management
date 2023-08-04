using System.IO.Abstractions.TestingHelpers;
using Giantnodes.Service.Dashboard.Domain.Values;
using Xunit;

namespace Giantnodes.Service.Dashboard.Domain.Tests.Values;

public class PathInfoTests
{
    private readonly MockFileSystem _fs = new MockFileSystem(new Dictionary<string, MockFileData> {
        { @"C:\tv-shows\Silicon Valley", new MockDirectoryData() },
        { @"C:\tv-shows\Silicon Valley\Season 1", new MockDirectoryData() },
        { @"C:\tv-shows\Silicon Valley\Season 1\.DS_Store", new MockFileData(string.Empty) },
        { @"C:\tv-shows\Silicon Valley\Season 1\poster.png", new MockFileData(string.Empty) },
        { @"C:\tv-shows\Silicon Valley\Season 1\Silicon Valley - S01E01 - Minimum Viable Product.mp4", new MockFileData(string.Empty) },
        { @"C:\tv-shows\Silicon Valley\Season 1\Silicon Valley - S01E02 - The Cap Table.mp4", new MockFileData(string.Empty) },
        { @"C:\tv-shows\Silicon Valley\Season 1\Silicon Valley - S01E03 - Articles of Incorporation.mkv", new MockFileData(string.Empty) },
        { @"C:\tv-shows\Silicon Valley\Season 1\Silicon Valley - S01E04 - Fiduciary Duties.mkv", new MockFileData(string.Empty) },
        { @"C:\tv-shows\Silicon Valley\Season 1\Silicon Valley - S01E05 - Signaling Risk.avi", new MockFileData(string.Empty) },
        { @"C:\tv-shows\Silicon Valley\Season 1\Silicon Valley - S01E06 - Third Party Insourcing.avi", new MockFileData(string.Empty) },
        { @"C:\tv-shows\Silicon Valley\Season 1\Silicon Valley - S01E07 - Proof of Concept.mov", new MockFileData(string.Empty) },
        { @"C:\tv-shows\Silicon Valley\Season 1\Silicon Valley - S01E08 - Optimal Tip-to-Tip Efficiency.mov", new MockFileData(string.Empty) }
    });

    [Theory]
    [InlineData(@"C:\tv-shows\Silicon Valley")]
    [InlineData(@"C:\tv-shows\Silicon Valley\Season 1")]
    public void Should_Construct_Directories(string path)
    {
        // arrange
        var directory = _fs.DirectoryInfo.New(path);
        
        // act
        var info = new PathInfo(directory);

        // assert
        Assert.Equal(info.Name, directory.Name);
        Assert.Equal(info.FullName, directory.FullName);
        Assert.Equal(info.DirectoryPath, Path.GetDirectoryName(directory.FullName));
        Assert.Null(info.Extension);
    }
    
    [Theory]
    [InlineData(@"C:\tv-shows\Silicon Valley/Season 1\Silicon Valley - S01E01 - Minimum Viable Product.mp4")]
    [InlineData(@"C:\tv-shows\Silicon Valley/Season 1\Silicon Valley - S01E04 - Fiduciary Duties.mkv")]
    [InlineData(@"C:\tv-shows\Silicon Valley/Season 1\Silicon Valley - S01E06 - Third Party Insourcing.avi")]
    [InlineData(@"C:\tv-shows\Silicon Valley/Season 1\Silicon Valley - S01E08 - Optimal Tip-to-Tip Efficiency.mov")]
    public void Should_Construct_Media_Files(string path)
    {
        // arrange
        var file = _fs.FileInfo.New(path);
        
        // act
        var info = new PathInfo(file);

        // assert
        Assert.Equal(info.Name, file.Name);
        Assert.Equal(info.FullName, file.FullName);
        Assert.Equal(info.DirectoryPath, Path.GetDirectoryName(file.FullName));
        Assert.Equal(info.Extension, file.Extension);
    }
}