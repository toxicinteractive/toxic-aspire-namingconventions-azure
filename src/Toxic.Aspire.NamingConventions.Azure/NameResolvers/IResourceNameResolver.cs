using Azure.Provisioning.Primitives;

namespace Toxic.Aspire.NamingConventions.Azure.NameResolvers;

/// <summary>
/// Resolves (produces) a valid Azure resource name for a given resource.
/// This is for reflection purposes, implement <see cref="IResourceNameResolver{T}"/>.
/// </summary>
public interface IResourceNameResolver
{
    Type ResourceType { get; }
    string? ResolveName(ProvisionableResource resource, NameResolutionContext context);
}

/// <summary>
/// Resolves (produces) a valid Azure resource name for a given resource.
/// </summary>
public interface IResourceNameResolver<T> : IResourceNameResolver where T : ProvisionableResource
{
    string? ResolveName(T resource, NameResolutionContext context);
}
