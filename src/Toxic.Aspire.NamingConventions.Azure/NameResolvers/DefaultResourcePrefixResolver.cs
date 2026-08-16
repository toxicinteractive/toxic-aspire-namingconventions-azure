using Azure.Provisioning.AppContainers;
using Azure.Provisioning.ContainerRegistry;
using Azure.Provisioning.KeyVault;
using Azure.Provisioning.OperationalInsights;
using Azure.Provisioning.Primitives;
using Azure.Provisioning.Roles;

namespace Toxic.Aspire.NamingConventions.Azure.NameResolvers;

/// <summary>
/// The default resolver for resource prefixes used in resource naming.
/// </summary>
public class DefaultResourcePrefixResolver : IResourcePrefixResolver
{
    public string? ResolveResourcePrefix<T>(T resource) where T : ProvisionableResource 
    {
        return resource switch
        {
            ContainerApp => ResourcePrefixes.ContainerApp,
            KeyVaultService => ResourcePrefixes.KeyVault,
            ContainerAppManagedEnvironment => ResourcePrefixes.ContainerAppEnvironment,
            ContainerRegistryService => ResourcePrefixes.ContainerRegistry,
            global::Azure.Provisioning.Storage.StorageAccount => ResourcePrefixes.StorageAccount,
            global::Azure.Provisioning.Sql.SqlServer => ResourcePrefixes.SqlServer,
            global::Azure.Provisioning.Sql.SqlDatabase => ResourcePrefixes.SqlDatabase,
            OperationalInsightsWorkspace => ResourcePrefixes.LogAnalyticsWorkspace,
            UserAssignedIdentity => ResourcePrefixes.ManagedIdentity,
            _ => null
        };
    }
}
