using Toxic.Aspire.NamingConventions.Azure;
using Toxic.Aspire.NamingConventions.Azure.AppHost.CustomNameResolvers;
using Toxic.Aspire.NamingConventions.Azure.NameResolvers;
using Azure.Provisioning.Storage;
using Microsoft.Extensions.DependencyInjection;

/**
    Run `aspire publish` and inspect the bicep files to see the generated names.
    See the readme for more information and customization.
**/

// enable resource naming in publish mode
// note that the default region used is defined in appsettings.json/env variable
var builder = DistributedApplication
    .CreateBuilder(args)
    .WithAzureNamingConvention("project");
    // use this overload to specify the pattern for the generated name (see readme for available segments)
    //.WithAzureNamingConvention("project", "{Prefix}{Workload}{Env}");

// this will generate the following name: "cae-project-prod-euw"
// it will also generate names for the auxiliary resources that aspire creates, the log workspace and the registry
builder
    .AddAzureContainerAppEnvironment("container-env");

builder
    .AddProject<Projects.Toxic_Aspire_NamingConventions_Azure_SampleApp>("api")
    // this will generate the following name: "ca-project-appname-prod-euw"
    // this is optional but must be used to identify multiple resources of the same type
    .WithAzureWorkloadName("appname")
    // use this overload to use the exact name you supply for the whole resource name, overriding the convention
    //.WithAzureWorkloadName("my-azure-resource-name", true)
    .WithExternalHttpEndpoints()
    .PublishAsAzureContainerApp((_, _) => {});

// this will use a custom name resolver for Azure storage accounts
builder
    .Services
    .AddSingleton<IResourceNameResolver<StorageAccount>, CustomStorageAccountNameResolver>();
builder
    .AddAzureStorage("storage")
    .RunAsEmulator();

builder
    .Build()
    .Run();
