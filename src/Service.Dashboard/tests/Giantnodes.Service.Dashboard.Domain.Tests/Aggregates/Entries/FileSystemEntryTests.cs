using Giantnodes.Service.Dashboard.Domain.Aggregates.Entries;
using Giantnodes.Service.Dashboard.Domain.Aggregates.Entries.Directories;
using Giantnodes.Service.Dashboard.Domain.Aggregates.Entries.Files;
using Giantnodes.Service.Dashboard.Domain.Aggregates.Libraries;
using Giantnodes.Service.Dashboard.Tests.Shared.Fixtures;
using Xunit;

namespace Giantnodes.Service.Dashboard.Domain.Tests.Aggregates.Entries;

public class FileSystemEntryTests : FileSystemFixture
{
    [Theory]
    [MemberData(nameof(GetFiles), parameters: 5)]
    public void Should_Build_FileSystemFile(string path)
    {
        // arrange
        var file = FileSystem.FileInfo.New(path);
        if (file.Directory == null)
            throw new DirectoryNotFoundException();

        var library = new Library(file.Directory, "Silicon Valley", "silicon-valley");
        var parent = new FileSystemDirectory(library, null, file.Directory);

        // act
        var entry = FileSystemEntry.Build(library, parent, file);

        // assert
        Assert.IsType<FileSystemFile>(entry);
    }

    [Theory]
    [MemberData(nameof(GetDirectories), parameters: 5)]
    public void Should_Build_FileSystemDirectory(string path)
    {
        // arrange
        var directory = FileSystem.DirectoryInfo.New(path);
        if (directory.Parent == null)
            throw new DirectoryNotFoundException();

        var library = new Library(directory.Parent, "Silicon Valley", "silicon-valley");
        var parent = new FileSystemDirectory(library, null, directory.Parent);

        // act
        var entry = FileSystemEntry.Build(library, parent, directory);

        // assert
        Assert.IsType<FileSystemDirectory>(entry);
    }
}