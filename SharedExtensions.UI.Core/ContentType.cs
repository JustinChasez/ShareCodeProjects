namespace DotNetBrightener.Extensions.ProblemsResult.UI;

internal class ContentType
{
    public const string JAVASCRIPT = "text/javascript";
    public const string CSS        = "text/css";
    public const string HTML       = "text/html";
    public const string PLAIN      = "text/plain";
    public const string MARKDOWN   = "text/markdown; charset=UTF-8";

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