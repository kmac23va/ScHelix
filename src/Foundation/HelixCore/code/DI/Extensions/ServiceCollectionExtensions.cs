using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using Microsoft.Extensions.DependencyInjection;
using GetAssemblies = ScHelix.Foundation.HelixCore.Methods.GetAssemblies;
using GetTypes = ScHelix.Foundation.HelixCore.Methods.GetTypes;

namespace ScHelix.Foundation.HelixCore.DI.Extensions {
    public static class ServiceCollectionExtensions {
        private const string DefaultControllerFilter = "*Controller";

        public static void AddClassesWithServiceAttribute(this IServiceCollection serviceCollection, params string[] assemblyFilters) {
            Assembly[] assemblies = GetAssemblies.GetByFilter(assemblyFilters);
            serviceCollection.AddClassesWithServiceAttribute(assemblies);
        }

        public static void AddMvcControllers(this IServiceCollection serviceCollection, params string[] assemblyFilters) => serviceCollection.AddMvcControllers(GetAssemblies.GetByFilter(assemblyFilters));

        private static void AddClassesWithServiceAttribute(this IServiceCollection serviceCollection, params Assembly[] assemblies) {
            var typesWithAttributes = assemblies
                .Where(assembly => !assembly.IsDynamic)
                .SelectMany(GetTypes.GetExportedTypes)
                .Where(type => !type.IsAbstract && !type.IsGenericTypeDefinition)
                .Select(type => new {
                    type.GetCustomAttribute<ServiceAttribute>()?.Lifetime,
                    ServiceType = type,
                    ImplementationType = type.GetCustomAttribute<ServiceAttribute>()?.ServiceType
                })
                .Where(t => t.Lifetime != null);

            foreach (var type in typesWithAttributes) {
                if (type.ImplementationType == null) {
                    Debug.Assert(type.Lifetime != null, "type.Lifetime != null");
                    serviceCollection.Add(type.ServiceType, type.Lifetime.Value);
                } else {
                    Debug.Assert(type.Lifetime != null, "type.Lifetime != null");
                    serviceCollection.Add(type.ImplementationType, type.ServiceType, type.Lifetime.Value);
                }
            }
        }

        private static void Add(this IServiceCollection serviceCollection, Type type, Lifetime lifetime) {
            switch (lifetime) {
                case Lifetime.Singleton:
                    serviceCollection.AddSingleton(type);
                    break;
                case Lifetime.Transient:
                    serviceCollection.AddTransient(type);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(lifetime), lifetime, null);
            }
        }

        private static void Add(this IServiceCollection serviceCollection, Type serviceType, Type implementationType, Lifetime lifetime) {
            switch (lifetime) {
                case Lifetime.Singleton:
                    serviceCollection.AddSingleton(serviceType, implementationType);
                    break;
                case Lifetime.Transient:
                    serviceCollection.AddTransient(serviceType, implementationType);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(lifetime), lifetime, null);
            }
        }

        private static void AddMvcControllers(this IServiceCollection serviceCollection, params Assembly[] assemblies) =>
            serviceCollection.AddMvcControllers(assemblies, new[] {
                DefaultControllerFilter
            });

        private static void AddMvcControllers(this IServiceCollection serviceCollection, IEnumerable<Assembly> assemblies, string[] classFilters) {
            IEnumerable<Type> controllers = GetTypes.GetTypesImplementing(typeof(IController), assemblies, classFilters);

            foreach (Type controller in controllers) {
                serviceCollection.Add(controller, Lifetime.Transient);
            }
        }
    }
}
