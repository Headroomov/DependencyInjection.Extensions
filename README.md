# Headroomov.DependencyInjection.Extensions

This package provides strict dependency injection (DI) extensions that enforce consistent service lifetime registration between interfaces and their implementations using `ServiceLifetimeAttribute`. It ensures that mismatches in lifetimes between interfaces and their implementations are caught at compile-time, making your DI setup reliable and predictable.

## Features

- **Lifetime Consistency Enforcement**: Guarantees that interfaces and their implementations have matching lifetimes.
- **Automatic Service Registration**: Simplifies adding services to DI containers while adhering to strict lifetime contracts.
- **Error Prevention**: Throws clear exceptions if the lifetimes of interfaces and implementations don’t match or if an attribute is missing.
- **Flexible API**: Supports both generic and non-generic registration methods.

## Installation

To install via NuGet, use the following command:

```bash
Install-Package Headroomov.DependencyInjection.Extensions
```

Or with the .NET CLI:

```bash
dotnet add package Headroomov.DependencyInjection.Extensions
```

## Usage

### Registering Services with Lifetime Attributes

The core of this package is the `AddWithAttribute` method, which ensures that both interfaces and their implementations are registered in the DI container with consistent lifetimes as defined by `ServiceLifetimeAttribute`.

#### Example: Interface and Implementation Registration Using Generics

```csharp
using Microsoft.Extensions.DependencyInjection;
using Headroomov.DependencyInjection.Extensions;
using Headroomov.DependencyInjection.Extensions.Attributes;

[ServiceLifetime(ServiceLifetime.Singleton)]
public interface IMyService
{
    void Execute();
}

[ServiceLifetime(ServiceLifetime.Singleton)]
public class MyService : IMyService
{
    public void Execute()
    {
        // Implementation here
    }
}

public class DependencyInjection
{
    public void ConfigureServices(IServiceCollection services)
    {
        // Register both the interface and the implementation with matching lifetimes using generics
        services.AddWithAttribute<IMyService, MyService>();
    }
}
```

### Explanation:

- **`AddWithAttribute<TInterface, TImplementation>`** ensures that both **the interface** and **the implementation** are checked for `ServiceLifetimeAttribute`, and their lifetimes must match. If they don’t, an exception will be thrown.
- This method is **preferred** as it leverages C#'s type safety with generics.

#### Example: Direct Service Registration Using Types

```csharp
[ServiceLifetime(ServiceLifetime.Singleton)]
public class MySingletonService
{
    // Implementation here
}

public class DependencyInjection
{
    public void ConfigureServices(IServiceCollection services)
    {
        // Register service directly with its lifetime defined in the attribute
        services.AddWithAttribute(typeof(MySingletonService), typeof(MySingletonService));
    }
}
```

### Explanation of Non-Generic Method (Backup):

You can also use the non-generic method to register services. This approach is **not recommended** but is provided for flexibility:

```csharp
public class DependencyInjection
{
    public void ConfigureServices(IServiceCollection services)
    {
        // Backup method: Register interface and implementation using types
        services.AddWithAttribute(typeof(IMyService), typeof(MyService));
    }
}
```

This method uses `Type` instead of generics, which makes it a bit less type-safe, but still functional.

### Example: Scoped Service Registration

```csharp
[ServiceLifetime(ServiceLifetime.Scoped)]
public class MyScopedService
{
    // Implementation for Scoped service
}

public class DependencyInjection
{
    public void ConfigureServices(IServiceCollection services)
    {
        // Register Scoped service using generics
        services.AddWithAttribute<MyScopedService, MyScopedService>();
    }
}
```

### Key Points

- **Interfaces and implementations must both have the `ServiceLifetimeAttribute`** applied. This ensures that the `AddWithAttribute` method checks for lifetime consistency.
- The method will throw an exception if the **lifetime contracts don’t match** between an interface and its implementation.
- **Preferred method**: Use the generic version `AddWithAttribute<TInterface, TImplementation>` for better type safety and simplicity.
- **Backup method**: You can use the `AddWithAttribute` with `Type` parameters (`AddWithAttribute(typeof(IMyService), typeof(MyService))`), but it is not as type-safe as the generic version.

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE.txt) file for details.

## Contact

For any questions or issues, feel free to open an issue or contact us directly at [Headroomov](https://github.com/Headroomov).
