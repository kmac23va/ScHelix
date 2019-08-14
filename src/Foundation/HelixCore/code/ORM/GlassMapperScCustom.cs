#region GlassMapperScCustom generated code

using System.Collections.Generic;
using Glass.Mapper.Configuration;
using Glass.Mapper.IoC;
using Glass.Mapper.Maps;
using Glass.Mapper.Sc;
using Glass.Mapper.Sc.IoC;
using Glass.Mapper.Sc.Pipelines.Response;
using ScHelix.Foundation.HelixCore.ORM.Extensions;
using IDependencyResolver = Glass.Mapper.Sc.IoC.IDependencyResolver;

namespace ScHelix.Foundation.HelixCore.ORM {
    public static class GlassMapperScCustom {
        public static IDependencyResolver CreateResolver() {
            Config config = new Config();

            DependencyResolver dependencyResolver = new DependencyResolver(config);
            // add any changes to the standard resolver here
            return dependencyResolver;
        }

        public static IEnumerable<IConfigurationLoader> GlassLoaders() =>
            new IConfigurationLoader[] {
            };

        public static void PostLoad() =>
            GetModelFromView.ViewTypeResolver = new ChainedViewTypeResolver(new IViewTypeResolver[] {
                new CompiledViewTypeFinder(), new RegexViewTypeResolver()
            });

        public static void AddMaps(IConfigFactory<IGlassMap> mapsConfigFactory) {
            mapsConfigFactory.AddFluentMaps("*.Foundation.*");
            mapsConfigFactory.AddFluentMaps("*.Feature.*");
            mapsConfigFactory.AddFluentMaps("*.Project.*");
        }
    }
}

#endregion
