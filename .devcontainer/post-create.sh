#!/usr/bin/env bash

# this is run on "postCreateCommand" in the container AFTER the container is CREATED for the first time
# https://containers.dev/implementors/json_reference/

mkdir -p $AZURE_CONFIG_DIR
sudo chown $USER:$USER $AZURE_CONFIG_DIR

dotnet new install Aspire.ProjectTemplates
dotnet tool install -g Aspire.Cli
dotnet tool install -g dotnet-outdated-tool
