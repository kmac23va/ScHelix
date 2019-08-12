using System;
using System.Collections.Generic;
using System.Reflection;
using Glass.Mapper.IoC;
using Glass.Mapper.Maps;
using ScHelix.Foundation.HelixCore.Methods;

namespace ScHelix.Foundation.HelixCore.ORM.Extensions {
    public static class MapsConfigFactoryExtension {
        public static void AddFluentMaps(this IConfigFactory<IGlassMap> mapsConfigFactory, params string[] assemblyFilters) {
            Assembly[] assemblies = GetAssemblies.GetByFilter(assemblyFilters);
            AddFluentMaps(mapsConfigFactory, assemblies);
        }

        private static void AddFluentMaps(this IConfigFactory<IGlassMap> mapsConfigFactory, params Assembly[] assemblies) {
            IEnumerable<Type> mappings = GetTypes.GetTypesImplementing<IGlassMap>(assemblies);

            foreach (Type map in mappings) {
                mapsConfigFactory.Add(() => Activator.CreateInstance(map) as IGlassMap);
            }
        }
    }
}
