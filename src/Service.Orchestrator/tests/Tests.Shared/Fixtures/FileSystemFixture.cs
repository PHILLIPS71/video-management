using System.IO.Abstractions.TestingHelpers;
using Xunit;

namespace Giantnodes.Service.Orchestrator.Tests.Shared.Fixtures;

public class FileSystemFixture : IDisposable
{
    /// <summary>
    /// A normalized root path of a mock file system directory the contains tv-shows
    /// </summary>
    protected static readonly string RootPath = MockUnixSupport.Path(@"C:\tv-shows");

    /// <summary>
    /// A mock file system that contains a set of <see cref="MockFileData" /> and <seealso cref="MockDirectoryData" />
    /// that are stored within the <seealso cref="RootPath" /> directory.
    /// </summary>
    protected static readonly MockFileSystem FileSystem = new (new Dictionary<string, MockFileData>
    {
        { MockUnixSupport.Path(@$"{RootPath}\Silicon Valley"), new MockDirectoryData() },
        { MockUnixSupport.Path(@$"{RootPath}\Silicon Valley\Season 1"), new MockDirectoryData() },
        { MockUnixSupport.Path(@$"{RootPath}\Silicon Valley\Season 1\.DS_Store"), new MockFileData(string.Empty) },
        { MockUnixSupport.Path(@$"{RootPath}\Silicon Valley\Season 1\poster.png"), new MockFileData(string.Empty) },
        { MockUnixSupport.Path(@$"{RootPath}\Silicon Valley\Season 1\Silicon Valley - S01E01 - Minimum Viable Product.mp4"), new MockFileData(string.Empty) },
        { MockUnixSupport.Path(@$"{RootPath}\Silicon Valley\Season 1\Silicon Valley - S01E02 - The Cap Table.mp4"), new MockFileData(string.Empty) },
        { MockUnixSupport.Path(@$"{RootPath}\Silicon Valley\Season 1\Silicon Valley - S01E03 - Articles of Incorporation.mkv"), new MockFileData(string.Empty) },
        { MockUnixSupport.Path(@$"{RootPath}\Silicon Valley\Season 1\Silicon Valley - S01E04 - Fiduciary Duties.mkv"), new MockFileData(string.Empty) },
        { MockUnixSupport.Path(@$"{RootPath}\Silicon Valley\Season 1\Silicon Valley - S01E05 - Signaling Risk.avi"), new MockFileData(string.Empty) },
        { MockUnixSupport.Path(@$"{RootPath}\Silicon Valley\Season 1\Silicon Valley - S01E06 - Third Party Insourcing.avi"), new MockFileData(string.Empty) },
        { MockUnixSupport.Path(@$"{RootPath}\Silicon Valley\Season 1\Silicon Valley - S01E07 - Proof of Concept.mov"), new MockFileData(string.Empty) },
        { MockUnixSupport.Path(@$"{RootPath}\Silicon Valley\Season 1\Silicon Valley - S01E08 - Optimal Tip-to-Tip Efficiency.mov"), new MockFileData(string.Empty) }
    });

    /// <summary>
    /// A collection of all the files and directories within the <see cref="RootPath" /> directory.
    /// </summary>
    protected static IEnumerable<string> AllEntries =>
        FileSystem
            .DirectoryInfo
            .New(RootPath)
            .GetFileSystemInfos("*", SearchOption.AllDirectories)
            .Select(x => x.FullName);

    /// <summary>
    /// A collection of all the files within the <see cref="RootPath" /> directory.
    /// </summary>
    protected static IEnumerable<string> AllFiles =>
        FileSystem
            .DirectoryInfo
            .New(RootPath)
            .GetFiles("*", SearchOption.AllDirectories)
            .Select(x => x.FullName);

    /// <summary>
    /// A collection of all the directories within the <see cref="RootPath" /> directory.
    /// </summary>
    protected static IEnumerable<string> AllDirectories =>
        FileSystem
            .DirectoryInfo
            .New(RootPath)
            .GetDirectories("*", SearchOption.AllDirectories)
            .Select(x => x.FullName);

    /// <summary>
    /// A collection of the top level directories within the <see cref="RootPath" /> directory.
    /// </summary>
    protected static IEnumerable<string> TopLevelDirectories =>
        FileSystem
            .DirectoryInfo
            .New(RootPath)
            .GetDirectories("*", SearchOption.TopDirectoryOnly)
            .Select(x => x.FullName);

    /// <summary>
    /// A collection of file paths within the <see cref="RootPath" /> directory which can be used as a data source for
    /// a <seealso cref="MemberDataAttribute" /> theory.
    /// </summary>
    /// <param name="limit">The maximum amount of paths that can be returned.</param>
    /// <returns>A collection of <see cref="FileSystemInfo.FullName"/> values represented as objects.</returns>
    public static IEnumerable<object[]> GetFiles(int limit = 5) =>
        FileSystem
            .DirectoryInfo
            .New(RootPath)
            .GetFiles("*", SearchOption.AllDirectories)
            .Select(x => new[] { x.FullName })
            .Take(limit);

    /// <summary>
    /// A collection of directory paths within the <see cref="RootPath" /> directory which can be used as a data source
    /// for a <seealso cref="MemberDataAttribute" /> theory.
    /// </summary>
    /// <param name="limit">The maximum amount of paths that can be returned.</param>
    /// <returns>A collection of <see cref="FileSystemInfo.FullName"/> values represented as objects.</returns>
    public static IEnumerable<object[]> GetDirectories(int limit = 5) =>
        FileSystem
            .DirectoryInfo
            .New(RootPath)
            .GetDirectories("*", SearchOption.AllDirectories)
            .Select(x => new[] { x.FullName })
            .Take(limit);

    /// <summary>
    /// Disposes the resources used by the <see cref="FileSystemFixture"/> instance.
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
    
    protected virtual void Dispose(bool dispose)
    {
    }
}