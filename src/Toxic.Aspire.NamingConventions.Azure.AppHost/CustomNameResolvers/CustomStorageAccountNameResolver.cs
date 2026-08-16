using Toxic.Aspire.NamingConventions.Azure.NameResolvers;
using Azure.Provisioning.Storage;

namespace Toxic.Aspire.NamingConventions.Azure.AppHost.CustomNameResolvers;

public class CustomStorageAccountNameResolver : DefaultResourceNameResolver<StorageAccount>
{
    public CustomStorageAccountNameResolver(
        IRegionNameResolver regionNameResolver,
        IResourcePrefixResolver resourcePrefixResolver,
        IEnvironmentNameResolver environmentNameResolver)
        : base(regionNameResolver, resourcePrefixResolver, environmentNameResolver)
    {

    }

    public override string ResolveName(StorageAccount resource, NameResolutionContext context)
    {
        // modify the context parameters or return an entirely custom name here
        return "mycustomname";

        //context.
        //return base.ResolveName(resource, context);
    }
}
