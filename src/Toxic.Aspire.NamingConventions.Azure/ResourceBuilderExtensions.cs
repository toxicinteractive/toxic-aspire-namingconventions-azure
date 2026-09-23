using Microsoft.Extensions.DependencyInjection;

namespace Toxic.Aspire.NamingConventions.Azure;

public static class ResourceBuilderExtensions
{
    extension<TResource>(IResourceBuilder<TResource> builder) where TResource : IResource
    {
        /// <summary>
        /// Set a specific workload name for this resource when deployed to Azure.
        /// Setting "cms" for a container app resource will result in the following name: "ca-projectname-cms-...".
        /// Can also override the entire resource name and ignore name resolvers.
        /// </summary>
        /// <param name="name">A specific resource workload name distinguisher.</param>
        /// <param name="overrideResourceName">If true will ignore all registered name resolvers and set the resource name to the name specified.</param>
        public IResourceBuilder<TResource> WithAzureWorkloadName(string name, bool overrideResourceName = false)
        {
            // there's no connection between a resource builder and the infrastructure resolver so we need to
            // register a mediator object that holds the resource name and the workload name
            builder
                .ApplicationBuilder
                .Services
                .AddKeyedSingleton<ResourceWorkloadNameAssociation>(
                    builder.Resource.Name.Replace("-", "_"), new ResourceWorkloadNameAssociation
                    {
                        ResourceType = typeof(TResource),
                        ResourceName = builder.Resource.Name,
                        AzureWorkloadName = name,
                        IgnoreNameResolvers = overrideResourceName
                    });

            return builder;
        }
    }
}
