using Sitecore.Configuration;

namespace ScHelix.Foundation.HelixCore.Wildcards {
    public class WildcardManager {
        private static readonly ProviderHelper<RouteResolver, RouteResolverCollection> Configuration;

        static WildcardManager() {
            Configuration = new ProviderHelper<RouteResolver, RouteResolverCollection>("wildcardManager");
        }

        public static RouteResolver Current => Configuration.Provider;
        public static RouteResolverCollection Providers => Configuration.Providers;
    }
}
