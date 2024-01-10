using System.Reflection;

namespace Giantnodes.Infrastructure;

public abstract class Enumeration : IComparable
{
    public int Id { get; init; }

    public string Name { get; init; }

    protected Enumeration(int id, string name)
    {
        Id = id;
        Name = name;
    }

    public override string ToString() => Name;

    public override int GetHashCode() => Id.GetHashCode();

    public override bool Equals(object? obj)
    {
        if (obj is not Enumeration enumeration)
            return false;

        return GetType() == obj.GetType() && Id.Equals(enumeration.Id);
    }

    public static IEnumerable<T> GetAll<T>() where T : Enumeration
    {
        return typeof(T).GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
            .Select(f => f.GetValue(null))
            .Cast<T>();
    }

    public int CompareTo(object? obj)
    {
        if (obj is not Enumeration enumeration)
            throw new ArgumentException($"'{obj}' is not of type {typeof(Enumeration)}");

        return Id.CompareTo(enumeration.Id);
    }

    public static TEnumeration Parse<TEnumeration, TValue>(TValue value, Func<TEnumeration, bool> predicate)
        where TEnumeration : Enumeration
    {
        var match = GetAll<TEnumeration>().FirstOrDefault(predicate);
        if (match == null)
            throw new InvalidOperationException($"'{value}' is not a valid name in {typeof(TEnumeration)}");

        return match;
    }

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