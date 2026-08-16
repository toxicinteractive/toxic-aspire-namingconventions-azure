using Azure.Core;

namespace Toxic.Aspire.NamingConventions.Azure.NameResolvers;

/// <summary>
/// Gets a common abbreviation for an Azure location.
/// </summary>
public interface IRegionNameResolver
{
    /// <summary>
    /// Gets a region name abbreviation for a location.
    /// </summary>
    string? ResolveRegionName(AzureLocation location);
}
