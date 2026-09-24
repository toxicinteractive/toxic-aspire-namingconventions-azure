using System.Reflection;
using Azure.Core;
using Azure.Provisioning;
using Azure.Provisioning.Primitives;
using Microsoft.Extensions.DependencyInjection;
using Toxic.Aspire.NamingConventions.Azure.NameResolvers;

namespace Toxic.Aspire.NamingConventions.Azure;

/// <summary>
/// Main mechanism for resolving Aspire resource names into Azure resource names.
/// </summary>
internal class NamingInfrastructureResolver : InfrastructureResolver
{
    /// <summary>
    /// Short project name identifier, e.g. "blog".
    /// </summary>
    private readonly string _projectName;

    /// <summary>
    /// Default Azure region used for region abbreviation.
    /// </summary>
    private readonly AzureLocation _defaultRegion;

    /// <summary>
    /// "Development", "Staging", "Production" etc.
    /// </summary>
    private readonly string _environmentName;

    /// <summary>
    /// User defined name pattern in the form of "{Prefix}{Region}" etc without separator characters.
    /// </summary>
    private readonly string? _pattern;

    private readonly IServiceCollection _serviceCollection;

    private IServiceProvider ServiceProvider
    {
        get
        {
            field ??= _serviceCollection.BuildServiceProvider();
            return field;
        }
    }

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="projectName">Short project name identifier, e.g. "blog".</param>
    /// <param name="environmentName">The currently resolved dotnet environment name. See <see cref="Microsoft.Extensions.Hosting.HostApplicationBuilder.Environment"/> </param>
    /// <param name="defaultRegion">Default Azure region. Used for region abbreviation. See <see cref="RegionNames"/>.</param>
    /// <param name="pattern">Modify the pattern for generated names. Defaults to {Prefix}{Project}{Workload}{Env}.</param>
    public NamingInfrastructureResolver(
        string projectName,
        string environmentName,
        string? pattern,
        AzureLocation defaultRegion,
        IServiceCollection serviceCollection)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(projectName, nameof(projectName));
        ArgumentException.ThrowIfNullOrWhiteSpace(environmentName, nameof(environmentName));

        _projectName = projectName;
        _environmentName = environmentName;
        _defaultRegion = defaultRegion;
        _pattern = pattern;
        _serviceCollection = serviceCollection;
    }

    /// <summary>
    /// Main method to resolve a resource's name.
    /// This is called automatically for all resoures by the Aspire AppHost.
    /// </summary>
    public override void ResolveProperties(ProvisionableConstruct construct, ProvisioningBuildOptions options)
    {
        // require a provisionable Azure resource and that it has a Name property (exclude meta things)
        if (construct is not ProvisionableResource resource || GetResourceNameProperty(resource) == null)
        {
            base.ResolveProperties(construct, options);
            return;
        }

        var nameResolver = GetResourceNameResolver(resource);
        var workloadNameAssociation = GetResourceWorkloadNameAssociation(resource);
        var nameRequirements = resource.GetResourceNameRequirements();

        if (workloadNameAssociation != null &&
           (workloadNameAssociation.IgnoreNameResolvers ||
            nameResolver == null))
        {
            // we have a defined workload name and it should override the entire resource name
            SetResourceName(resource, workloadNameAssociation.AzureWorkloadName);
            return;
        }

        if (nameResolver != null)
        {
            var name = nameResolver.ResolveName(resource, new NameResolutionContext
            {
                Pattern = _pattern,
                ProjectName = _projectName,
                AzureWorkloadName = workloadNameAssociation?.AzureWorkloadName,
                EnvironmentName = _environmentName,
                DefaultRegion = _defaultRegion,
                ResourceRegion = GetResourceLocation(resource),
                SupportsRegion = GetResourceLocationProperty(resource) != null,
                Separator = GetValidNameSeparator(nameRequirements),
                NameRequirements = nameRequirements
            });

            // set resource name with the resolver we found for this resource type
            if (!string.IsNullOrWhiteSpace(name))
            {
                SetResourceName(resource, name);
            }
            else
            {
                Console.WriteLine($"No resoure prefix could be determined for {resource.GetType().Name}");
            }

            return;
        }

        Console.WriteLine($"No name resolver found for {resource.GetType().Name}");
    }

    /// <summary>
    /// Gets the name segment separator based on Azure resource name requirements.
    /// </summary>
    private string GetValidNameSeparator(ResourceNameRequirements requirements)
    {
        // we can have hyphens (preferred)
        if (requirements.ValidCharacters.HasFlag(ResourceNameCharacters.Hyphen))
        {
            return "-";
        }

        // have to use underscore
        if (requirements.ValidCharacters.HasFlag(ResourceNameCharacters.Underscore))
        {
            return "_";
        }

        // only alphanumeric characters so skip separator
        return string.Empty;
    }

    private ResourceWorkloadNameAssociation? GetResourceWorkloadNameAssociation(ProvisionableResource resource)
    {
        if (!string.IsNullOrWhiteSpace(resource.BicepIdentifier))
        {
            // find a registered "Azure workload name association" for this resource
            return ServiceProvider.GetKeyedService<ResourceWorkloadNameAssociation>(resource.BicepIdentifier);
        }

        return null;
    }

    private IResourceNameResolver? GetResourceNameResolver(ProvisionableResource resource)
    {
        var resolverType = typeof(IResourceNameResolver<>).MakeGenericType(resource.GetType());
        var resolver = ServiceProvider.GetService(resolverType) as IResourceNameResolver;

        if (resolver == null)
        {
            resolverType = typeof(IResourceNameResolver<>).MakeGenericType(typeof(ProvisionableResource));
            resolver = ServiceProvider.GetService(resolverType) as IResourceNameResolver;
        }

        return resolver;
    }

    private void SetResourceName(ProvisionableResource resource, string name)
    {
        GetResourceNameProperty(resource)?
            .SetValue(resource, new BicepValue<string>(name));
    }

    private PropertyInfo? GetResourceNameProperty(ProvisionableResource resource)
    {
        var prop = resource.GetType().GetProperty("Name");

        if (prop == null || prop.PropertyType != typeof(BicepValue<string>))
        {
            return null;
        }

        return prop;
    }

    private PropertyInfo? GetResourceLocationProperty(ProvisionableResource resource)
    {
        var prop = resource.GetType().GetProperty("Location");

        if (prop == null || prop.PropertyType != typeof(BicepValue<AzureLocation>))
        {
            return null;
        }

        return prop;
    }

    private AzureLocation? GetResourceLocation(ProvisionableResource resource)
    {
        var regionName = GetResourceLocationProperty(resource)?
            .GetValue(resource)?.ToString();

        if (string.IsNullOrWhiteSpace(regionName))
        {
            return null;
        }

        var location = new AzureLocation(regionName.Replace("'", string.Empty));

        return !string.IsNullOrWhiteSpace(location.DisplayName) ?
            location :
            (AzureLocation?)null;
    }
}
