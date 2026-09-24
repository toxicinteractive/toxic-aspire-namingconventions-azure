# Toxic.Aspire.NamingConventions.Azure
## How to develop
1. Spin up the devcontainer
2. Log in to an Azure dev subscription with your IDE or directly with `az login`
3. Test name generation by running `aspire publish`
4. Test remote name deployment with `aspire deploy`

## How to push a new version
1. Update the version number in Toxic.Aspire.NamingConventions.Azure.csproj. The major version number of the package version aligns with the corresponding Aspire version.
2. Create a git tag with the version number
3. Run the publishing workflow in GitHub

## Missing features
* Tests
