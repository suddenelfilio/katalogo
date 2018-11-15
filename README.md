# Katalogo 

> Katalogo - Esperanto for Catalog  

Assembly scanning and catalog support for Microsoft.Extensions.DependencyInjection. This allows you to define a Catalog in your projects which allows you to create a set of service registrations.
This technique resembles the Registry in Structuremap and Module in Autofac.

**This project was inspired by the [Scrutor Library by Kristian Hellang](https://github.com/khellang/Scrutor) Assembly scanning and decoration extensions for Microsoft.Extensions.DependencyInjection.** 

## Installation

Install the [Katalogo NuGet Package](https://www.nuget.org/packages/Katalogo).

### Package Manager Console

```
Install-Package Katalogo
```

### .NET Core CLI

```
dotnet add package Katalogo
```

## Usage

The library adds two extension methods to `IServiceCollection`:

* `Scan` - This is the entry point to set up your assembly scanning to search for classes deriving from Catalog.
* `RegisterCatalog` - This method is used to register a catalog directly without assembly scanning.

See **Examples** below for usage examples.

## Examples

### Scanning

In your project you create a public class deriving from Catalog. 

```csharp
  public interface ITest
    {
        void Hello();
    }

    public class Test : ITest
    {
        public void Hello()
        {

        }
    }

    public class TestCatalog : Catalog
    {
        public TestCatalog(IServiceCollection services) : base(services)
        {
            services.AddTransient<ITest, Test>();
        }
    }
```

In your startup or whatever entry point you use to wire up your ServiceCollection you can do the following.

```csharp
    IServiceCollection services = new ServiceCollection();

    services.Scan(scan => scan.FromCallingAssembly().RegisterCatalogs());
```

This will register all classes deriving from Catalog into your ServiceCollection that are in the Calling Assembly.

### Direct registration
You can also register your Catalog directly the following way:

```csharp
    IServiceCollection services = new ServiceCollection();
    services.RegisterCatalog<TestCatalog>();
```
