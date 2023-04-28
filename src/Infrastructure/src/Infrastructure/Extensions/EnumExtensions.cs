using System.ComponentModel;
using System.Reflection;

namespace Giantnodes.Infrastructure.Extensions
{
    public static class EnumExtensions
    {
        public static string GetStringValue(this Enum value)
        {
            var type = value.GetType();

            var info = type.GetField(value.ToString());
            var attribute = info?.GetCustomAttributes(typeof(DescriptionAttribute), false) as DescriptionAttribute[] ??
                            Array.Empty<DescriptionAttribute>();

            return attribute.Any() ? attribute.First().Description : string.Empty;
        }
    }
}
