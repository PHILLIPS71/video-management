using Giantnodes.Service.Dashboard.Domain.Values;
using Giantnodes.Service.Dashboard.Tests.Shared.Fixtures;
using Xunit;

namespace Giantnodes.Service.Dashboard.Domain.Tests.Values;

public class PathInfoTests : FileSystemFixture
{
    [Theory]
    [MemberData(nameof(GetDirectories), parameters: 5)]
    public void Should_Construct_Directories(string path)
    {
        // arrange
        var directory = FileSystem.DirectoryInfo.New(path);
        
        // act
        var info = new PathInfo(directory);

        // assert
        Assert.Equal(info.Name, directory.Name);
        Assert.Equal(info.FullName, directory.FullName);
        Assert.Equal(info.DirectoryPath, Path.GetDirectoryName(directory.FullName));
        Assert.Null(info.Extension);
    }

    [Theory]
    [MemberData(nameof(GetFiles), parameters: 5)]
    public void Should_Construct_Media_Files(string path)
    {
        // arrange
        var file = FileSystem.FileInfo.New(path);
        
        // act
        var info = new PathInfo(file);

        // assert
        Assert.Equal(info.Name, file.Name);
        Assert.Equal(info.FullName, file.FullName);
        Assert.Equal(info.DirectoryPath, Path.GetDirectoryName(file.FullName));
        Assert.Equal(info.Extension, file.Extension);
    }
}