using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace ScHelix.Foundation.HelixCore.Methods {
    public static class GetTypes {
        public static IEnumerable<Type> GetTypesImplementing<T>(params Assembly[] assemblies) {
            if (assemblies == null || assemblies.Length == 0) {
                return new Type[0];
            }

            Type targetType = typeof(T);

            return assemblies
                .Where(assembly => !assembly.IsDynamic)
                .SelectMany(GetExportedTypes)
                .Where(type => !type.IsAbstract && !type.IsGenericTypeDefinition && targetType.IsAssignableFrom(type))
                .ToArray();
        }

        public static IEnumerable<Type> GetTypesImplementing(Type implementsType, IEnumerable<Assembly> assemblies, params string[] classFilter) {
            IEnumerable<Type> types = GetTypesImplementing(implementsType, assemblies.ToArray());

            if (classFilter != null && classFilter.Any()) {
                types = types.Where(type => classFilter.Any(filter => GetAssemblies.IsWildcardMatch(type.FullName, filter)));
            }

            return types;
        }

        private static IEnumerable<Type> GetTypesImplementing(Type implementsType, params Assembly[] assemblies) {
            if (assemblies == null || assemblies.Length == 0) {
                return new Type[0];
            }

            Type targetType = implementsType;

            return assemblies
                .Where(assembly => !assembly.IsDynamic)
                .SelectMany(GetExportedTypes)
                .Where(type => !type.IsAbstract && !type.IsGenericTypeDefinition && targetType.IsAssignableFrom(type))
                .ToArray();
        }

        public static IEnumerable<Type> GetExportedTypes(Assembly assembly) {
            try {
                return assembly.GetExportedTypes();
            } catch (NotSupportedException) {
                // A type load exception would typically happen on an Anonymously Hosted DynamicMethods
                // Assembly and it would be safe to skip this exception.
                return Type.EmptyTypes;
            } catch (ReflectionTypeLoadException ex) {
                // Return the types that could be loaded. Types can contain null values.
                return ex.Types.Where(type => type != null);
            } catch (Exception ex) {
                // Throw a more descriptive message containing the name of the assembly.
                throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture,
                    "Unable to load types from assembly {0}. {1}", assembly.FullName, ex.Message), ex);
            }
        }
    }
}
