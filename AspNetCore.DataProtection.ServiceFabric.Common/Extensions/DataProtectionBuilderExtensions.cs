using AspNetCore.DataProtection.ServiceFabric.Repositories;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.DataProtection.Repositories;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace AspNetCore.DataProtection.ServiceFabric.Extensions
{
    public static class DataProtectionBuilderExtensions
    {
        /// <summary>
        /// Config DataProtection provider with ServiceFabric
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="serviceUri">service uri of AspNetCore.DataProtection.ServiceFabric, ex: "fabric:/ServiceFabric.DataProtection/DataProtectionService" </param>
        /// <returns></returns>
        public static IDataProtectionBuilder PersistKeysToServiceFabric(this IDataProtectionBuilder builder,
            string serviceUri = "fabric:/ServiceFabric.DataProtection/DataProtectionService")
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.Services.Configure<KeyManagementOptions>(options =>
            {
                options.XmlRepository = new ServiceFabricXmlRepository(serviceUri);
            });
            return builder;
        }
    }
}
