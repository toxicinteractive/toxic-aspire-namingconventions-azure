using Azure.Provisioning.AppContainers;

namespace Toxic.Aspire.NamingConventions.Azure.NameResolvers.Resources;

public class ContainerAppManagedEnvironmentNameResolver : DefaultResourceNameResolver<ContainerAppManagedEnvironment>
{
    public ContainerAppManagedEnvironmentNameResolver(
        IRegionNameResolver regionNameResolver,
        IResourcePrefixResolver resourcePrefixResolver,
        IEnvironmentNameResolver environmentNameResolver)
        : base(regionNameResolver, resourcePrefixResolver, environmentNameResolver)
    {

    }

    public override string? ResolveName(ContainerAppManagedEnvironment resource, NameResolutionContext context)
    {
        // allow hyphen even though aspire name requirements dictate otherwise
        context.Separator = "-";

        return base.ResolveName(resource, context);
    }
}
