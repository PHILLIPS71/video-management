using System.Reflection;

namespace Giantnodes.Infrastructure;

/// <summary>
/// Abstract base class for enumerations with an integer identifier and a name.
/// </summary>
public abstract class Enumeration : IComparable
{
    /// <summary>
    /// Gets the identifier of the enumeration.
    /// </summary>
    public int Id { get; init; }

    /// <summary>
    /// Gets the name of the enumeration.
    /// </summary>
    public string Name { get; init; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Enumeration"/> class.
    /// </summary>
    /// <param name="id">The identifier of the enumeration.</param>
    /// <param name="name">The name of the enumeration.</param>
    protected Enumeration(int id, string name)
    {
        Id = id;
        Name = name;
    }

    /// <inheritdoc/>
    public override string ToString() => Name;

    /// <inheritdoc/>
    public override int GetHashCode() => Id.GetHashCode();

    /// <inheritdoc/>
    public override bool Equals(object? obj)
    {
        if (obj is not Enumeration enumeration)
            return false;

        return GetType() == obj.GetType() && Id.Equals(enumeration.Id);
    }

    /// <summary>
    /// Gets all values of the enumeration type.
    /// </summary>
    /// <typeparam name="T">The type of the enumeration.</typeparam>
    /// <returns>An IEnumerable containing all values of the enumeration type.</returns>
    public static IEnumerable<T> GetAll<T>() where T : Enumeration
    {
        return typeof(T).GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
            .Select(f => f.GetValue(null))
            .Cast<T>();
    }

    /// <inheritdoc/>
    public int CompareTo(object? obj)
    {
        if (obj is not Enumeration enumeration)
            throw new ArgumentException($"'{obj}' is not of type {typeof(Enumeration)}");

        return Id.CompareTo(enumeration.Id);
    }

    /// <summary>
    /// Parses an enumeration value based on a predicate.
    /// </summary>
    /// <typeparam name="TEnumeration">The type of the enumeration.</typeparam>
    /// <typeparam name="TValue">The type of the value used for parsing.</typeparam>
    /// <param name="value">The value to parse.</param>
    /// <param name="predicate">The predicate to match against enumeration items.</param>
    /// <returns>The matching enumeration item.</returns>
    public static TEnumeration Parse<TEnumeration, TValue>(TValue value, Func<TEnumeration, bool> predicate)
        where TEnumeration : Enumeration
    {
        var match = GetAll<TEnumeration>().FirstOrDefault(predicate);
        if (match == null)
            throw new InvalidOperationException($"'{value}' is not a valid name in {typeof(TEnumeration)}");

        return match;
    }

    /// <summary>
    /// Attempts to parse an enumeration value based on its identifier.
    /// </summary>
    /// <typeparam name="TValue">The type of the enumeration.</typeparam>
    /// <param name="id">The identifier to parse.</param>
    /// <returns>The parsed enumeration item, or null if parsing fails.</returns>
    public static TValue? TryParse<TValue>(int id)
        where TValue : Enumeration
    {
        try
        {
            return Parse<TValue, int>(id, item => item.Id == id);
        }
        catch (InvalidOperationException)
        {
            return null;
        }
    }
    
    /// <summary>
    /// Attempts to parse an enumeration value based on its name.
    /// </summary>
    /// <typeparam name="TValue">The type of the enumeration.</typeparam>
    /// <param name="name">The name to parse.</param>
    /// <returns>The parsed enumeration item, or null if parsing fails.</returns>
    public static TValue? TryParse<TValue>(string name)
        where TValue : Enumeration
    {
        try
        {
            return Parse<TValue, string>(name, item => item.Name == name);
        }
        catch (InvalidOperationException)
        {
            return null;
        }
    }
}