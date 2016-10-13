using System.Linq;
using System.Reflection;
using Autofac;

namespace Northwind.Web
{
    /// <summary>
    /// Service layer registeration helper
    /// </summary>
    internal static class ServicesConfig {
        /// <summary>
        /// Adds services 
        /// </summary>
        /// <param name="builder"></param>
        internal static void Register(ContainerBuilder builder) {

            var types = typeof(MvcApplication).Assembly.GetTypes();

            // overrides for default services registration
            var serviceSettings = (from type in types
                                   where !type.GetTypeInfo().IsAbstract && type.Name.EndsWith("Service")
                                   select type).ToList();
            if (!serviceSettings.Any()) return;
            foreach (var type in serviceSettings) {
                builder.RegisterType(type).InstancePerRequest();
            }
        }
    }
}
