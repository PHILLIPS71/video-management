using Giantnodes.Infrastructure.Domain.Values;

namespace Giantnodes.Service.Orchestrator.Domain.Values;

public abstract class FileStream : ValueObject
{
    public int Index { get; init; }

    public string Codec { get; init; }

    protected FileStream()
    {
    }

    protected FileStream(int index, string codec)
    {
        Index = index;
        Codec = codec;
    }
}