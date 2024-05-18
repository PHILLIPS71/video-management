using Giantnodes.Infrastructure.Domain.Values;
using Giantnodes.Service.Dashboard.Domain.Shared.Enums;

namespace Giantnodes.Service.Dashboard.Domain.Values;

public class Machine : ValueObject
{
    public string Name { get; init; }

    public string UserName { get; init; }

    public ProcessorType ProcessorType { get; init; }

    protected Machine()
    {
    }

    public Machine(string name, string username, ProcessorType processor)
    {
        Name = name;
        UserName = username;
        ProcessorType = processor;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Name;
        yield return UserName;
        yield return ProcessorType;
    }
}