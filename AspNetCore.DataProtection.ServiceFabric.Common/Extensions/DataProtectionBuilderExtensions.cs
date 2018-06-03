using AspNetCore.DataProtection.ServiceFabric.Repositories;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.DataProtection.Repositories;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace AspNetCore.DataProtection.ServiceFabric.Extensions
{
    public static class DataProtectionBuilderExtensions
    {
        public static IDataProtectionBuilder PersistKeysToServiceFabric(this IDataProtectionBuilder builder, string serviceUri)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            if (string.IsNullOrEmpty(serviceUri))
            {
                throw new ArgumentNullException(nameof(serviceUri));
            }

            return builder.Use(ServiceDescriptor.Singleton<IXmlRepository>(services => new ServiceFabricXmlRepository(serviceUri)));
        }

        public static IDataProtectionBuilder Use(this IDataProtectionBuilder builder, ServiceDescriptor descriptor)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            if (descriptor == null)
            {
                throw new ArgumentNullException(nameof(descriptor));
            }

            for (int i = builder.Services.Count - 1; i >= 0; i--)
            {
                if (builder.Services[i]?.ServiceType == descriptor.ServiceType)
                {
                    builder.Services.RemoveAt(i);
                }
            }

            builder.Services.Add(descriptor);

            return builder;
        }
    }
}
