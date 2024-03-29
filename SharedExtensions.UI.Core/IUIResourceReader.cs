using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace DotNetBrightener.Extensions.ProblemsResult.UI;

internal interface IUIResourcesReader
{
    IEnumerable<UIResource> UIResources { get; }
}