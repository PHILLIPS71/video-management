using System.Text.RegularExpressions;

namespace System;

public static partial class StringExtensions
{
    [GeneratedRegex("^[a-z0-9]+(?:-[a-z0-9]+)*$")]
    private static partial Regex SlugExpression();

    public static bool IsSlug(this string value)
    {
        return SlugExpression().IsMatch(value);
    }
}