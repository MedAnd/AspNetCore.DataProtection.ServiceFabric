using AspNetCore.DataProtection.ServiceFabric.Repositories;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
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

            builder.Services.Configure<KeyManagementOptions>(options =>
            {
                options.XmlRepository = new ServiceFabricXmlRepository(serviceUri);
            });

            return builder;
        }
    }
}
