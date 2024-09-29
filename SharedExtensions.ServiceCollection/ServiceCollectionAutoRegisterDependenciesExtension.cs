using System.Reflection;

namespace Microsoft.Extensions.DependencyInjection.Extensions;

internal static class ServiceCollectionAutoRegisterDependenciesExtension
{
    /// <summary>
    ///     Detects and registers the implementation services that implement <typeparamref name="TDependency"/> service type 
    /// </summary>
    /// <typeparam name="TDependency">The service type to register</typeparam>
    /// <param name="serviceCollection">The <see cref="IServiceCollection"/></param>
    /// <param name="assembliesBundle">
    ///     All the assemblies loaded to the context
    /// </param>
    /// <param name="lifetime">
    ///     Specifies the <see cref="ServiceLifetime"/> for resolving the service
    /// </param>
    /// <param name="registerProvidedServiceType">
    ///     Indicates if the <typeparamref name="TDependency" /> should be registered as well.
    ///     It is only useful when the <typeparamref name="TDependency"/> is not directly implemented by the service
    /// </param>
    public static void RegisterServiceImplementations<TDependency>(this IServiceCollection serviceCollection,
                                                                   IEnumerable<Assembly>   assembliesBundle,
                                                                   ServiceLifetime lifetime =
                                                                       ServiceLifetime.Scoped,
                                                                   bool registerProvidedServiceType =
                                                                       false)
    {
        var serviceTypes = assembliesBundle.GetDerivedTypes<TDependency>();

        RegisterServiceImplementations<TDependency>(serviceCollection,
                                                    serviceTypes,
                                                    lifetime,
                                                    registerProvidedServiceType);
    }

    public static void RegisterImplementorsFromAssembly(this IServiceCollection serviceCollection,
                                                        Assembly                assembly)
    {
        var exportedTypes = assembly.GetExportedTypes()
                                    .Where(t => !t.IsAbstract && !t.IsInterface);

        var lifetime = ServiceLifetime.Scoped;

        foreach (var dependencyType in exportedTypes)
        {
            var interfaces = dependencyType.GetInterfaces().ToArray();

            // Get only direct parent interfaces
            interfaces = interfaces.Except(interfaces.SelectMany(t => t.GetInterfaces()))
                                   .ToArray();

            if (interfaces.Any())
            {
                foreach (var @interface in interfaces)
                {
                    if (@interface.IsGenericType &&
                        dependencyType.IsGenericType)
                    {
                        var interfaceGenericTypeDef  = @interface.GetGenericTypeDefinition();
                        var dependencyGenericTypeDef = dependencyType.GetGenericTypeDefinition();

                        serviceCollection.Add(ServiceDescriptor.Describe(interfaceGenericTypeDef,
                                                                         dependencyGenericTypeDef,
                                                                         lifetime));
                        // register the type itself
                        serviceCollection.TryAdd(ServiceDescriptor.Describe(dependencyGenericTypeDef,
                                                                            dependencyGenericTypeDef,
                                                                            lifetime));
                    }
                    else if (!dependencyType.IsGenericType)
                    {
                        var existingRegistration = serviceCollection.FirstOrDefault(d => d.ServiceType == @interface &&
                                                                                         d.ImplementationType ==
                                                                                         dependencyType);

                        if (existingRegistration != null)
                            continue;

                        serviceCollection.Add(ServiceDescriptor.Describe(@interface,
                                                                         dependencyType,
                                                                         lifetime));
                    }
                }
            }

            // register the type itself
            serviceCollection.TryAdd(ServiceDescriptor.Describe(dependencyType,
                                                                dependencyType,
                                                                lifetime));
        }
    }

    public static void ReplaceServiceLifetimeScope<TDependency>(this IServiceCollection serviceCollection,
                                                                ServiceLifetime         lifetime)
    {
        var serviceRegistrationsToReplace = serviceCollection.Where(d => d.ServiceType == typeof(TDependency) &&
                                                                         d.Lifetime != lifetime)
                                                             .ToArray();

        foreach (var serviceRegistration in serviceRegistrationsToReplace)
        {
            ServiceDescriptor serviceDescriptor = null;

            if (serviceRegistration.ImplementationType != null)
            {
                serviceDescriptor = ServiceDescriptor.Describe(serviceRegistration.ServiceType,
                                                               serviceRegistration.ImplementationType,
                                                               lifetime);
            }

            if (serviceRegistration.ImplementationFactory != null)
            {
                serviceDescriptor = ServiceDescriptor.Describe(serviceRegistration.ServiceType,
                                                               serviceRegistration.ImplementationFactory,
                                                               lifetime);
            }

            if (serviceDescriptor != null)
                serviceCollection.Add(serviceDescriptor);
        }
    }

