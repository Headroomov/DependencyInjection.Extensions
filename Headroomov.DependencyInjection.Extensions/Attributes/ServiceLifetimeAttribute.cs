using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;

namespace Headroomov.DependencyInjection.Extensions.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface,
                AllowMultiple = false,
                Inherited = false)]
public sealed class ServiceLifetimeAttribute : Attribute
{
    public ServiceLifetime Lifetime { get; }

    public ServiceLifetimeAttribute(ServiceLifetime lifetime)
    {
        Lifetime = lifetime;
    }
}