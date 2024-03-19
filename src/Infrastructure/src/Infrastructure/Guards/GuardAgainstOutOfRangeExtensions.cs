using System.ComponentModel;
using System.Runtime.CompilerServices;
using Giantnodes.Infrastructure;

namespace Ardalis.GuardClauses;

public static class GuardAgainstOutOfRangeExtensions
{
    public static T OutOfRange<T>(
        this IGuardClause guard,
        T input,
        [CallerArgumentExpression("input")] string? parameter = null,
        string? message = null)
        where T : Enumeration
    {
        if (Enumeration.TryParse<T>(input.Id) == null)
        {
            if (string.IsNullOrEmpty(message))
                throw new InvalidEnumArgumentException(parameter, Convert.ToInt32(input.Id), typeof(T));

            throw new InvalidEnumArgumentException(message);
        }

        return input;
    }
}