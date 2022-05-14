#!/bin/bash

# Ensure dotnet sdk is installed
wget https://packages.microsoft.com/config/ubuntu/21.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
sudo dpkg -i packages-microsoft-prod.deb
rm packages-microsoft-prod.deb

sudo apt-get update; \
sudo apt-get install -y apt-transport-https && \
sudo apt-get update && \
sudo apt-get install -y dotnet-sdk-6.0

# Ensure zip is installed
sudo apt install zip -y

ProjectRootDirectory="${0%/*}"
cd $ProjectRootDirectory # Set script location as working dir
cd ./WebAPI

if [[ -d ./out ]]; then
    echo "Output directory already exists, removing directory..."
    rm -r ./out
fi

echo "Building deployment"
dotnet publish ./WebAPI.csproj -c Release -o out
cd ./out

echo "Zipping deployment"
zip -r ../../deployment.zip ./*

echo "Done"






