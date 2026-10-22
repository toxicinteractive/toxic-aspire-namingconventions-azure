using Azure.Provisioning.AppService;

namespace Toxic.Aspire.NamingConventions.Azure.NameResolvers.Resources;

public class AppServicePlanNameResolver : DefaultResourceNameResolver<AppServicePlan>
{
    public AppServicePlanNameResolver(
        IRegionNameResolver regionNameResolver,
        IResourcePrefixResolver resourcePrefixResolver,
        IEnvironmentNameResolver environmentNameResolver)
        : base(regionNameResolver, resourcePrefixResolver, environmentNameResolver)
    {

    }

    public override string? GetResourceNameValue(AppServicePlan resource)
    {
        // app service plan names get an "_asplan" suffix
        return resource.BicepIdentifier.Replace("_asplan", string.Empty);
    }
}
