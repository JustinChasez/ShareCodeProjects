using System.Diagnostics;
using System.IO.Compression;
using System.Text;
using System.Text.Json;

namespace DotNetBrightener.Utils.MessageCompression;

internal static class MessageCompressionService
{
    /// <summary>
    ///     Compresses the given byte-array
    /// </summary>
    /// <param name="jsonBytes"></param>
    /// <returns></returns>
    [DebuggerStepThrough]
    public static async Task<byte[]> Compress(this byte[] jsonBytes)
    {
        using var       msCompress = new MemoryStream();
        await using var gsCompress = new GZipStream(msCompress, CompressionMode.Compress);
        await gsCompress.WriteAsync(jsonBytes, 0, jsonBytes.Length);

        gsCompress.Close();
        var result = msCompress.ToArray();

        return result;
    }

    /// <summary>
    ///     Serializes the given object into JSON format and convert it to bytes array
    /// </summary>
    /// <param name="message"></param>
    /// <returns></returns>
    [DebuggerStepThrough]
    public static async Task<byte[]> ToJsonBytes(this object message)
    {
        var serializedJsonString      = JsonSerializer.Serialize(message, JsonSerializerSettings.SerializeOptions);
        return Encoding.UTF8.GetBytes(serializedJsonString);
    }

    /// <summary>
    ///     Decompresses the given byte-array and converts it to RequestMessage
    /// </summary>
    /// <param name="msRequest">The stream contains the message</param>
    /// <param name="bufferSize">The buffer size</param>
    /// <returns></returns>
    [DebuggerStepThrough]
    public static async Task<TResult> Decompress<TResult>(this MemoryStream msRequest, int bufferSize)
    {
        var decompressedBytes = await msRequest.DecompressBytes(bufferSize);

        await using var msDecompress = new MemoryStream(decompressedBytes);
        using var srDecompress = new StreamReader(msDecompress, Encoding.UTF8);
        var       jsonRequest  = await srDecompress.ReadToEndAsync();
        var result = JsonSerializer.Deserialize<TResult>(jsonRequest, JsonSerializerSettings.DeserializeOptions)!;

        return result;
    }

    /// <summary>
    ///     Decompresses the given byte-array
    /// </summary>
    /// <param name="msRequest">The stream contains the message</param>
    /// <param name="bufferSize">The buffer size</param>
    /// <returns>
    ///     The byte-array after decompression
    /// </returns>
    [DebuggerStepThrough]
    public static async Task<byte[]> DecompressBytes(this MemoryStream msRequest, int bufferSize)
    {
        msRequest.Seek(0, SeekOrigin.Begin);
        var             bufferDecompress    = new byte[bufferSize];
        await using var gsDecompress        = new GZipStream(msRequest, CompressionMode.Decompress);
        using var       msDecompress        = new MemoryStream();
        var             bytesReadDecompress = 0;

        while ((bytesReadDecompress = await gsDecompress.ReadAsync(bufferDecompress, 0, bufferDecompress.Length)) > 0)
        {
            await msDecompress.WriteAsync(bufferDecompress, 0, bytesReadDecompress);
        }

        msDecompress.Seek(0, SeekOrigin.Begin);

        return msDecompress.ToArray();
    }
}