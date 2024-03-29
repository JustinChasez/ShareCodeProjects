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

namespace DotNetBrightener.Extensions.ProblemsResult.UI;

internal class UIResource
{
    public string Content     { get; internal set; }
    public string ContentType { get; }
    public string FileName    { get; }

    public UIResource(string fileName, string content, string contentType)
    {
        Content     = Guard.ThrowIfNull(content);
        ContentType = Guard.ThrowIfNull(contentType);
        FileName    = Guard.ThrowIfNull(fileName);
    }

    public static UIResource Create(string fileName, string content, string contentType)
    {
        return new UIResource(fileName, content, contentType);
    }
}