namespace Toxic.Aspire.NamingConventions.Azure;

/// <summary>
/// Mediator object for connecting resource builder "workload names" with the infrastructure resolver.
/// </summary>
public class ResourceWorkloadNameAssociation
{
    public required Type ResourceType { get; init; }
    public required string ResourceName { get; init; }
    public required string AzureWorkloadName { get; init; }

    /// <summary>
    /// Whether to override the resoure name completely and ignore registered name resolvers.
    /// </summary>
    public required bool IgnoreNameResolvers { get; init; }
}
