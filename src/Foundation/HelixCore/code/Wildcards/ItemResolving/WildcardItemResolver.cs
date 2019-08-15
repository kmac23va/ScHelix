using Sitecore.Configuration;

namespace ScHelix.Foundation.HelixCore.Wildcards.ItemResolving {
    public class WildcardItemResolver {
        private static readonly ProviderHelper<ItemResolver, ItemResolverCollection> Configuration;

        static WildcardItemResolver() {
            Configuration = new ProviderHelper<ItemResolver, ItemResolverCollection>("wildcardItemResolver");
        }

        public static ItemResolver Current => Configuration.Provider;

        public static ItemResolverCollection Providers => Configuration.Providers;
    }
}