    /// <summary>
    /// Detects and registers the implementation services that implement <typeparamref name="TDependency"/> service type 
    /// </summary>
    /// <typeparam name="TDependency">The service type to register</typeparam>
    /// <param name="serviceCollection">The <see cref="IServiceCollection"/></param>
    /// <param name="dependencyTypes">
    ///     All the detected service types loaded to the context
    /// </param>
    /// <param name="lifetime">
    ///     Specifies the <see cref="ServiceLifetime"/> for resolving the service
    /// </param>
    /// <param name="registerProvidedServiceType">
    ///     Indicates if the <typeparamref name="TDependency" /> should be registered as well.
    ///     It is only useful when the <typeparamref name="TDependency"/> is not directly implemented by the service
    /// </param>
    public static void RegisterServiceImplementations<TDependency>(this IServiceCollection serviceCollection,
                                                                   IEnumerable<Type>       dependencyTypes,
                                                                   ServiceLifetime lifetime =
                                                                       ServiceLifetime.Scoped,
                                                                   bool registerProvidedServiceType =
                                                                       false)
    {
        var typesToRegister = dependencyTypes.Where(t => t.IsSubclassOf(typeof(TDependency)) ||
                                                         typeof(TDependency).IsAssignableFrom(t))
                                             .Distinct()
                                             .ToArray();

        if (!typesToRegister.Any())
            return;

        var shouldRegisterAsInterface = typeof(TDependency).IsInterface;

        foreach (var dependencyType in typesToRegister)
        {
            if (shouldRegisterAsInterface)
            {
                var interfaces = dependencyType.GetInterfaces().ToArray();

                // Get only direct parent interfaces
                interfaces = dependencyType.IsGenericType
                                 ? interfaces.Except(interfaces.SelectMany(t => t.GetInterfaces().Where(t2 => !t2.IsGenericType)))
                                             .ToArray()
                                 : interfaces.Except(interfaces.SelectMany(t => t.GetInterfaces()))
                                             .ToArray();

                if (registerProvidedServiceType && !interfaces.Contains(typeof(TDependency)))
                {
                    interfaces = interfaces.Concat(new[] {typeof(TDependency)})
                                           .ToArray();
                }

                foreach (var @interface in interfaces)
                {
                    if (@interface.IsGenericType && dependencyType.IsGenericType)
                    {
                        var interfaceGenericTypeDef  = @interface.GetGenericTypeDefinition();
                        var dependencyGenericTypeDef = dependencyType.GetGenericTypeDefinition();

                        serviceCollection.Add(ServiceDescriptor.Describe(interfaceGenericTypeDef,
                                                                         dependencyGenericTypeDef,
                                                                         lifetime));
                        // register the type itself
                        serviceCollection.TryAdd(ServiceDescriptor.Describe(dependencyGenericTypeDef,
                                                                            dependencyGenericTypeDef,
                                                                            lifetime));
                    }
                    else if (!dependencyType.IsGenericType)
                    {
                        // find the existing registration
                        var existingRegistration = serviceCollection.FirstOrDefault(d => d.ServiceType == @interface &&
                                                                                         d.ImplementationType ==
                                                                                         dependencyType);

                        if (existingRegistration != null)
                        {
                            // remove the existing registration if lifetime is different
                            if (existingRegistration.Lifetime != lifetime)
                            {
                                serviceCollection.Remove(existingRegistration);
                            }
                            else // already available, ignore for this service
                            {
                                continue;
                            }
                        }

                        // add correct service descriptor
                        serviceCollection.Add(ServiceDescriptor.Describe(@interface,
                                                                         dependencyType,
                                                                         lifetime));
                    }
                }
            }
            else
            {
                // register the type as the parent class
                if (dependencyType.BaseType != null)
                {
                    serviceCollection.Add(ServiceDescriptor.Describe(dependencyType.BaseType,
                                                                     dependencyType,
                                                                     lifetime));
                }

                if (registerProvidedServiceType)
                {
                    serviceCollection.Add(ServiceDescriptor.Describe(typeof(TDependency),
                                                                     dependencyType,
                                                                     lifetime));
                }
            }

            // register the type itself
            serviceCollection.TryAdd(ServiceDescriptor.Describe(dependencyType,
                                                                dependencyType,
                                                                lifetime));
        }
    }
}