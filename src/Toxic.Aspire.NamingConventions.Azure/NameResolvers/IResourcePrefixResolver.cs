using Azure.Provisioning.Primitives;

namespace Toxic.Aspire.NamingConventions.Azure.NameResolvers;

/// <summary>
/// Generates a prefix for a given resource type.
/// </summary>
public interface IResourcePrefixResolver
{
    /// <summary>
    /// Gets a prefix for a resource.
    /// </summary>
    string? ResolveResourcePrefix<T>(T resource) where T : ProvisionableResource;
}
