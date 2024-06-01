﻿using Giantnodes.Service.Orchestrator.Domain.Aggregates.Entries;
using Giantnodes.Service.Orchestrator.Domain.Aggregates.Entries.Directories;
using Giantnodes.Service.Orchestrator.Domain.Aggregates.Entries.Files;
using Giantnodes.Service.Orchestrator.Domain.Aggregates.Libraries;
using Giantnodes.Service.Orchestrator.Tests.Shared.Fixtures;
using Xunit;

namespace Giantnodes.Service.Orchestrator.Domain.Tests.Aggregates.Entries;

public class FileSystemEntryTests : FileSystemFixture
{
    [Theory]
    [MemberData(nameof(GetFiles), parameters: 5)]
    public void Build_ShouldCreateFileSystemFile_WhenGivenFilePath(string path)
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
    public void Build_ShouldCreateFileSystemDirectory_WhenGivenDirectoryPath(string path)
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