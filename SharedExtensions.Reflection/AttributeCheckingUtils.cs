using System.Diagnostics;
using System.Reflection;

namespace System;

internal static class AttributeCheckingUtils
{
    [DebuggerStepThrough]
    public static bool HasAttribute<TAttribute>(this Type type) where TAttribute : Attribute
    {
        return type != null && type.GetCustomAttribute<TAttribute>() != null;
    }

    [DebuggerStepThrough]
    public static bool HasAttribute<TAttribute>(this Type type, out TAttribute attribute) where TAttribute : Attribute
    {
        attribute = null;

        var customAttribute = type?.GetCustomAttribute<TAttribute>();

        if (customAttribute != null)
        {
            attribute = customAttribute;
        }

        return customAttribute != null;
    }

    [DebuggerStepThrough]
    public static bool HasAttribute<TAttribute>(this MemberInfo type) where TAttribute : Attribute
    {
        return type != null && type.GetCustomAttribute<TAttribute>() != null;
    }

    [DebuggerStepThrough]
    public static bool HasProperty<TType>(this Type type, string propertyName)
    {
        if (type == null)
            throw new ArgumentNullException(nameof(type));

        var property = type.GetProperties()
                           .FirstOrDefault(_ => _.Name == propertyName && _.PropertyType == typeof(TType));

        return property != null;
    }
}