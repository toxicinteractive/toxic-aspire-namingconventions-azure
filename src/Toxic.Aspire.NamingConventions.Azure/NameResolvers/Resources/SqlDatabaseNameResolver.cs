using Azure.Provisioning.Sql;

namespace Toxic.Aspire.NamingConventions.Azure.NameResolvers.Resources;

public class SqlDatabaseNameResolver : DefaultResourceNameResolver<SqlDatabase>
{
    public SqlDatabaseNameResolver(
        IRegionNameResolver regionNameResolver,
        IResourcePrefixResolver resourcePrefixResolver,
        IEnvironmentNameResolver environmentNameResolver)
        : base(regionNameResolver, resourcePrefixResolver, environmentNameResolver)
    {

    }

    public override string? ResolveName(SqlDatabase resource, NameResolutionContext context)
    {
        // don't include region name in database name
        context.SupportsRegion = false;

        return base.ResolveName(resource, context);
    }
}
