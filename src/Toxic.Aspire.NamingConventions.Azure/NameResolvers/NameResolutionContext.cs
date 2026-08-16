using Azure.Core;
using Azure.Provisioning.Primitives;

namespace Toxic.Aspire.NamingConventions.Azure.NameResolvers;

/// <summary>
/// Context for resource name resolvers with data about the resource and relevant parameters concerning naming.
/// </summary>
public class NameResolutionContext
{
    /// <summary>
    /// User defined name pattern in the form of "{Prefix}{Region}" etc without separator characters.
    /// </summary>
    public string? Pattern { get; init; }

    /// <summary>
    /// Gets the short project name/identifier, e.g. "blog".
    /// </summary>
    public required string ProjectName { get; init; }

    /// <summary>
    /// Gets the environment name, e.g. "Development".
    /// </summary>
    public required string EnvironmentName { get; init; }

    /// <summary>
    /// Gets the default Azure region as specified in the project.
    /// Can be overridden on a resource basis. See <see cref="ResourceRegion"/>.
    /// </summary>
    public required AzureLocation DefaultRegion { get; init; }

    /// <summary>
    /// Gets the Azure region specific to this resource as specified in the App Host.
    /// </summary>
    public AzureLocation? ResourceRegion { get; init; }

    /// <summary>
    /// Gets the optional resource workload name when deployed to Azure. E.g. "cms" for a CMS web app.
    /// </summary>
    public string? AzureWorkloadName { get; init; }

    /// <summary>
    /// Gets or sets whether the resource is region-based and not global (if the region should be part of the name).
    /// </summary>
    public bool SupportsRegion { get; set; }

    /// <summary>
    /// Gets or sets the segment separator, can be "-", "_" or "" depending on resource name requirements.
    /// </summary>
    public required string Separator { get; set; }

    /// <summary>
    /// Gets the Azure resource name requirements such as valid characters and lengths.
    /// </summary>
    public required ResourceNameRequirements NameRequirements { get; init; }
}
