namespace Toxic.Aspire.NamingConventions.Azure.NameResolvers;

/// <summary>
/// Translates between an Asp.Net environment string and a resource environment name suffix.
/// </summary>
public interface IEnvironmentNameResolver
{
    string? ResolveEnvironmentName(string environmentName);
}
