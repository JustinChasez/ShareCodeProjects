// ReSharper disable once CheckNamespace

namespace System;

internal static class ServiceProviderExtensions
{
    public static TService? TryGet<TService>(this IServiceProvider provider) where TService : class
    {
        if (TryGet(provider, typeof(TService)) is TService tServiceInstance)
            return tServiceInstance;

        return default;
    }

    public static object TryGet(this IServiceProvider provider, Type type)
    {
        Exception innerException = null;

        if (type.IsInterface)
        {
            var serviceInstance = provider.GetService(type);

            return serviceInstance;
        }

        try
        {
            var instance = provider.GetService(type);

            if (instance is not null)
                return instance;

            return Activator.CreateInstance(type);
        }
        catch
        {
            foreach (var constructor in type.GetConstructors())
            {
                try
                {
                    //try to resolve constructor parameters
                    var parameters = constructor.GetParameters()
                                                .Select(parameter =>
                                                 {
                                                     var service = provider.TryGet(parameter.ParameterType);

                                                     return service ?? throw new Exception("Unknown dependency");
                                                 });

                    // all is ok, so create instance
                    return Activator.CreateInstance(type, parameters.ToArray());
                }
                catch (Exception ex)
                {
                    innerException = ex;
                }
            }

            throw new Exception("No constructor was found that had all the dependencies satisfied.", innerException);
        }
    }
}