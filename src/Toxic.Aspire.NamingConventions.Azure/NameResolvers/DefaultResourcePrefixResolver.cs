using Azure.Provisioning.AppConfiguration;
using Azure.Provisioning.AppContainers;
using Azure.Provisioning.AppService;
using Azure.Provisioning.ContainerRegistry;
using Azure.Provisioning.KeyVault;
using Azure.Provisioning.Network;
using Azure.Provisioning.OperationalInsights;
using Azure.Provisioning.Primitives;
using Azure.Provisioning.RedisEnterprise;
using Azure.Provisioning.Roles;
using Azure.Provisioning.ServiceBus;

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
            AppServicePlan => ResourcePrefixes.AppServicePlan,
            WebSite => ResourcePrefixes.AppService,
            AppConfigurationStore => ResourcePrefixes.AppConfigurationStore,
            RedisEnterpriseCluster => ResourcePrefixes.Redis,
            ServiceBusNamespace => ResourcePrefixes.ServiceBusNamespace,
            VirtualNetwork => ResourcePrefixes.VirtualNetwork,
            NetworkSecurityGroup => ResourcePrefixes.NetworkSecurityGroup,
            NetworkSecurityPerimeter => ResourcePrefixes.NetworkSecurityPerimeter,
            _ => null
        };
    }
}
