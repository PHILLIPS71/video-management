using Giantnodes.Service.Orchestrator.Domain.Values;
using Giantnodes.Service.Orchestrator.Tests.Shared.Fixtures;
using Xunit;

namespace Giantnodes.Service.Orchestrator.Domain.Tests.Values;

public class PathInfoTests : FileSystemFixture
{
    [Theory]
    [MemberData(nameof(GetDirectories), parameters: 5)]
    public void Construct_ShouldCreatePathInfo_WhenGivenValidDirectory(string path)
    {
        // arrange
        var directory = FileSystem.DirectoryInfo.New(path);

        // act
        var info = new PathInfo(directory);

        // assert
        Assert.Equal(info.Name, directory.Name);
        Assert.Equal(info.FullName, directory.FullName);
        Assert.Null(info.Extension);
        Assert.Equal(info.DirectoryPath, Path.GetDirectoryName(directory.FullName));
        Assert.Equal(info.DirectorySeparatorChar, Path.DirectorySeparatorChar);
    }

    [Theory]
    [MemberData(nameof(GetFiles), parameters: 5)]
    public void Construct_ShouldCreatePathInfo_WhenGivenValidFile(string path)
    {
        // arrange
        var file = FileSystem.FileInfo.New(path);

        // act
        var info = new PathInfo(file);

        // assert
        Assert.Equal(info.Name, file.Name);
        Assert.Equal(info.FullName, file.FullName);
        Assert.Equal(info.Extension, file.Extension);
        Assert.Equal(info.DirectoryPath, Path.GetDirectoryName(file.FullName));
        Assert.Equal(info.DirectorySeparatorChar, Path.DirectorySeparatorChar);
    }
}