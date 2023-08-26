using System.IO.Abstractions;
using Giantnodes.Infrastructure.Domain.Values;

namespace Giantnodes.Service.Dashboard.Domain.Values;

public class PathInfo : ValueObject
{
    public string Name { get; init; } = null!;

    public string FullName { get; init; } = null!;

    public string? Extension { get; init; }

    public string? DirectoryPath { get; init; }

    public char DirectorySeparatorChar { get; init; }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Name;
        yield return FullName;
        yield return DirectorySeparatorChar;
    }

    protected PathInfo()
    {
    }

    public PathInfo(IFileSystemInfo info)
    {
        Name = info.Name;
        FullName = info.FullName;
        Extension = string.IsNullOrWhiteSpace(info.Extension) ? null : info.Extension;
        DirectoryPath = Path.GetDirectoryName(info.FullName);
        DirectorySeparatorChar = Path.DirectorySeparatorChar;
    }
}