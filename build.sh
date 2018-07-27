#!/bin/bash
DIR=`dirname $0`
ROOTDIR=$(pwd)
source $DIR/dotnet-include.sh

dotnet restore $DIR/ServiceFabric.DataProtection.sln -s https://api.nuget.org/v3/index.json
dotnet build $DIR/ServiceFabric.DataProtection.sln -v normal

cd $ROOTDIR/AspNetCore.DataProtection.ServiceFabric/
   dotnet publish -o ../ServiceFabric.DataProtection/Pkg/AspNetCore.DataProtection.ServiceFabricPkg/Code -r ubuntu.16.04-x64
   cp -R ./PackageRoot/* ../ServiceFabric.DataProtection/Pkg/AspNetCore.DataProtection.ServiceFabricPkg/
cd $ROOTDIR/ServiceFabric.DataProtection.Web/
   dotnet publish -o ../ServiceFabric.DataProtection/Pkg/ServiceFabric.DataProtection.WebPkg/Code -r ubuntu.16.04-x64
   cp -R ./PackageRoot/* ../ServiceFabric.DataProtection/Pkg/ServiceFabric.DataProtection.WebPkg/
