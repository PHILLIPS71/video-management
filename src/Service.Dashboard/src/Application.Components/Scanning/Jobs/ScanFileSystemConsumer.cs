using System.IO.Abstractions;
using Giantnodes.Service.Dashboard.Application.Contracts.Scanning.Commands;
using Giantnodes.Service.Dashboard.Domain.Entities;
using Giantnodes.Service.Dashboard.Domain.Entities.Files;
using Giantnodes.Service.Dashboard.Persistence.DbContexts;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace Giantnodes.Service.Dashboard.Application.Components.Scanning.Jobs;

public class ScanFileSystemConsumer : IConsumer<ScanFileSystem.Command>
{
    private readonly ApplicationDbContext _database;
    private readonly IFileSystem _system;

    public ScanFileSystemConsumer(ApplicationDbContext database, IFileSystem system)
    {
        _database = database;
        _system = system;
    }

    public async Task Consume(ConsumeContext<ScanFileSystem.Command> context)
    {
        // prevent non-library paths from being scanned and eventually stored in the database
        var isLibraryPath = await _database
            .Libraries
            .AnyAsync(x => context.Message.FullPath.StartsWith(x.FullPath), context.CancellationToken);

        if (isLibraryPath == false)
        {
            await context.RejectAsync<ScanFileSystem.Rejected, ScanFileSystem.Rejection>(ScanFileSystem.Rejection.LibraryNotFound);
            return;   
        }
        
        // continue to check if the path can be found as a nested library directory could be scanned that does not exist
        var root = _system.DirectoryInfo.New(context.Message.FullPath);
        if (root.Exists == false)
        {
            await context.RejectAsync<ScanFileSystem.Rejected, ScanFileSystem.Rejection>(ScanFileSystem.Rejection.DirectoryNotFound);
            return;
        }
        
        var nodes = await _database
            .Nodes
            .Include(x => x.ParentDirectory)
            .Where(x => x.FullPath.StartsWith(context.Message.FullPath))
            .ToListAsync(context.CancellationToken);

        var infos = root
            .GetFileSystemInfos("*", SearchOption.AllDirectories)
            .ToList();

        var directories = infos
            .OfType<IDirectoryInfo>()
            .ToList();

        var files = infos
            .OfType<IFileInfo>()
            .ToList();

        // remove any nodes that no longer exist within the scanned path
        nodes.RemoveAll(x => infos.Any(y => y.FullName == x.FullPath) == false);

        foreach (var directory in directories)
        {
            var node = nodes
                .OfType<FileSystemDirectory>()
                .SingleOrDefault(x => x.FullPath == directory.FullName);

            if (node == null)
            {
                node = new FileSystemDirectory { FullPath = directory.FullName, Name = directory.Name };
                nodes.Add(node);
            }

            node.ParentDirectory ??= FindOrCreateParentDirectory(directories, nodes, directory.Parent?.FullName);
        }

        foreach (var file in files)
        {
            var node = nodes
                .OfType<FileSystemFile>()
                .SingleOrDefault(x => x.FullPath == file.FullName);

            if (node == null)
            {
                node = new FileSystemFile { FullPath = file.FullName, Name = file.Name };
                nodes.Add(node);
            }

            node.ParentDirectory ??= FindOrCreateParentDirectory(directories, nodes, file.DirectoryName);
            node.Size = file.Length;
        }

        _database.Nodes.AddRange(nodes.Where(x => x.Id == default));
        await _database.SaveChangesAsync(context.CancellationToken);
        
        await context.RespondAsync<ScanFileSystem.Result>(new { context.Message.FullPath });
    }

    private static FileSystemDirectory? FindOrCreateParentDirectory(IEnumerable<IDirectoryInfo> directories, IEnumerable<FileSystemNode> nodes, string? path)
    {
        var parent = nodes
            .OfType<FileSystemDirectory>()
            .SingleOrDefault(x => x.FullPath == path);

        if (parent != null)
            return parent;

        var directory = directories.SingleOrDefault(x => x.FullName == path);
        if (directory != null)
            parent = new FileSystemDirectory { FullPath = directory.FullName, Name = directory.Name };

        return parent;
    }
}