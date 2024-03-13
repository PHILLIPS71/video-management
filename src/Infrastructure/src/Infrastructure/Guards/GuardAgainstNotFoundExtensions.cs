using System.Runtime.CompilerServices;
using Giantnodes.Infrastructure;

namespace Ardalis.GuardClauses;

public static class GuardAgainstNotFoundExtensions
{
    public static T NotFound<T>(this IGuardClause guard, IEnumerable<T> collection, T input)
        where T : Enumeration
    {
        guard.Null(input);

        if (!collection.Contains(input))
            throw new NotFoundException(input.Id.ToString(), nameof(collection));

        return input;
    }
}