using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.DataProtection.Repositories;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace ServiceFabric.DataProtection.Web
{
    /// <summary>
    /// 
    /// </summary>
    public static class DataProtectionBuilderExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IDataProtectionBuilder PersistKeysToServiceFabric(this IDataProtectionBuilder builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            return builder.Use(ServiceDescriptor.Singleton<IXmlRepository>(services => new ServiceFabricXmlRepository()));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="descriptor"></param>
        /// <returns></returns>
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
