using System.Reflection;
using Headroomov.DependencyInjection.Extensions.Attributes;
using Microsoft.Extensions.DependencyInjection;

namespace Headroomov.DependencyInjection.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddWithAttribute(this IServiceCollection services, Type interfaceType, Type implementationType)
    {
        if (services == null)
            throw new ArgumentNullException(nameof(services));

        if (interfaceType == null)
            throw new ArgumentNullException(nameof(interfaceType));

        if (implementationType == null)
            throw new ArgumentNullException(nameof(implementationType));

        ServiceLifetimeAttribute? interfaceAttribute = interfaceType.GetCustomAttribute<ServiceLifetimeAttribute>(inherit: false);

        if (interfaceAttribute == null)
            throw new InvalidOperationException($"{nameof(ServiceLifetimeAttribute)} is missing on service interface {interfaceType.Name}");

        ServiceLifetimeAttribute? implementationAttribute = implementationType.GetCustomAttribute<ServiceLifetimeAttribute>(inherit: false);

        if (implementationAttribute == null)
            throw new InvalidOperationException($"{nameof(ServiceLifetimeAttribute)} is missing on service implementation {implementationType.Name}");

        if (implementationAttribute.Lifetime != interfaceAttribute.Lifetime)
            throw new InvalidOperationException($"Lifetime mismatch for service: {interfaceType.Name} → {implementationType.Name}. " +
                                                $"Interface contract requires {interfaceAttribute.Lifetime}, " +
                                                $"but implementation declares {implementationAttribute.Lifetime}. " +
                                                $"The implementation must respect the lifetime defined on the interface. " +
                                                $"Fix: change {implementationType.Name}'s [{nameof(ServiceLifetimeAttribute)}] to {interfaceAttribute.Lifetime} " +
                                                $"or align the interface attribute.");

        switch (implementationAttribute.Lifetime)
        {
            case ServiceLifetime.Singleton:
                services.AddSingleton(interfaceType, implementationType);
                break;

            case ServiceLifetime.Scoped:
                services.AddScoped(interfaceType, implementationType);
                break;

            case ServiceLifetime.Transient:
                services.AddTransient(interfaceType, implementationType);
                break;

            default:
                throw new NotImplementedException();
        }
    }

    public static IServiceCollection AddWithAttribute<TInterface, TImplementation>(this IServiceCollection services)
        where TImplementation : class, TInterface
    {
        if (services == null)
            throw new ArgumentNullException(nameof(services));

        Type interfaceType = typeof(TInterface);
        Type implementationType = typeof(TImplementation);

        services.AddWithAttribute(interfaceType, implementationType);
        return services;
    }

    public static IServiceCollection AddWithAttribute<TService>(this IServiceCollection services) where TService : class
    {
        Type serviceType = typeof(TService);

        ServiceLifetimeAttribute? attribute = serviceType.GetCustomAttribute<ServiceLifetimeAttribute>();

        if (attribute == null)
            throw new InvalidOperationException($"{nameof(ServiceLifetimeAttribute)} is missing on service {serviceType.Name}");

        switch (attribute.Lifetime)
        {
            case ServiceLifetime.Singleton:
                services.AddSingleton(serviceType);
                break;

            case ServiceLifetime.Scoped:
                services.AddScoped(serviceType);
                break;

            case ServiceLifetime.Transient:
                services.AddTransient(serviceType);
                break;

            default:
                throw new NotImplementedException();
        }

        return services;
    }
}
