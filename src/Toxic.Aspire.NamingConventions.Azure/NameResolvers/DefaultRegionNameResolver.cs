using Azure.Core;

namespace Toxic.Aspire.NamingConventions.Azure.NameResolvers;

/// <summary>
/// The default resolver for region name used in resource naming.
/// </summary>
public class DefaultRegionNameResolver : IRegionNameResolver
{
    public string? ResolveRegionName(AzureLocation location)
    {
        if (RegionNames.Regions.TryGetValue(location.Name, out var regionName))
        {
            return regionName;
        }

        return null;
    }
}
