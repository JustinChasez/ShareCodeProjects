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
using System.Collections.Generic;

namespace DotNetBrightener.Extensions.ProblemsResult.UI;

internal class ContentType
{
    public const string JAVASCRIPT = "text/javascript";
    public const string CSS        = "text/css";
    public const string HTML       = "text/html";
    public const string PLAIN      = "text/plain";
    public const string MARKDOWN      = "text/markdown; charset=UTF-8";

    public static Dictionary<string, string> supportedContent =
        new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase)
        {
            {
                "js", JAVASCRIPT
            },
            {
                "html", HTML
            },
            {
                "css", CSS
            },
            {
                "md", MARKDOWN
            },
        };

    public static string FromExtension(string fileExtension)
        => supportedContent.TryGetValue(fileExtension, out var result) ? result : PLAIN;
}