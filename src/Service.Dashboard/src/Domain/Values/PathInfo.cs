using System.IO.Abstractions;
using Giantnodes.Infrastructure;
using Giantnodes.Infrastructure.Domain.Values;
using Giantnodes.Service.Dashboard.Domain.Enumerations;
using Giantnodes.Service.Dashboard.Domain.Shared;

namespace Giantnodes.Service.Dashboard.Domain.Values;

public class PathInfo : ValueObject
{
    public string Name { get; init; }
    
    public string FullName { get; init; }

    public MediaFileExtension? Extension { get; init; }

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
        Extension = Enumeration.TryParse<MediaFileExtension>(info.Extension);
        DirectoryPath = Path.GetDirectoryName(info.FullName);
    }
}