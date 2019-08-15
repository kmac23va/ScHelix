using Sitecore.Configuration;

namespace ScHelix.Foundation.HelixCore.Wildcards.UrlGeneration.TokenValueExtraction {
    public class UrlGenerationTokenValueExtractor {
        private static readonly ProviderHelper<TokenValueExtractor, TokenValueExtractorCollection> Configuration;

        static UrlGenerationTokenValueExtractor() {
            Configuration = new ProviderHelper<TokenValueExtractor, TokenValueExtractorCollection>("urlGenerationTokenValueExtractor");
        }

        public static TokenValueExtractor Current => Configuration.Provider;

        public static TokenValueExtractorCollection Providers => Configuration.Providers;
    }
}
