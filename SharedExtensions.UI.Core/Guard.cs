// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//                         _oo0oo_
//                        o8888888o
//                        88" . "88
//                       (| - _ - |)
//                        0\  =  /0
//                      ___/`---'\___
//                    .' \\|        |// '.
//                   / \\|||    :   |||// \
//                  /  _|||||  -:-  |||||-\
//                 |   | \\\  -  /// |    |
//                 | \_|  ''\---/''  |_/  |
//                 \  .-\__  '-'  ___/-. /
//               ___'. .'  /--.--\  `. .'___
//            ."" '<  `.___\_<|>_/___.' >' "".
//           | | :  `- \`.;`\ _ /`;.`/ - ` : | |
//           \  \ `_.   \_ __\ /__ _/   .-` /  /
//       =====`-.____`.___ \_____/___.-`___.-'=====
//                         `=---='
// 
//       ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace DotNetBrightener.Extensions.ProblemsResult.UI;

internal static class Guard
{
    /// <summary>Throws an <see cref="ArgumentNullException"/> if <paramref name="argument"/> is null.</summary>
    /// <param name="argument">The reference type argument to validate as non-null.</param>
    /// <param name="throwOnEmptyString">Only applicable to strings.</param>
    /// <param name="paramName">The name of the parameter with which <paramref name="argument"/> corresponds.</param>
    public static T ThrowIfNull<T>([NotNull] T?                                   argument,
                                   bool                                           throwOnEmptyString = false,
                                   [CallerArgumentExpression("argument")] string? paramName          = null)
        where T : class
    {
#if NET6_0_OR_GREATER
        ArgumentNullException.ThrowIfNull(argument, paramName);

        if (throwOnEmptyString &&
            argument is string s &&
            string.IsNullOrEmpty(s))
            throw new ArgumentNullException(paramName);
#else
        if (argument is null || throwOnEmptyString && argument is string s && string.IsNullOrEmpty(s))
            throw new ArgumentNullException(paramName);
#endif
        return argument;
    }
}