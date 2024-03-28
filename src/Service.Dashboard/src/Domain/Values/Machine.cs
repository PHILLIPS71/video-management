using Giantnodes.Infrastructure.Domain.Values;

namespace Giantnodes.Service.Dashboard.Domain.Values;

public class Machine : ValueObject
{
    public string Name { get; init; }

    public string UserName { get; init; }

    protected Machine()
    {
    }

    public Machine(string name, string username)
    {
        Name = name;
        UserName = username;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Name;
        yield return UserName;
    }
}