using System.Reflection;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using Calabonga.Portal.Config;
using Northwind.Data;
using Northwind.Web.Infrastructure;

namespace Northwind.Web {

    /// <summary>
    /// Dependancy Container initialization
    /// Using Autofac DI-container
    /// </summary>
    public static class DependencyContainerConfig {

        public static IContainer InitializeAutofac() {

            var builder = new ContainerBuilder();

            // Common
            builder.RegisterControllers(Assembly.GetExecutingAssembly()).PropertiesAutowired();
            builder.RegisterModelBinders(Assembly.GetExecutingAssembly());
            builder.RegisterAssemblyTypes(typeof(MvcApplication).Assembly).AsImplementedInterfaces();
            builder.RegisterModule(new AutofacWebTypesModule());
            builder.RegisterFilterProvider();
            builder.RegisterSource(new ViewRegistrationSource());
            
            builder.RegisterType<ConfigServiceBase<CurrentAppSettings>>().As<IConfigService<CurrentAppSettings>>().SingleInstance();
            builder.RegisterType<JsonConfigSerializer>().As<IConfigSerializer>();
            builder.RegisterType<CacheService>().As<ICacheService>();
            builder.RegisterType<EmailService>().As<IEmailService>();
            
            builder.RegisterType<NorthwindDbContext>().As<IContext>().InstancePerRequest();


            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
                .Where(x => x.Name.EndsWith("Service"))
                .AsImplementedInterfaces()
                .InstancePerRequest();

            
            ServicesConfig.Register(builder);
            var container = builder.Build();
            
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
            return container;
        }
    }
}