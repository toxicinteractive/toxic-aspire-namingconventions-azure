using Azure.Provisioning.Primitives;

namespace Toxic.Aspire.NamingConventions.Azure.NameResolvers;

/// <summary>
/// The default resolver for Azure resource names according to the defined standard naming convention.
/// This is used by default for all resources but can be overridden by registering a type-specific singleton for a particular resource.
/// Produces something like "ca-blog-cms-prod-euw" (container app, blog project, cms workload, prod environment, sweden central region).
/// </summary>
public class DefaultResourceNameResolver<T> : IResourceNameResolver<T> where T : ProvisionableResource
{
    public Type ResourceType => typeof(T);
    protected virtual string DefaultPattern { get; } = "{Prefix}{Project}{Workload}{Env}";
    private readonly IRegionNameResolver _regionNameResolver;
    private readonly IResourcePrefixResolver _resourcePrefixResolver;
    private readonly IEnvironmentNameResolver _environmentNameResolver;

    public DefaultResourceNameResolver(
        IRegionNameResolver regionNameResolver,
        IResourcePrefixResolver resourcePrefixResolver,
        IEnvironmentNameResolver environmentNameResolver)
    {
        _regionNameResolver = regionNameResolver;
        _resourcePrefixResolver = resourcePrefixResolver;
        _environmentNameResolver = environmentNameResolver;
    }

    public string? ResolveName(ProvisionableResource resource, NameResolutionContext context) =>
        ResolveName((T)resource, context);

    public virtual string? ResolveName(T resource, NameResolutionContext context)
    {
        var prefix = _resourcePrefixResolver.ResolveResourcePrefix(resource);
        var envName = _environmentNameResolver.ResolveEnvironmentName(context.EnvironmentName);
        var regionName = context.SupportsRegion ?
            _regionNameResolver.ResolveRegionName(context.ResourceRegion ?? context.DefaultRegion) :
            null;

        if (string.IsNullOrWhiteSpace(prefix))
        {
            return null;
        }

        var pattern = context.Pattern ?? DefaultPattern;

        pattern = ReplaceSegment(pattern, "{Prefix}", prefix, context.Separator);
        pattern = ReplaceSegment(pattern, "{Project}", context.ProjectName, context.Separator);
        pattern = ReplaceSegment(pattern, "{Workload}", context.AzureWorkloadName, context.Separator);
        pattern = ReplaceSegment(pattern, "{Env}", envName, context.Separator);
        pattern = ReplaceSegment(pattern, "{Region}", regionName, context.Separator);
        pattern = pattern.TrimEnd(context.Separator).ToString();

        return pattern;
    }

    protected virtual string ReplaceSegment(string pattern, string segment, string? value, string separator)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return pattern.Replace(segment, string.Empty);
        }

        return pattern.Replace(segment, $"{value}{separator}");
    }
}
