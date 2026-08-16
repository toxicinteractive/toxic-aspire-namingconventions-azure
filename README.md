# Toxic.Aspire.NamingConventions.Azure
Contains a framework for automatically generating predictable Azure resource names with Aspire according to conventions instead of random identifiers.

## Why
When you define a resource in your Aspire app host you give it a name, but when deploying that resource name will be added a random identifier.
E.g. a resource `sql-server` will get the name `sql-serverlaw-nj6mfe7xbw`. This package will generate predictable [CAF-like](https://learn.microsoft.com/en-us/azure/cloud-adoption-framework/ready/azure-best-practices/resource-naming) resource names when deploying with Aspire in a customizable way so that `sql-server` becomes `sql-mysite-shop-dev-swc` (customizable). This helps keep a uniform, organized naming scheme across your organisation or project resources.

## How to use
1. Install the [Toxic.Aspire.NamingConventions.Azure nuget package](https://www.nuget.org/packages/Toxic.Aspire.NamingConventions.Azure) to your app host project. The package version aligns with the corresponding Aspire version.
2. Make sure your appsettings includes the required Aspire settings:
    ```json
    "Azure": {
      "Location": "swedencentral",
      "ResourceGroup": "rg-project-dev-swc"
    }
    ```
3. Call `WithAzureNamingConvention` on your AppHost builder: 
    ```csharp
    var builder = DistributedApplication
        .CreateBuilder(args)
        .WithAzureNamingConvention("projectname");
    ```
    Supported resources will now have their deployed names automatically generated when running `aspire deploy` or `aspire publish`. You can use `WithAzureWorkloadName` to give a particular resource a unique identifier to tell similar resources apart.
4. Run `aspire publish` to verify the generated names.

See below for more customization and visit the [GitHub page](https://github.com/toxicinteractive/toxic-aspire-namingconventions-azure) for a sample AppHost project.

## Features
### Default naming convention
By default the `DefaultResourceNameResolver` will generate a name for a resource according to the following pattern: `{Prefix}{Project}{Workload}{Env}{Region}`.

* Prefix: Generated from a table of resource prefixes from https://www.azureperiodictable.com
* Project: The project name given to `WithAzureNamingConvention`
* Workload: A unique workload name given to a specific resource with `WithAzureWorkloadName` (optional, and won't be used for automatically generated resources)
* Env: Environment name (dev, stage, prod, etc)
* Region: The resource region if the resource supports it

### Custom pattern
To influence the name resolver outcome for all resources you can specify a custom pattern when calling `WithAzureNamingConvention`: 
```csharp
builder.WithAzureNamingConvention("projectname", "{Prefix}{Workload}{Env}")
```
 The default pattern is `{Prefix}{Project}{Workload}{Env}{Region}`.

### Custom resource name resolver
To have direct control over how the name is generated for a particular resource type you can add a custom name resolver override: 
```csharp
builder.Services.AddSingleton<IResourceNameResolver<SqlDatabase>, MyCustomSqlDatabaseNameResolver>();
``` 
This is useful when controlling resources that Aspire creates automatically that are not declared in your AppHost.

### Set explicit resource names without convention
You can also just hard-code a resource name directly on the resource like this: 
```csharp
resourceBuilder.WithAzureWorkloadName("completely-custom-name", true)
```
This will short-circuit the name generation and just use that name for the resource when deploying.

### Custom resource prefix resolver
The default resource prefix resolver takes a prefix from a list of resource prefixes generated from https://www.azureperiodictable.com. A limited number of resource types is currently mapped to a prefix. To add more register a custom implementation like this: 
```csharp
builder.Services.AddSingleton<IResourcePrefixResolver, MyCustomResourcePrefixResolver>();
``` 
You can extend the `DefaultResourcePrefixResolver` and include more prefixes. The list of known prefixes can be found in `ResourcePrefixes`.

### Custom environment name resolver
The environment name resolver looks at the current environment and returns a short version of it. The default resolver only recognizes Production (prod), Staging (stage) and Development (dev). To add your own register a new `IEnvironmentNameResolver`: 
```csharp
builder.Services.AddSingleton<IEnvironmentNameResolver, MyCustomEnvironmentNameResolver>();
```
You can extend the `DefaultEnvironmentNameResolver` and include your own environments.

### Custom region name resolver
The region name resolver generates an abbreviated name for an Azure location. Not all regions are currently supported. To add more or customize it register a custom implementation like this: 
```csharp
builder.Services.AddSingleton<IRegionNameResolver, MyCustomRegionNameResolver>();
```
You can extend the `DefaultRegionNameResolver` and include more regions. 

## Considerations/limitations
### Supported resource types
Each resource type that should have their name generated must be registered to a name resolver explicitly like this: 
```csharp
builder.Services.AddSingleton<IResourceNameResolver<SqlDatabase>, DefaultResourceNameResolver<SqlDatabase>>();
```
There is a limited number of resource types that are registered by the framework by default. There is also a limited number of resource types that is handled by the default resource prefix resolver.

See `DistributedApplicationBuilderExtensions` for all resource types that are enabled by default and `DefaultResourcePrefixResolver` for supported resource prefixes.

### Name collisions
If you declare 2 or more resources without using `WithAzureWorkloadName` to identify them their generated names will be identical. E.g: 

```csharp
builder
    .AddAzureSqlServer("sql1")  // sql-project-prod-euw
    .AddAzureSqlServer("sql2"); // sql-project-prod-euw
```

In these situations you need to uniquely identify the resources like this:

```csharp
builder
    .AddAzureSqlServer("sql1")
    .WithAzureWorkloadName("store")  // sql-project-store-prod-euw
    .AddAzureSqlServer("sql2")
    .WithAzureWorkloadName("users"); // sql-project-users-prod-euw
```

This is also true for automatically generated resources. Aspire will generate required resources automatically if there's a need to, for example a container registry and a managed identity when you create a container app environment. To avoid name collisions for automatically generated resources you could:

* Manually declare the resources in your AppHost and hand them to Aspire when declaring the master resource, now you can identify your resources
* Exclude the resource types completely from the name resolution with a custom name resolver that does nothing
* Handle the resolution in some other way in a custom name resolver

