using System.Linq.Expressions;
using System.Reflection;

namespace Giantnodes.Infrastructure.EntityFrameworkCore;

public class ObjectHelper
{
    public static void SetProperty<TObject, TValue>(
        TObject obj,
        Expression<Func<TObject, TValue>> property,
        Func<TObject, TValue> value) where TObject : class
    {
        if (property.Body is not MemberExpression expression)
            return;

        const BindingFlags flags = BindingFlags.Instance |
                                   BindingFlags.Public |
                                   BindingFlags.NonPublic |
                                   BindingFlags.SetProperty;

        var parent = obj.GetType();
        while (parent != null)
        {
            var info = parent.GetProperty(expression.Member.Name, flags);
            if (info == null)
                throw new ArgumentOutOfRangeException(expression.Member.Name,
                    $"Property {expression.Member.Name} was not found in Type {parent.FullName}");

            if (info.CanWrite)
                break;

            parent = obj.GetType().BaseType;
        }

        parent?.InvokeMember(expression.Member.Name, flags, Type.DefaultBinder, obj, new object?[] { value(obj) });
    }
}