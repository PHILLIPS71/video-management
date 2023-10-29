using Giantnodes.Service.Dashboard.Domain.Aggregates.Entries;
using Giantnodes.Service.Dashboard.Domain.Aggregates.Entries.Directories;
using Giantnodes.Service.Dashboard.Domain.Aggregates.Entries.Files;
using Giantnodes.Service.Dashboard.Domain.Aggregates.Libraries;
using Giantnodes.Service.Dashboard.Domain.Aggregates.Libraries.Services.Impl;
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
        var service = new FileSystemService(FileSystem);

        var file = FileSystem.FileInfo.New(path);
        if (file.Directory == null)
            throw new DirectoryNotFoundException();

        var library = new Library(service, file.Directory, "Silicon Valley", "silicon-valley");
        var parent = new FileSystemDirectory(library, null, file.Directory, service);

        // act
        var entry = FileSystemEntry.Build(library, parent, file, service);

        // assert
        Assert.IsType<FileSystemFile>(entry);
    }

    [Theory]
    [MemberData(nameof(GetDirectories), parameters: 5)]
    public void Should_Build_FileSystemDirectory(string path)
    {
        // arrange
        var service = new FileSystemService(FileSystem);

        var directory = FileSystem.DirectoryInfo.New(path);
        if (directory.Parent == null)
            throw new DirectoryNotFoundException();

        var library = new Library(service, directory.Parent, "Silicon Valley", "silicon-valley");
        var parent = new FileSystemDirectory(library, null, directory.Parent, service);

        // act
        var entry = FileSystemEntry.Build(library, parent, directory, service);

        // assert
        Assert.IsType<FileSystemDirectory>(entry);
    }
}