using System.Text.RegularExpressions;
using Azure.Provisioning.AppService;

namespace Toxic.Aspire.NamingConventions.Azure.NameResolvers.Resources;

public class AppServiceAppNameResolver : DefaultResourceNameResolver<WebSite>
{
    public AppServiceAppNameResolver(
        IRegionNameResolver regionNameResolver,
        IResourcePrefixResolver resourcePrefixResolver,
        IEnvironmentNameResolver environmentNameResolver)
        : base(regionNameResolver, resourcePrefixResolver, environmentNameResolver)
    {

    }

    public override string? GetResourceNameValue(WebSite resource)
    {
        // app service web sites requires reading the resource name from a bicep expression:
        // take('${toLower('name')}-${uniqueString(resourceGroup().id)}', 60)

        var nameProp = resource.GetType().GetProperty("Name");
        var bicepName = nameProp?.GetValue(resource)?.ToString();

        if (!string.IsNullOrWhiteSpace(bicepName))
        {
            var resourceName = Regex.Match(
                bicepName, 
                @"\${toLower\('(?<name>[^']+)'\)}", 
                RegexOptions.IgnoreCase);

            if (resourceName.Success)
            {
                return resourceName.Groups["name"].Value;
            } 
        }
        
        return base.GetResourceNameValue(resource);
    }
}
