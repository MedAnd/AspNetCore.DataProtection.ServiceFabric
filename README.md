ASP.NET Core DataProtection for Service Fabric
==============================================
A ready to use ASP.NET Core DataProtection Service Fabric microservice, used for protecting and unprotecting sensitive data such as tokens and cookies in a multi-node & load balanced environment.

### Getting started - Installing the DataProtection microservice

Open your existing Service Fabric .NET Core solution in Visual Studio 2017, then in Solution Explorer right-click on the solution node and select the Manage Nuget Packages for Solution menu. Search for the AspNetCore.DataProtection.ServiceFabric package, check the application project and click the Install button. After the installation completes, publish to Service Fabric and you'll have a fully working stateful microservice!

The AspNetCore.DataProtection.ServiceFabric microservice can also be installed via the Package Manager Console:

```
Install-Package AspNetCore.DataProtection.ServiceFabric
```

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
    .PersistKeysToServiceFabric();
```

You can find documentation for .NET Core Data Protection in the [ASP.NET Core Documentation](http://docs.asp.net/en/latest/security/data-protection/index.html).

The AspNetCore.DataProtection.ServiceFabric project is packaged using Microsoft's SFNuGet, which allows you to package and share Azure Service Fabric services as NuGet packages. For more info head over to [SFNuGet](https://github.com/Azure/SFNuGet).