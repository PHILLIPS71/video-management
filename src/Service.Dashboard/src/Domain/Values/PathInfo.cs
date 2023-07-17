using System.IO.Abstractions;
using Giantnodes.Infrastructure.Domain.Values;

namespace Giantnodes.Service.Dashboard.Domain.Values;

public class PathInfo : ValueObject
{
    public string Name { get; init; }
    
    public string FullName { get; init; }

    public string? Extension { get; init; }

    public string? DirectoryPath { get; init; }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Name;
        yield return FullName;
    }

    protected PathInfo()
    {
    }
    
    public PathInfo(IFileSystemInfo info)
    {
        Name = info.Name;
        FullName = info.FullName;
        Extension = info.Extension;
        DirectoryPath = Path.GetDirectoryName(info.FullName);
    }
}