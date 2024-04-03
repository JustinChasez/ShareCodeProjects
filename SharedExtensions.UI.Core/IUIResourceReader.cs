namespace DotNetBrightener.Extensions.ProblemsResult.UI;

internal interface IUIResourcesReader
{
    IEnumerable<UIResource> UIResources { get; }
}