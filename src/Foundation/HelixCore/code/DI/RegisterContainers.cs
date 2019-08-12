using Glass.Mapper.Sc;
using Glass.Mapper.Sc.Web;
using Glass.Mapper.Sc.Web.Mvc;
using ScHelix.Foundation.HelixCore.DI.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Sitecore.DependencyInjection;

namespace ScHelix.Foundation.HelixCore.DI {
    public class RegisterContainers : IServicesConfigurator {
        public void Configure(IServiceCollection serviceCollection) {
            //Controllers
            serviceCollection.AddMvcControllers("*.Foundation.*");
            serviceCollection.AddMvcControllers("*.Feature.*");
            serviceCollection.AddMvcControllers("*.Project.*");

            //Classes
            serviceCollection.AddClassesWithServiceAttribute("*.Foundation.*");
            serviceCollection.AddClassesWithServiceAttribute("*.Feature.*");
            serviceCollection.AddClassesWithServiceAttribute("*.Project.*");

            //Glass Mapper
            serviceCollection.AddTransient<ISitecoreService>(provider => new SitecoreService(Sitecore.Context.Database));
            serviceCollection.AddScoped<IRequestContext, RequestContext>();
            serviceCollection.AddScoped<IMvcContext, MvcContext>();
            serviceCollection.AddScoped<IGlassHtml, GlassHtml>();
        }
    }
}
