

// ReSharper disable once CheckNamespace

namespace Microsoft.AspNetCore.Http;

internal static class HttpContextAccessorExtensions
{
    /// <summary>
    ///		Stores a value to the current HttpContext to retrieve later
    /// </summary>
    /// <remarks>
    ///		The stored value will be retrieved with the key is typeof(<typeparamref name="TItem"/>)
    /// </remarks>
    /// <typeparam name="TItem">
    ///		The type of object to store to the HttpContext, which acts as the key to retrieve
    /// </typeparam>
    /// <param name="httpContextAccessor"></param>
    /// <param name="value">The value to store</param>
    ///  <exception cref="InvalidOperationException">
    /// 		Thrown if the HttpContext is not available
    ///  </exception>
    public static void StoreValue<TItem>(this IHttpContextAccessor httpContextAccessor, TItem value)
        => httpContextAccessor.HttpContext.StoreValue(value);

    /// <summary>
    ///		Retrieves the stored object from the current HttpContext, using the key of typeof(<typeparamref name="TItem"/>)
    /// </summary>
    /// <typeparam name="TItem">
    ///		The type of stored object, which acts as the key to retrieve the object
    /// </typeparam>
    /// <param name="httpContextAccessor"></param>
    /// <returns>
    ///		The <typeparamref name="TItem"/> object if found, otherwise, <c>null</c>
    /// </returns>
    /// <exception cref="InvalidOperationException">
    ///		Thrown if the HttpContext is not available
    /// </exception>
    public static TItem RetrieveValue<TItem>(this IHttpContextAccessor httpContextAccessor) 
        => httpContextAccessor.HttpContext.RetrieveValue<TItem>();


    ///  <summary>
    /// 		Stores a value to the current HttpContext using the given <see cref="storeKey"/>. Use that to retrieve later
    ///  </summary>
    ///  <typeparam name="TItem">
    /// 		The type of object to store to the HttpContext, which acts as the key to retrieve
    ///  </typeparam>
    ///  <param name="httpContextAccessor"></param>
    ///  <param name="storeKey">The key for storing the object</param>
    ///  <param name="value">The value to store</param>
    ///  <exception cref="InvalidOperationException">
    /// 		Thrown if the HttpContext is not available
    ///  </exception>
    public static void StoreValue<TItem>(this IHttpContextAccessor httpContextAccessor, string storeKey, TItem value) 
        => httpContextAccessor.HttpContext.StoreValue(storeKey, value);

    ///  <summary>
    /// 		Retrieves the stored object from the current HttpContext using the given <see cref="storeKey"/>
    ///  </summary>
    ///  <typeparam name="TItem">
    /// 		The type of stored object expected to retrieve
    ///  </typeparam>
    ///  <param name="httpContextAccessor"></param>
    ///  <param name="storeKey">The key for retrieving the object</param>
    ///  <returns>
    /// 		The <typeparamref name="TItem"/> object if found, otherwise, <c>null</c>
    ///  </returns>
    ///  <exception cref="InvalidOperationException">
    /// 		Thrown if the HttpContext is not available
    ///  </exception>
    public static TItem RetrieveValue<TItem>(this IHttpContextAccessor httpContextAccessor, string storeKey)
        => httpContextAccessor.HttpContext.RetrieveValue<TItem>(storeKey);
}