#region GlassMapperSc generated code
/*************************************

DO NOT CHANGE THIS FILE - UPDATE GlassMapperScCustom.cs

**************************************/

using System.Collections.Generic;
using System.Linq;
using Glass.Mapper;
using Glass.Mapper.Configuration;
using Glass.Mapper.Maps;
using Glass.Mapper.Sc.Configuration.Fluent;
using Glass.Mapper.Sc.IoC;

namespace ScHelix.Foundation.HelixCore.ORM {
    public class GlassMapperSc : Glass.Mapper.Sc.Pipelines.Initialize.GlassMapperSc {
        public override IDependencyResolver CreateResolver() {
            IDependencyResolver resolver = GlassMapperScCustom.CreateResolver();
            base.CreateResolver(resolver);

            return resolver;
        }

        public override IConfigurationLoader[] GetGlassLoaders(Context context) {
            IEnumerable<IConfigurationLoader> loaders1 = GlassMapperScCustom.GlassLoaders();
            IConfigurationLoader[] loaders2 = base.GetGlassLoaders(context);

            return loaders1.Concat(loaders2).ToArray();
        }

        public override void LoadConfigurationMaps(IDependencyResolver resolver, Context context) {
            if (!(resolver is DependencyResolver dependencyResolver)) {
                return;
            }

            if (dependencyResolver.ConfigurationMapFactory is ConfigurationMapConfigFactory) {
                GlassMapperScCustom.AddMaps(dependencyResolver.ConfigurationMapFactory);
            }

            IConfigurationMap configurationMap = new ConfigurationMap(dependencyResolver);
            SitecoreFluentConfigurationLoader configurationLoader = configurationMap.GetConfigurationLoader<SitecoreFluentConfigurationLoader>();
            context.Load(configurationLoader);

            base.LoadConfigurationMaps(resolver, context);
        }

        public override void PostLoad(IDependencyResolver dependencyResolver) {
            GlassMapperScCustom.PostLoad();
            base.PostLoad(dependencyResolver);
        }

    }
}

#endregion
