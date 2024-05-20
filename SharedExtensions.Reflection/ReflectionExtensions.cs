using System.Diagnostics;
using System.Reflection;
using System.Text.RegularExpressions;

namespace System;

internal static class ReflectionExtensions
{
    internal static string AssemblySkipLoadingPattern =
        "^Anonymously|^System|^mscorlib|^Microsoft|^AjaxControlToolkit|^Antlr3|" +
        "^Autofac|^AutoMapper|^Castle|^ComponentArt|^CppCodeProvider|" +
        "^DotNetOpenAuth|^EPPlus|^FluentValidation|" +
        "^ImageResizer|^itextsharp|^log4net|^MaxMind|^MbUnit|^MiniProfiler|" +
        "^Mono.Math|^MvcContrib|^Newtonsoft|^NHibernate|^nunit|^Org.Mentalis|" +
        "^PerlRegex|^QuickGraph|^Recaptcha|^Remotion|^RestSharp|^Rhino|^Telerik|" +
        "^Iesi|^TestDriven|^TestFu|^UserAgentStringLibrary|^VJSharpCodeProvider|" +
        "^WebActivator|^WebDev|^WebGrease|^NLog|^Handlebars|" +
        "^Npgsql|^Markdig|^LinqToDb|^Twilio";



    public static List<string> GetAvailableFieldNames(this Type    type,
                                                      string       name          = "",
                                                      List<string> recursiveList = null,
                                                      int          level         = 0)
    {
        if (recursiveList == null)
            recursiveList = new List<string>();

        if (level > 3)
            return recursiveList;

        var properties = type.GetProperties();

        foreach (var property in properties)
        {
            recursiveList.Add($"{name}.{property.Name}".Trim('.'));

            if (property.PropertyType.IsClass &&
                property.PropertyType.IsNotSystemType())
            {
                GetAvailableFieldNames(property.PropertyType,
                                       $"{name}.{property.Name}".Trim('.'),
                                       recursiveList,
                                       level + 1);
            }
        }

        return recursiveList;
    }

    /// <summary>
    ///     Skips the assemblies are usually system assemblies for from third-party libraries
    /// </summary>
    /// <param name="assemblies"></param>
    /// <returns></returns>
    public static IEnumerable<Assembly> FilterSkippedAssemblies(this IEnumerable<Assembly> assemblies)
    {
        return assemblies.Where(x => !IsMatch(x.FullName,
                                              AssemblySkipLoadingPattern,
                                              RegexOptions.IgnoreCase | RegexOptions.Compiled));
    }

    public static bool IsNotSystemType(this Type type)
    {
        return type.Namespace == null || !IsMatch(type.Namespace,
                                                  AssemblySkipLoadingPattern,
                                                  RegexOptions.IgnoreCase | RegexOptions.Compiled);
    }

    public static Type[] GetDerivedTypes<TParent>(this IEnumerable<Assembly> assemblies)
    {
        return GetDerivedTypes(assemblies, typeof(TParent)).ToArray();
    }

    public static Type[] GetDerivedTypes<TParent>(this Assembly assembly)
    {
        return GetDerivedTypes(new[]
                               {
                                   assembly
                               },
                               typeof(TParent))
           .ToArray();
    }

    public static List<Type> GetDerivedTypes(this IEnumerable<Assembly> bundledAssemblies, Type t)
    {
        var result = new List<Type>();

        foreach (var assembly in bundledAssemblies)
        {
            Type[] types;

            try
            {
                types = assembly.GetTypes();
            }
            catch (ReflectionTypeLoadException exception)
            {
                types = exception.Types
                                 .Where(_ => _ is not null)
                                 .ToArray();
            }

            foreach (Type type in types.Where(t => t is not null))
            {
                try
                {
                    if ((t.IsAssignableFrom(type) ||
                         type!.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == t)) &&
                        !type.IsAbstract &&
                        !type.IsInterface)
                    {
                        result.Add(type);
                    }
                }
                catch (Exception err)
                {
                }
            }
        }

        return result;
    }

    public static bool IsNullable(this Type type)
    {
        return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
    }



    [DebuggerStepThrough]
    private static bool IsMatch(this string  input,
                                string       pattern,
                                RegexOptions options = RegexOptions.IgnoreCase | RegexOptions.Multiline)
    {
        return Regex.IsMatch(input, pattern, options);
    }

    [DebuggerStepThrough]
    private static bool IsMatch(this string  input,
                                string       pattern,
                                out Match    match,
                                RegexOptions options = RegexOptions.IgnoreCase | RegexOptions.Multiline)
    {
        match = Regex.Match(input, pattern, options);

        return match.Success;
    }
}