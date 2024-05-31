using System.Text.Json;
using System.Text.Json.Serialization;

namespace DotNetBrightener.Utils.MessageCompression;

internal static class JsonSerializerSettings
{
    public static readonly JsonSerializerOptions SerializeOptions = new()
    {
        PropertyNamingPolicy   = JsonNamingPolicy.CamelCase,
        DictionaryKeyPolicy    = JsonNamingPolicy.CamelCase,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        ReferenceHandler       = ReferenceHandler.IgnoreCycles
    };

    public static readonly JsonSerializerOptions DeserializeOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        DefaultIgnoreCondition      = JsonIgnoreCondition.WhenWritingNull
    };
}