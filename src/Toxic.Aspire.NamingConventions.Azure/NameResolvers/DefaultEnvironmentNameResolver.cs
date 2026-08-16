using Microsoft.Extensions.Hosting;

namespace Toxic.Aspire.NamingConventions.Azure.NameResolvers;

/// <summary>
/// The default resolver for environment name (suffix) used in resource naming.
/// Translates standard Asp.Net environment names into short suffixes.
/// </summary>
public class DefaultEnvironmentNameResolver : IEnvironmentNameResolver
{
    public string? ResolveEnvironmentName(string environmentName)
    {
        if (string.Equals(environmentName, Environments.Development, StringComparison.OrdinalIgnoreCase))
        {
            return "dev";
        }

        if (string.Equals(environmentName, Environments.Staging, StringComparison.OrdinalIgnoreCase))
        {
            return "stage";
        }

        if (string.Equals(environmentName, Environments.Production, StringComparison.OrdinalIgnoreCase))
        {
            return "prod";
        }

        return null;
    }
}
