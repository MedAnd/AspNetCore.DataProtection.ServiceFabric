ASP.NET Core DataProtection for Service Fabric
==============================================
A ready to use ASP.NET Core DataProtection Service Fabric microservice, used for protecting and unprotecting sensitive data such as tokens and cookies in a multi-node & load balanced environment.


### Getting started - Installing the DataProtection microservice

Open your existing Service Fabric .NET Core solution in Visual Studio 2017, then in Solution Explorer right-click on the solution node and select the Manage Nuget Packages for Solution menu. Search for the AspNetCore.DataProtection.ServiceFabric package, make sure to check the master Service Fabric application project and click the Install button. After the installation completes, in the ApplicationPackageRoot there will be a new AspNetCore.DataProtection.ServiceFabricPkg folder.

The AspNetCore.DataProtection.ServiceFabric microservice can also be installed via the Package Manager Console:

```
Install-Package AspNetCore.DataProtection.ServiceFabric
```

Before you publish to Service Fabric though, make sure to check the ApplicationManifest.xml file, setting the AspNetCore.DataProtection.ServiceFabric TargetReplicaSetSize and MinReplicaSetSize relevant for your cluster or development environment. Note the PartitionCount and InstanceCount should both be set to 1.


### Using the DataProtection microservice in your ASP.NET Core projects

To use the AspNetCore.DataProtection.ServiceFabric microservice in your ASP.NET Core projects, simply install the AspNetCore.DataProtection.ServiceFabric.Common NuGet library:

```
Install-Package AspNetCore.DataProtection.ServiceFabric.Common
```

In the ConfigureServices method of your Startup.cs, don't forget to add:

```csharp
// Add Service Fabric DataProtection
services.AddDataProtection()
    .SetApplicationName("ServiceFabric-DataProtection-Your-App-Name")
    .PersistKeysToServiceFabric(dataProtectionServiceUri);
```

Where dataProtectionServiceUri is the uri to your instance of the AspNetCore.DataProtection.ServiceFabric microservice, for example "fabric:/ServiceFabric.DataProtection/DataProtectionService".
You'll also need these two using statements:

```csharp
using AspNetCore.DataProtection.ServiceFabric.Extensions;
using Microsoft.AspNetCore.DataProtection;
```


You can find documentation for .NET Core Data Protection in the [ASP.NET Core Documentation](http://docs.asp.net/en/latest/security/data-protection/index.html).

The AspNetCore.DataProtection.ServiceFabric project is packaged using Microsoft's SFNuGet, which allows you to package and share Azure Service Fabric services as NuGet packages. For more info head over to [SFNuGet](https://github.com/Azure/SFNuGet).