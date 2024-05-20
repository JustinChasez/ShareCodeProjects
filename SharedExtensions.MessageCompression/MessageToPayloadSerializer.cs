using System.Text.Json;

namespace DotNetBrightener.Utils.MessageCompression;

internal static class MessageToPayloadSerializer
{
    public static Dictionary<string, object> ToPayload<T>(this T entity) where T : class, new()
    {
        var jsonPayload = JsonSerializer.Serialize(entity, JsonSerializerSettings.SerializeOptions);

        Dictionary<string, object> result =
            JsonSerializer.Deserialize<Dictionary<string, object>>(jsonPayload, JsonSerializerSettings.DeserializeOptions)!;

        result.Remove(nameof(Action).ToLower());

        return result;
    }
}