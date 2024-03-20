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

    /// <summary>
    /// Throws an exception if the specified date occurs in the future.
    /// </summary>
    /// <param name="guard">The guard clause instance.</param>
    /// <param name="input">The date to validate.</param>
    /// <param name="parameter">The name of the parameter being validated (optional).</param>
    /// <param name="message">The custom error message to use (optional).</param>
    /// <returns>The input date if it is not in the future.</returns>
    /// <exception cref="ArgumentException">Thrown if the input date occurs in the future.</exception>
    public static DateTime FutureDate(
        this IGuardClause guard,
        DateTime input,
        [CallerArgumentExpression("input")] string? parameter = null,
        string? message = null)
    {
        if (input > DateTime.UtcNow)
            throw new ArgumentException(message ?? $"{nameof(input)} cannot occur in the future.", parameter);

        return input;
    }
}