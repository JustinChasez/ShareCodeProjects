/***
 *
 * Copyright (c) 2021 DotNetBrightener.
 * Licensed under MIT.
 * Feel free to use!!
 * https://gist.github.com/JustinChasez/b6bdde4511f6b641d2d9969e262b7df8
 ***/

using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

// ReSharper disable once CheckNamespace
namespace System;

internal static class StringFormatExtensions
{
    /// <summary>
    ///     Retrieves a formatted string from the pre-defined template string and the object to translate into the output
    /// </summary>
    /// <remarks>
    ///     Given an object as :
    ///     var obj = new {
    ///         propertyA = "value 1"
    ///     };
    ///
    ///     And a string
    ///     var templateString = "This is the formatted string: {propertyA}";
    ///
    ///     Calling the method as extensions invoke: templateString.FormatWith(obj);
    ///     Retrieve: "This is the formatted string: value 1"
    /// </remarks>
    /// <param name="templateString">The string contains template</param>
    /// <param name="inputObject">The object to format with the template string</param>
    /// <returns>
    ///     The formatted string
    /// </returns>
    public static string FormatWith(this string templateString, object inputObject, IFormatProvider provider = null)
    {
        if (templateString == null)
            throw new ArgumentNullException("templateString");

        var r = new Regex(@"(?<start>\{)+(?<property>[\w\.\[\]]+)(?<format>:[^}]+)?(?<end>\})+",
                          RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);

        var values = new List<object>();

        var rewrittenFormat = r.Replace(templateString, delegate (Match m)
        {
            var startGroup    = m.Groups["start"];
            var propertyGroup = m.Groups["property"];
            var formatGroup   = m.Groups["format"];
            var endGroup      = m.Groups["end"];

            var objectValue = propertyGroup.Value == "0"
                                  ? inputObject
                                  : Eval(inputObject, propertyGroup.Value);

            values.Add(objectValue);

            return new string('{', startGroup.Captures.Count) +
                   (values.Count - 1) + formatGroup.Value
                 + new string('}', endGroup.Captures.Count);
        });

        return string.Format(provider, rewrittenFormat, values.ToArray());
    }

    private static object Eval(object source, string propertyName)
    {
        var prop = source.GetType()
                         .GetProperties()
                         .FirstOrDefault(_ => _.Name == propertyName);

        if (prop == null)
            return null;

        return prop.GetValue(source);
    }
}