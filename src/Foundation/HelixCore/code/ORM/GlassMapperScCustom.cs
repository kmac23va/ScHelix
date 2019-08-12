#region GlassMapperScCustom generated code

using System.Collections.Generic;
using Glass.Mapper.Configuration;
using Glass.Mapper.IoC;
using Glass.Mapper.Maps;
using Glass.Mapper.Sc;
using Glass.Mapper.Sc.IoC;
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

        public static void PostLoad() {
            //Remove the comments to activate CodeFist
            /* CODE FIRST START
            var dbs = Sitecore.Configuration.Factory.GetDatabases();
            foreach (var db in dbs)
            {
                var provider = db.GetDataProviders().FirstOrDefault(x => x is GlassDataProvider) as GlassDataProvider;
                if (provider != null)
                {
                    using (new SecurityDisabler())
                    {
                        provider.Initialise(db);
                    }
                }
            }
             * CODE FIRST END
             */
        }

        public static void AddMaps(IConfigFactory<IGlassMap> mapsConfigFactory) {
            mapsConfigFactory.AddFluentMaps("*.Foundation.*");
            mapsConfigFactory.AddFluentMaps("*.Feature.*");
            mapsConfigFactory.AddFluentMaps("*.Project.*");
        }
    }
}

#endregion
