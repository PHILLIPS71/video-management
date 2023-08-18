﻿using Giantnodes.Service.Dashboard.Domain.Aggregates.Libraries.Entities;
using Giantnodes.Service.Dashboard.Domain.Aggregates.Libraries.Factories;
using Giantnodes.Service.Dashboard.Tests.Shared.Fixtures;
using Xunit;

namespace Giantnodes.Service.Dashboard.Domain.Tests.Aggregates.Factories;

public class FileSystemEntryFactoryTests : FileSystemFixture
{
    [Theory]
    [MemberData(nameof(GetFiles), parameters: 5)]
    public void Should_Build_FileSystemFile(string path)
    {
        // arrange
        var file = FileSystem.FileInfo.New(path);
        if (file.Directory == null)
            throw new DirectoryNotFoundException();

        var parent = new FileSystemDirectory(null, file.Directory);

        // act
        var entry = FileSystemEntryFactory.Build(parent, file);

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

        var parent = new FileSystemDirectory(null, directory.Parent);

        // act
        var entry = FileSystemEntryFactory.Build(parent, directory);

        // assert
        Assert.IsType<FileSystemDirectory>(entry);
    }
}