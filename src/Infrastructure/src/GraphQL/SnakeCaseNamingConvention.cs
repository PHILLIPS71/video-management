using System.Reflection;
using System.Text.RegularExpressions;
using HotChocolate.Types;
using HotChocolate.Types.Descriptors;

namespace Giantnodes.Infrastructure.GraphQL;

public partial class SnakeCaseNamingConvention : DefaultNamingConventions
{
    [GeneratedRegex("[A-Z]{2,}(?=[A-Z][a-z]+[0-9]*|\\b)|[A-Z]?[a-z]+[0-9]*|[A-Z]|[0-9]+")]
    private static partial Regex SnakeCaseRegexPattern();

    public override string GetMemberName(MemberInfo member, MemberKind kind)
    {
        if (kind is not (MemberKind.ObjectField or MemberKind.InterfaceField or MemberKind.InputObjectField))
            return base.GetMemberName(member, kind);

        var pattern = SnakeCaseRegexPattern();
        return string.Join("_", pattern.Matches(member.Name)).ToLower();
    }

    public override string GetEnumValueName(object value)
    {
        var input = value.ToString();
        if (input == null)
            return base.GetEnumValueName(value);

        var pattern = SnakeCaseRegexPattern();
        return string.Join("_", pattern.Matches(input)).ToUpper();
    }
}
